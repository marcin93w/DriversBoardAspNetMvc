using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Driver.WebSite.Models;
using Humanizer;

namespace Driver.WebSite.ViewModels
{
    public class CommentViewModel
    {
        public int Id { set; get; }
        public string AuthorLogin { set; get; }
        public Item Item { set; get; }
        public string Text { set; get; }
        public DateTime DateTime { set; get; }
        public string ReadableDateTime => DateTime.Humanize();
        public int VotesCount { set; get; }
        public int UserVote { set; get; }
    }
}
