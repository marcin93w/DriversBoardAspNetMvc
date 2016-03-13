using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Driver.WebSite.Models;

namespace Driver.WebSite.DAL.Votes
{
    public class ItemVotesRepository : IVotesRepository<Item>
    {
        private readonly ApplicationDbContext _context;

        public ItemVotesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        private ItemVote GetItemVoteFromBaseClass(Vote<Item> vote)
        {
            return vote as ItemVote ?? AutoMapper.Mapper.Map<ItemVote>(vote);
        }

        public async Task<int> GetCurrentVoteCount(int itemId)
        {
            var query = from item in _context.Items
                        where item.Id == itemId
                        select item.UpVotesCount - item.DownVotesCount;

            return await query.FirstAsync();
        }

        public async Task<Vote<Item>> GetVote(int itemId, string userId)
        {
            var user = new ApplicationUser {Id = userId};

            return await (from row in _context.Set<ItemVote>()
                          where row.Votable.Id == itemId && row.User.Id == user.Id
                          select row).FirstAsync();
        }

        public async Task<int> AddVote(Vote<Item> vote)
        {
            var itemVote = GetItemVoteFromBaseClass(vote);
            vote.Votable.Votes.Add(itemVote);
            _context.Items.Attach(itemVote.Votable);
            _context.Users.Attach(itemVote.User);
            _context.Set<ItemVote>().Add(itemVote);

            return await _context.SaveChangesAsync();
        }

        public async Task<int> RemoveVote(Vote<Item> vote)
        {
            var itemVote = GetItemVoteFromBaseClass(vote);
            _context.Set<ItemVote>().Remove(itemVote);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateVote(Vote<Item> vote)
        {
            var itemVote = GetItemVoteFromBaseClass(vote);
            return await _context.SaveChangesAsync();
        }
    }
}
