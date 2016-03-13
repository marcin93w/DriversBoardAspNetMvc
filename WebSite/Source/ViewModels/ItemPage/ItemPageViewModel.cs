using System;
using System.Collections.Generic;
using System.Linq;
using Driver.WebSite.ViewModels.ItemPanel;
using Driver.WebSite.ViewModels.Sidebar;

namespace Driver.WebSite.ViewModels.ItemPage
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

        public SidebarViewModel Sidebar { set; get; }
    }
}
