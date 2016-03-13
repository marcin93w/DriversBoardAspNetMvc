using System.Data.Entity;
using System.Linq;
using Driver.WebSite.Models;

namespace Driver.WebSite.DAL.Items
{
    public class SingleItemQuery : ItemsQuery
    {
        private readonly int _itemId;

        public SingleItemQuery(ApplicationDbContext context, int itemId) : base(context)
        {
            _itemId = itemId;
        }

        protected override IQueryable<Item> Query
        {
            get
            {
                var source = Context.Items.Include(i => i.Author)
                   .Include(i => i.DriversOccurrences)
                   .Include(i => i.DriversOccurrences.Select(o => o.Driver))
                   .Include(i => i.Comments)
                   .Include(i => i.Comments.Select(c => c.User));
                return from item in source
                    where item.Id == _itemId
                    select item;
            }
        }
    }
}
