using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Driver.WebSite.Models;

namespace Driver.WebSite.DAL
{
    public class CommentVotesRepository : IVotesRepository<Comment>
    {
        private readonly ApplicationDbContext _context;

        public CommentVotesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        private CommentVote GetCommentVoteFromBaseClass(Vote<Comment> vote)
        {
            return vote as CommentVote ?? AutoMapper.Mapper.Map<CommentVote>(vote);
        }

        public async Task<int> GetCurrentVoteCount(int itemId)
        {
            var query = from item in _context.Comments
                        where item.Id == itemId
                        select item.UpVotesCount - item.DownVotesCount;

            return await query.FirstAsync();
        }

        public async Task<Vote<Comment>> GetVote(int itemId, string userId)
        {
            var user = new ApplicationUser { Id = userId };

            return await (from row in _context.Set<CommentVote>()
                          where row.Votable.Id == itemId && row.User.Id == user.Id
                          select row).FirstAsync();
        }

        public async Task<int> AddVote(Vote<Comment> vote)
        {
            var commentVote = GetCommentVoteFromBaseClass(vote);
            //vote.Votable.CommentVotes.Add(commentVote);
            _context.Comments.Attach(commentVote.Votable);
            _context.Users.Attach(commentVote.User);
            _context.Set<CommentVote>().Add(commentVote);

            return await _context.SaveChangesAsync();
        }

        public async Task<int> RemoveVote(Vote<Comment> vote)
        {
            var commentVote = GetCommentVoteFromBaseClass(vote);
            _context.Set<CommentVote>().Remove(commentVote);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateVote(Vote<Comment> vote)
        {
            var commentVote = GetCommentVoteFromBaseClass(vote);
            return await _context.SaveChangesAsync();
        }
    }
}
