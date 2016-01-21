using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Driver.Common.Models
{
    public class Item
    {
        [Key]
        public int Id { set; get; }
        public ApplicationUser Author { set; get; }
        public string Title { set; get; }
        public ItemContentType ContentType { set; get; }
        public string ContentUrl { set; get; }
        public int UpScore { get; set; }
        public int DownScore { get; set; }
        public string Comment { get; set; }
        public DateTime DateAdded { set; get; }
        public List<ItemRate> Rates { set; get; }
    }
}
