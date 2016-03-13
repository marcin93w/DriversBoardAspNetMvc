using System.Linq;
using System.Threading.Tasks;
using Driver.WebSite.Models;

namespace Driver.WebSite.DAL.Items
{
    public class ItemsRepository : IItemsRepository
    {
        private readonly ApplicationDbContext _context;

        public ItemsRepository(ApplicationDbContext context, HomePageItemsQuery homePageItemsQuery, 
            TopItemsQuery topItemsQuery)
        {
            _context = context;
            HomePageItemsQuery = homePageItemsQuery;
            TopItemsQuery = topItemsQuery;

            context.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
        }

        public HomePageItemsQuery HomePageItemsQuery { get; }
        public TopItemsQuery TopItemsQuery { get; }

        public async Task<Item> GetItem(int itemId, string userId)
        {
            var singleItemQuery = new SingleItemQuery(_context, itemId);
            return (await singleItemQuery.GetItems(userId, 0, 1)).First();
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
