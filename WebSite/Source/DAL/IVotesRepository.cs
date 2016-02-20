using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Driver.WebSite.Models;

namespace Driver.WebSite.DAL
{
    public interface IVotesRepository<T> where T:IVotable
    {
        Task<int> AddVote(Vote<T> vote);
        Task<int> RemoveVote(Vote<T> vote);
        Task<int> UpdateVote(Vote<T> vote);
        Task<int> GetCurrentVoteCount(int votableId);
        Task<Vote<T>> GetVote(int votableId, string userId);
    }
}
