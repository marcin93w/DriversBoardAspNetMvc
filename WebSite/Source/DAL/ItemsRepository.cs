using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Driver.WebSite.Models;

namespace Driver.WebSite.DAL
{
    public class ItemsRepository : IItemsRepository
    {
        private readonly ApplicationDbContext _context;

        public ItemsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Item>> GetAllItems(string userId)
        {
            var data = await (from item in _context.Items
                               orderby item.DateAdded descending
                               select new { Item = item, AuthorLogin = item.Author.UserName })
                    .ToArrayAsync();
            var items = data.Select(i =>
            {
                i.Item.Author = new ApplicationUser { UserName = i.AuthorLogin };
                return i.Item;
            });

            return await LoadVotes(items.ToArray(), userId);
        }

        public async Task<Item> GetItem(int itemId, string userId)
        {
            var data = await (from item in _context.Items.Include(i => i.Comments)
                               where item.Id == itemId
                               orderby item.DateAdded descending
                               select new { Item = item, AuthorLogin = item.Author.UserName })
                   .ToArrayAsync();
            var items = data.Select(i =>
            {
                i.Item.Author = new ApplicationUser {UserName = i.AuthorLogin};
                return i.Item;
            });

            return (await LoadVotes(items.ToArray(), userId)).First();
        }

        private async Task<IEnumerable<Item>> LoadVotes(Item[] items, string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var itemsIds = items.Select(i => i.Id).ToArray();
                var votesQuery =
                    from itemVote in _context.Set<ItemVote>()
                    where itemVote.User.Id == userId
                        && itemsIds.Contains(itemVote.Item.Id)
                    select itemVote;

                var votes = await votesQuery.ToArrayAsync();

                foreach (var item in items)
                {
                    item.Votes = votes.Where(v => v.Item == item).ToArray();
                }
            }

            return items;
        }
    }
}
