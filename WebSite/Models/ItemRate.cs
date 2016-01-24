using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace Driver.WebSite.Models
{
    public class ItemRate
    {
        [Key]
        public int Id { set; get; }
        [Index("IX_Rate", 1, IsUnique = true)]
        public Item Item { get; set; }
        [Index("IX_Rate", 2, IsUnique = true)]
        public ApplicationUser User { get; set; }
        public bool Positive { get; set; }
    }
}