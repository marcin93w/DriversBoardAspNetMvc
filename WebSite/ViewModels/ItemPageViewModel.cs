using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driver.WebSite.ViewModels
{
    public class ItemPageViewModel
    {
        public ItemPageViewModel(ItemPanelViewModel itemPanel)
        {
            itemPanel.GenerateLinksToItemPage = false;
            ItemPanel = itemPanel;
        }

        public ItemPanelViewModel ItemPanel { get; }
    }
}
