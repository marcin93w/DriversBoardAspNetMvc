using System.Threading.Tasks;
using Driver.WebSite.Models;

namespace Driver.WebSite.DAL.Items
{
    public interface IItemsRepository
    {
        IItemsQuery HomePageItemsQuery { get; }
        IItemsQuery TopItemsQuery { get; }
        IItemsQuery WaitingItemsQuery { get; }

        IItemsQuery GetDriverItemsQuery(int driverId);
        IItemsQuery GetDriverItemsQuery(string plate);

        Task<Item> GetItem(int itemId, string userId);

        Task<int> AddItem(Item item);
        Task<int> AddComment(Comment comment);
    }
}
