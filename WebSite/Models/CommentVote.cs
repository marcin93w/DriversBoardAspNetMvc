using System.ComponentModel.DataAnnotations.Schema;

namespace Driver.WebSite.Models
{
    public class CommentVote : Vote
    {
        [Index("IX_Rate", 1, IsUnique = true)]
        public Comment Comment{ get; set; }
    }
}