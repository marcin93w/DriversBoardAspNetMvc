using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Driver.WebSite.Models;

namespace Driver.WebSite.DAL.Votes
{
    public class DriverOccurenceVotesRepository : IVotesRepository<DriverOccurrence>
    {
        private readonly ApplicationDbContext _context;

        public DriverOccurenceVotesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        private DriverOccurrenceVote GetDriverOccurrenceVoteFromBaseClass(Vote<DriverOccurrence> vote)
        {
            return vote as DriverOccurrenceVote ?? AutoMapper.Mapper.Map<DriverOccurrenceVote>(vote);
        }

        public async Task<int> GetCurrentVoteCount(int itemId)
        {
            var query = from item in _context.DriverOccurrences
                        where item.Id == itemId
                        select item.UpVotesCount - item.DownVotesCount;

            return await query.FirstAsync();
        }

        public async Task<Vote<DriverOccurrence>> GetVote(int itemId, string userId)
        {
            var user = new ApplicationUser { Id = userId };

            return await (from row in _context.Set<DriverOccurrenceVote>()
                          where row.Votable.Id == itemId && row.User.Id == user.Id
                          select row).FirstAsync();
        }

        public async Task<int> AddVote(Vote<DriverOccurrence> vote)
        {
            var driverOccurrenceVote = GetDriverOccurrenceVoteFromBaseClass(vote);
            _context.DriverOccurrences.Attach(driverOccurrenceVote.Votable);
            _context.Users.Attach(driverOccurrenceVote.User);
            _context.Set<DriverOccurrenceVote>().Add(driverOccurrenceVote);

            return await _context.SaveChangesAsync();
        }

        public async Task<int> RemoveVote(Vote<DriverOccurrence> vote)
        {
            var driverOccurrenceVote = GetDriverOccurrenceVoteFromBaseClass(vote);
            _context.Set<DriverOccurrenceVote>().Remove(driverOccurrenceVote);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateVote(Vote<DriverOccurrence> vote)
        {
            var driverOccurrenceVote = GetDriverOccurrenceVoteFromBaseClass(vote);
            return await _context.SaveChangesAsync();
        }
    }
}
