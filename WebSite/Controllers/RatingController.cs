using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Driver.WebSite.Models;

namespace Driver.WebSite.Controllers
{
    [Authorize]
    public class RatingController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public RatingController(ApplicationDbContext context)
        {
            _context = context;
        }


        private async Task<int?> GetCurrentRateCount(int itemId)
        {
            var query = from item in _context.Items
                        where item.Id == itemId
                        select item.UpVotesCount - item.DownVotesCount;

            return await query.FirstAsync();
        }

        private async Task<int?> Rate(int itemId, bool positive)
        {
            var user = await this.GetCurrentUserAsync(_context);
            
            var ratedItem = new Item { Id = itemId };
            ratedItem.Votes.Add(new ItemVote
            {
                Item = ratedItem,
                Positive = positive,
                User = user
            });

            _context.Items.Attach(ratedItem);
            _context.Set<ItemVote>().Add(ratedItem.Votes.First());

            var savedRates = await _context.SaveChangesAsync();

            if (savedRates > 0)
            {
                return await GetCurrentRateCount(itemId);
            }

            return null;
        }

        [HttpGet]
        public async Task<int?> RateUp(int id)
        {
            return await Rate(id, true);
        }

        [HttpGet]
        public async Task<int?> RateDown(int id)
        {
            return await Rate(id, false);
        }

        private async Task<ItemVote> GetItemRate(int itemId)
        {
            var user = await this.GetCurrentUserAsync(_context);

            return await (from row in _context.Set<ItemVote>()
                where row.Item.Id == itemId && row.User.Id == user.Id
                select row).FirstAsync();
        }

        private async Task<int?> ClearRating(int id, bool positive)
        {
            var itemRate = await GetItemRate(id);
            if (itemRate.Positive != positive)
                return null;

            _context.Set<ItemVote>().Remove(itemRate);

            var savedRates = await _context.SaveChangesAsync();

            if (savedRates > 0)
            {
                return await GetCurrentRateCount(id);
            }

            return null;
        }

        [HttpGet]
        public async Task<int?> ClearRatingUp(int id)
        {
            return await ClearRating(id, true);
        }

        [HttpGet]
        public async Task<int?> ClearRatingDown(int id)
        {
            return await ClearRating(id, false);
        }

        private async Task<int?> ChangeRating(int id, bool positive)
        {
            var itemRate = await GetItemRate(id);
            itemRate.Positive = positive;

            var savedRates = await _context.SaveChangesAsync();

            if (savedRates > 0)
            {
                return await GetCurrentRateCount(id);
            }

            return null;
        }

        [HttpGet]
        public async Task<int?> ChangeRateToUp(int id)
        {
            return await ChangeRating(id, true);
        }

        [HttpGet]
        public async Task<int?> ChangeRateToDown(int id)
        {
            return await ChangeRating(id, false);
        }
    }
}
