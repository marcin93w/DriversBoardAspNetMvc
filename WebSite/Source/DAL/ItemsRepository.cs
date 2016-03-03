using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Driver.WebSite.Models;

namespace Driver.WebSite.DAL
{
    public class ItemsRepository : IItemsRepository
    {
        private readonly ApplicationDbContext _context;

        public ItemsRepository(ApplicationDbContext context)
        {
            _context = context;
            context.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
        }

        public async Task<IEnumerable<Item>> GetAllItems(string userId)
        {
            var source = _context.Items.Include(i => i.Author)
                .Include(i => i.DriversOccurrences)
                .Include(i => i.DriversOccurrences.Select(o => o.Driver));
            var items = await (from item in source
                               orderby item.DateAdded descending
                               select item).ToArrayAsync();

            return await LoadVotes(items, userId);
        }

        public async Task<Item> GetItem(int itemId, string userId)
        {
            var source = _context.Items.Include(i => i.Author)
                .Include(i => i.DriversOccurrences)
                .Include(i => i.DriversOccurrences.Select(o => o.Driver))
                .Include(i => i.Comments)
                .Include(i => i.Comments.Select(c => c.User));
            var items = await (from item in source
                               where item.Id == itemId
                               orderby item.DateAdded descending
                               select item).ToArrayAsync();

            return (await LoadVotes(items, userId)).First();
        }

        private async Task<IEnumerable<Item>> LoadVotes(Item[] items, string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var itemsIds = items.Select(i => i.Id).ToArray();
                var votesQuery =
                    from itemVote in _context.Set<ItemVote>()
                    where itemVote.User.Id == userId
                        && itemsIds.Contains(itemVote.Votable.Id)
                    select itemVote;

                var votes = await votesQuery.ToArrayAsync();

                foreach (var item in items)
                {
                    item.Votes = votes.Where(v => v.Votable == item).ToArray();
                    if (item.Comments != null && item.Comments.Count > 0)
                    {
                        await LoadCommentsVotes(item, userId);
                    }
                    if (item.DriversOccurrences != null && item.DriversOccurrences.Count > 0)
                    {
                        await LoadDriverOccurrenceVotes(item, userId);
                    }
                }
            }

            return items;
        }

        private async Task LoadCommentsVotes(Item item, string userId)
        {
            var votesQuery =
                from commentVote in _context.Set<CommentVote>()
                where commentVote.User.Id == userId
                    && commentVote.Votable.Item.Id == item.Id
                select commentVote;

            var votes = await votesQuery.ToArrayAsync();

            foreach (var comment in item.Comments)
            {
                comment.CommentVotes = votes.Where(v => v.Votable.Id == comment.Id).ToArray();
            }
        }

        private async Task LoadDriverOccurrenceVotes(Item item, string userId)
        {
            var votesQuery =
                from driverOccurrenceVote in _context.Set<DriverOccurrenceVote>()
                where driverOccurrenceVote.User.Id == userId
                    && driverOccurrenceVote.Votable.Item.Id == item.Id
                select driverOccurrenceVote;

            var votes = await votesQuery.ToArrayAsync();

            foreach (var driverOccurrence in item.DriversOccurrences)
            {
                driverOccurrence.Votes = votes.Where(v => v.Votable.Id == driverOccurrence.Id).ToArray();
            }
        }

        public async Task<int> AddItem(Item item)
        {
            _context.Set<ApplicationUser>().Attach(item.Author);

            var alreadyExistingDrivers = item.DriversOccurrences
                .Select(driverOccurrence => driverOccurrence.Driver)
                .Where(driver => driver.Id != default(int));
            foreach (var driver in alreadyExistingDrivers)
            {
                _context.Set<Models.Driver>().Attach(driver);
            }

            _context.Items.Add(item);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> AddComment(Comment comment)
        {
            _context.Set<ApplicationUser>().Attach(comment.User);
            _context.Comments.Add(comment);
            return await _context.SaveChangesAsync();
        }
    }
}
