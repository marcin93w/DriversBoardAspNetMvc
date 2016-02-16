using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace Driver.WebSite.Models
{
    public class ItemVote : Vote
    {
        [Index("IX_Rate", 1, IsUnique = true)]
        public Item Item { get; set; }
    }
}