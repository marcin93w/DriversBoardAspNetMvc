using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Driver.Common.Models
{
    public class ItemRate
    {
        [Key]
        public Item Item { get; set; }
        [Key]
        public ApplicationUser User { get; set; }
        public bool? Positive { get; set; }
    }
}