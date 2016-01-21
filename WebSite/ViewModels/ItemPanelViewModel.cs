using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Driver.Common.Models;

namespace Driver.WebSite.ViewModels
{
    public class ItemPanelViewModel
    {
        public int Id { set; get; }
        public string AuthorLogin { set; get; }
        public string Title { set; get; }
        public ItemContentType ContentType { set; get; }
        public string ContentUrl { set; get; }
        public int UpScore { get; set; }
        public int DownScore { get; set; }
        public string Comment { get; set; }
        public DateTime DateAdded { set; get; }

        public bool GenerateLinksToItemPage { set; get; } = true;
    }
}
