using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driver.WebSite.Models
{
    public class DriverOccurrence : IVotable
    {
        [Key]
        public int Id { set; get; }
        public Item Item { set; get; }
        public Driver Driver { set; get; }
        public string Description { set; get; }
        public int StartSecond { set; get; }
        public int EndSecond { set; get; }

        public int UpVotesCount { get; set; }
        public int DownVotesCount { get; set; }

        public ICollection<DriverOccurrenceVote> Votes { set; get; } = new HashSet<DriverOccurrenceVote>();
    }
}
