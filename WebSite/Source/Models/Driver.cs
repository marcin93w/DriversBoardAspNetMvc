using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driver.WebSite.Models
{
    public class Driver : IVotable
    {
        [Key]
        public int Id { set; get; }
        public string Plate { set; get; }
        public string Description { set; get; }

        public int UpVotesCount { get; set; }
        public int DownVotesCount { get; set; }

        public ICollection<DriverOccurrence> Occurrences { set; get; }

        public Driver()
        {
            Occurrences = new HashSet<DriverOccurrence>();
        }
    }
}
