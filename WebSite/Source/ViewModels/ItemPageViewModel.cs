using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Driver.WebSite.Models;

namespace Driver.WebSite.ViewModels
{
    public class ItemPageViewModel
    {
        public ItemPageViewModel(ItemPanelViewModel itemPanel, IEnumerable<Comment> comments, int? addedCommentId = null)
        {
            itemPanel.GenerateLinksToItemPage = false;
            ItemPanel = itemPanel;
            Comments = comments ?? new Comment[0];
            AddedComment = addedCommentId.HasValue ? Comments.FirstOrDefault(c => c.Id == addedCommentId) : null;
        }

        public ItemPageViewModel(ItemPanelViewModel itemPanel, IEnumerable<Comment> comments,
            Exception commentAddingException) :this(itemPanel, comments)
        {
            CommentAddingError = commentAddingException != null;
        }

        public ItemPanelViewModel ItemPanel { get; }
        public IEnumerable<Comment> Comments { get; } 

        public Comment AddedComment { get; }

        public bool CommentAddingError { get; }
    }
}
