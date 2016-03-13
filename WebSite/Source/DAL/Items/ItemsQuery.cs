using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Driver.WebSite.Models;

namespace Driver.WebSite.DAL.Items
{
    public abstract class ItemsQuery : IItemsQuery
    {
        protected ApplicationDbContext Context { get; }

        protected ItemsQuery(ApplicationDbContext context)
        {
            Context = context;
            context.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
        }

        protected abstract IQueryable<Item> Query { get; } 

        public virtual async Task<IEnumerable<Item>> GetItems(string userId, int startingFrom, int limit)
        {
            var queryWithSkip = Query;
            if (startingFrom > 0)
                queryWithSkip = queryWithSkip.Skip(startingFrom);

            var items = await queryWithSkip.Take(limit)
                .ToArrayAsync();

            return await LoadVotes(items, userId);
        }

        public virtual async Task<bool> AreThereOlderItems(DateTime time)
        {
            return await Query.Where(i => i.DateAdded < time).AnyAsync();
        }

        private async Task<IEnumerable<Item>> LoadVotes(Item[] items, string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var itemsIds = items.Select(i => i.Id).ToArray();
                var votesQuery =
                    from itemVote in Context.Set<ItemVote>()
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
                from commentVote in Context.Set<CommentVote>()
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
                from driverOccurrenceVote in Context.Set<DriverOccurrenceVote>()
                where driverOccurrenceVote.User.Id == userId
                    && driverOccurrenceVote.Votable.Item.Id == item.Id
                select driverOccurrenceVote;

            var votes = await votesQuery.ToArrayAsync();

            foreach (var driverOccurrence in item.DriversOccurrences)
            {
                driverOccurrence.Votes = votes.Where(v => v.Votable.Id == driverOccurrence.Id).ToArray();
            }
        }
    }
}
