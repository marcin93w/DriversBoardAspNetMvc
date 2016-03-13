using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Core.Metadata.Edm;

namespace Driver.WebSite.Models
{
    public class Item : IVotable
    {
        [Key]
        public int Id { set; get; }
        public ApplicationUser Author { set; get; }
        public string Title { set; get; }
        public ItemContentType ContentType { set; get; }
        public string ContentUrl { set; get; }
        public int UpVotesCount { get; set; }
        public int DownVotesCount { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { set; get; }
        public ICollection<Comment> Comments { set; get; }  
        public ICollection<ItemVote> Votes { set; get; }
        public ICollection<DriverOccurrence> DriversOccurrences { set; get; } 

        public bool DisplayOnHomePage { set; get; }

        public Item()
        {
            Votes = new HashSet<ItemVote>();
            Comments = new HashSet<Comment>();
            DriversOccurrences = new HashSet<DriverOccurrence>();
        }
    }
}
