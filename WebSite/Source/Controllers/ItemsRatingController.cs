using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Driver.WebSite.DAL;
using Driver.WebSite.Models;

namespace Driver.WebSite.Controllers
{
    public class ItemsRatingController : RatingController
    {
        //public ItemsRatingController(ApplicationDbContext context) : base(context)
        //{
        //}

        //private async Task<int?> GetCurrentRateCount(int itemId)
        //{
        //    var query = from item in Context.Items
        //                where item.Id == itemId
        //                select item.UpVotesCount - item.DownVotesCount;

        //    return await query.FirstAsync();
        //}

        //protected override async Task<int?> Rate(int id, bool positive)
        //{
        //    var user = await this.GetCurrentUserAsync(Context);

        //    var ratedItem = new Item { Id = id };
        //    ratedItem.Votes.Add(new ItemVote
        //    {
        //        Item = ratedItem,
        //        Positive = positive,
        //        User = user
        //    });

        //    Context.Items.Attach(ratedItem);
        //    Context.Set<ItemVote>().Add(ratedItem.Votes.First());

        //    var savedRates = await Context.SaveChangesAsync();

        //    if (savedRates > 0)
        //    {
        //        return await GetCurrentRateCount(id);
        //    }

        //    return null;
        //}

        //private async Task<ItemVote> GetItemRate(int itemId)
        //{
        //    var user = await this.GetCurrentUserAsync(Context);

        //    return await (from row in Context.Set<ItemVote>()
        //                  where row.Item.Id == itemId && row.User.Id == user.Id
        //                  select row).FirstAsync();
        //}

        //protected override async Task<int?> ClearRating(int id, bool positive)
        //{
        //    var itemRate = await GetItemRate(id);
        //    if (itemRate.Positive != positive)
        //        return null;

        //    Context.Set<ItemVote>().Remove(itemRate);

        //    var savedRates = await Context.SaveChangesAsync();

        //    if (savedRates > 0)
        //    {
        //        return await GetCurrentRateCount(id);
        //    }

        //    return null;
        //}

        //protected override async Task<int?> ChangeRating(int id, bool positive)
        //{
        //    var itemRate = await GetItemRate(id);
        //    itemRate.Positive = positive;

        //    var savedRates = await Context.SaveChangesAsync();

        //    if (savedRates > 0)
        //    {
        //        return await GetCurrentRateCount(id);
        //    }

        //    return null;
        //}
    }
}
