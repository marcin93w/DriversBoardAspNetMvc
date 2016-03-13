using System;
using Driver.WebSite.Models;
using Humanizer;

namespace Driver.WebSite.ViewModels.ItemPage
{
    public class CommentViewModel
    {
        public int Id { set; get; }
        public string AuthorLogin { set; get; }
        public Item Item { set; get; }
        public string Text { set; get; }
        public DateTime DateTime { set; get; }
        public string ReadableDateTime => DateTime.Humanize(false);
        public int VotesCount { set; get; }
        public int UserVote { set; get; }
    }
}
