using System.Threading.Tasks;
using Driver.WebSite.Models;

namespace Driver.WebSite.DAL.Items
{
    public interface IItemsRepository
    {
        HomePageItemsQuery HomePageItemsQuery { get; }
        TopItemsQuery TopItemsQuery { get; }
        WaitingItemsQuery WaitingItemsQuery { get; }

        DriverItemsQuery GetDriverItemsQuery(int driverId);
        DriverItemsQuery GetDriverItemsQuery(string plate);

        Task<Item> GetItem(int itemId, string userId);

        Task<int> AddItem(Item item);
        Task<int> AddComment(Comment comment);
    }
}
