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
        public ItemPageViewModel(ItemPanelViewModel itemPanel, IEnumerable<CommentViewModel> comments, int? addedCommentId = null)
        {
            itemPanel.GenerateLinksToItemPage = false;
            ItemPanel = itemPanel;
            Comments = comments ?? new CommentViewModel[0];
            AddedComment = addedCommentId.HasValue ? Comments.FirstOrDefault(c => c.Id == addedCommentId) : null;
        }

        public ItemPageViewModel(ItemPanelViewModel itemPanel, IEnumerable<CommentViewModel> comments,
            Exception commentAddingException) :this(itemPanel, comments)
        {
            CommentAddingError = commentAddingException != null;
        }

        public ItemPanelViewModel ItemPanel { get; }
        public IEnumerable<CommentViewModel> Comments { get; } 

        public CommentViewModel AddedComment { get; }

        public bool CommentAddingError { get; }
    }
}
