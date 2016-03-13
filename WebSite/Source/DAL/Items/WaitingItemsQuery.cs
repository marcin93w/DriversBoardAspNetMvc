using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Driver.WebSite.Models;

namespace Driver.WebSite.DAL.Items
{
    public class WaitingItemsQuery : ItemsQuery
    {
        public WaitingItemsQuery(ApplicationDbContext context) : base(context)
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
                       where !item.DisplayOnHomePage
                       orderby item.DateAdded descending
                       select item;
            }
        }
    }
}
