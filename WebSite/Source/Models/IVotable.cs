using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driver.WebSite.Models
{
    public interface IVotable
    {
        int Id { set; get; }
        int UpVotesCount { set; get; }
        int DownVotesCount { set; get; }
    }

    public static class VotableExtensions
    {
        public static int GetVotesCount(this IVotable votable)
        {
            return votable.UpVotesCount - votable.DownVotesCount;
        }
    }
}
