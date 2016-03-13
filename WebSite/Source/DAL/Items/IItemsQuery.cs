using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Driver.WebSite.Models;

namespace Driver.WebSite.DAL.Items
{
    public interface IItemsQuery
    {
        Task<IEnumerable<Item>> GetItems(string userId, int startingFrom, int limit);
        Task<bool> AreThereOlderItems(DateTime time);
    }
}
