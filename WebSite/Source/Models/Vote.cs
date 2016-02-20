using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Driver.WebSite.Models
{
    public class ItemVote : Vote<Item> { }
    public class CommentVote : Vote<Comment> { }

    public class Vote<T> where T : IVotable
    {
        [Key]
        public int Id { set; get; }
        [Index("IX_Rate", 1, IsUnique = true)]
        public T Votable { get; set; }
        [Index("IX_Rate", 2, IsUnique = true)]
        public ApplicationUser User { get; set; }
        public bool Positive { get; set; }
    }
}
