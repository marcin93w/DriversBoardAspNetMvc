using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Driver.WebSite.Models;

namespace Driver.WebSite.DAL
{
    public interface IItemsRepository
    {
        Task<IEnumerable<Item>> GetHomePageItems(string userId, int startingFrom, int limit);
        Task<bool> AreThereOlderHomePageItems(DateTime time);

        Task<Item> GetItem(int itemId, string userId);

        Task<int> AddItem(Item item);
        Task<int> AddComment(Comment comment);
    }
}
