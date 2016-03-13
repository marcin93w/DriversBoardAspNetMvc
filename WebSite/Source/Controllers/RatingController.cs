using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Driver.WebSite.DAL;
using Driver.WebSite.DAL.Votes;
using Driver.WebSite.Models;

namespace Driver.WebSite.Controllers
{
    [Authorize]
    public class ItemsRatingController : RatingController<Item>
    {
        public ItemsRatingController(IVotesRepository<Item> votesRepository) 
            : base(votesRepository) { }
    }

    [Authorize]
    public class CommentsRatingController : RatingController<Comment>
    {
        public CommentsRatingController(IVotesRepository<Comment> votesRepository)
            : base(votesRepository) { }
    }

    [Authorize]
    public class DriverOccurrenceRatingController : RatingController<DriverOccurrence>
    {
        public DriverOccurrenceRatingController(IVotesRepository<DriverOccurrence> votesRepository)
            : base(votesRepository)
        { }

        public override Task<int?> ChangeRateToUp(int id)
        {
            throw new NotSupportedException("Drivers can only be rated down");
        }

        public override Task<int?> RateUp(int id)
        {
            throw new NotSupportedException("Drivers can only be rated down");
        }

        public override Task<int?> ClearRatingUp(int id)
        {
            throw new NotSupportedException("Drivers can only be rated down");
        }
    }

    [Authorize]
    public class RatingController<T> : ApiController where T : IVotable, new()
    {
        private readonly IVotesRepository<T> _votesRepository;

        public RatingController(IVotesRepository<T> votesRepository)
        {
            _votesRepository = votesRepository;
        }

        private async Task<int?> Rate(int itemId, bool positive)
        {
            var userId = this.GetCurrentUserId();
            var ratedItem = new T { Id = itemId };

            var savedRates = await _votesRepository.AddVote(new Vote<T>
            {
                Votable = ratedItem,
                Positive = positive,
                User = new ApplicationUser { Id = userId }
            });

            if (savedRates > 0)
            {
                return await _votesRepository.GetCurrentVoteCount(itemId);
            }
            return null;
        }

        [HttpGet]
        public virtual async Task<int?> RateUp(int id)
        {
            return await Rate(id, true);
        }

        [HttpGet]
        public virtual async Task<int?> RateDown(int id)
        {
            return await Rate(id, false);
        }

        private async Task<int?> ClearRating(int id, bool positive)
        {
            var itemRate = await _votesRepository.GetVote(id, this.GetCurrentUserId());
            if (itemRate.Positive != positive)
                return null;

            var savedRates = await _votesRepository.RemoveVote(itemRate);

            if (savedRates > 0)
            {
                return await _votesRepository.GetCurrentVoteCount(id);
            }

            return null;
        }

        [HttpGet]
        public virtual async Task<int?> ClearRatingUp(int id)
        {
            return await ClearRating(id, true);
        }

        [HttpGet]
        public virtual async Task<int?> ClearRatingDown(int id)
        {
            return await ClearRating(id, false);
        }

        private async Task<int?> ChangeRating(int id, bool positive)
        {
            var itemRate = await _votesRepository.GetVote(id, this.GetCurrentUserId());
            itemRate.Positive = positive;

            var savedRates = await _votesRepository.UpdateVote(itemRate);

            if (savedRates > 0)
            {
                return await _votesRepository.GetCurrentVoteCount(id);
            }

            return null;
        }

        [HttpGet]
        public virtual async Task<int?> ChangeRateToUp(int id)
        {
            return await ChangeRating(id, true);
        }

        [HttpGet]
        public virtual async Task<int?> ChangeRateToDown(int id)
        {
            return await ChangeRating(id, false);
        }
    }
}
