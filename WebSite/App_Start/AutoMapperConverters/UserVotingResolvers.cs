using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Driver.WebSite.Models;

namespace Driver.WebSite.AutoMapperConverters
{
    public class ItemUserVotingResolver : ValueResolver<Item, int>
    {
        protected override int ResolveCore(Item source)
        {
            return UserVotingResolverHelper.ResolveUserVote(source.Votes);
        }
    }
    public class CommentUserVotingResolver : ValueResolver<Comment, int>
    {
        protected override int ResolveCore(Comment source)
        {
            return UserVotingResolverHelper.ResolveUserVote(source.CommentVotes);
        }
    }
    public class DriverOccurrenceUserVotingResolver : ValueResolver<DriverOccurrence, int>
    {
        protected override int ResolveCore(DriverOccurrence source)
        {
            return UserVotingResolverHelper.ResolveUserVote(source.Votes);
        }
    }

    internal class UserVotingResolverHelper
    {
        public static int ResolveUserVote<T>(IEnumerable<Vote<T>> votes) where T : IVotable
        {
            bool? userVotedPositive = votes.FirstOrDefault()?.Positive;
            return userVotedPositive.HasValue ? (userVotedPositive.Value ? 1 : -1) : 0;
        }
    }
}
