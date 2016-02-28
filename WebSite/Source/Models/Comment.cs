using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Driver.WebSite.Models
{
    public class Comment : IVotable
    {
        [Key]
        public int Id { set; get; }
        public ApplicationUser User { set; get; }
        public Item Item { set; get; }
        public string Text { set; get; }
        public DateTime DateTime { set; get; }
        public int UpVotesCount { get; set; }
        public int DownVotesCount { get; set; }
        public ICollection<CommentVote> CommentVotes { set; get; } = new HashSet<CommentVote>();
    }
}