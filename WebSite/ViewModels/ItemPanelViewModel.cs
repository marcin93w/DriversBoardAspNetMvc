using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Driver.WebSite.Models;

namespace Driver.WebSite.ViewModels
{
    public class ItemPanelViewModel
    {
        public int Id { set; get; }
        public string AuthorLogin { set; get; }
        public string Title { set; get; }
        public ItemContentType ContentType { set; get; }
        public string ContentUrl { set; get; }
        public int UpVotesCount { get; set; }
        public int DownVotesCount { get; set; }
        public int Rate => UpVotesCount - DownVotesCount;
        /// <summary>
        /// 1 - user voted up, -1 - user voted down, 0 - no vote
        /// </summary>
        public int UserVoting { get; set; } = 0;
        public string Comment { get; set; }
        public DateTime DateAdded { set; get; }

        public bool GenerateLinksToItemPage { set; get; } = true;
    }
}
