using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Driver.WebSite.Models;

namespace Driver.WebSite.ViewModels
{
    public class ItemPageViewModel
    {
        public ItemPageViewModel(ItemPanelViewModel itemPanel, IEnumerable<Comment> comments)
        {
            itemPanel.GenerateLinksToItemPage = false;
            ItemPanel = itemPanel;
            Comments = comments;
        }

        public ItemPanelViewModel ItemPanel { get; }
        public IEnumerable<Comment> Comments { get; } 
    }
}
