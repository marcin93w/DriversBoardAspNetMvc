using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Driver.WebSite.Models;

namespace Driver.WebSite.DAL.Items
{
    public class TopItemsQuery : ItemsQuery
    {
        public TopItemsQuery(ApplicationDbContext context) : base(context)
        {
        }

        protected override IQueryable<Item> Query
        {
            get
            {
                var source = Context.Items.Include(i => i.Author)
                    .Include(i => i.DriversOccurrences)
                    .Include(i => i.DriversOccurrences.Select(o => o.Driver));
                return from item in source
                       orderby item.UpVotesCount - item.DownVotesCount descending, item.Id
                       select item;
            }
        }

        public override async Task<bool> AreThereNextItems(Item lastItem)
        {
            return await Query
                .Where(i => 
                    (
                        i.UpVotesCount - i.DownVotesCount < lastItem.UpVotesCount - lastItem.DownVotesCount
                    ) 
                    || 
                    (
                        (
                            i.UpVotesCount - i.DownVotesCount == lastItem.UpVotesCount - lastItem.DownVotesCount
                        )
                        && 
                            i.Id > lastItem.Id
                    )
                ).AnyAsync();
        }
    }
}
