using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Driver.WebSite.ViewModels.ItemPanel;
using Driver.WebSite.ViewModels.Sidebar;

namespace Driver.WebSite.ViewModels
{
    public class ItemsPageViewModel
    {
        public IEnumerable<ItemPanelViewModel> Items { get; set; }
        public bool DisplayAddedInfo { get; set; }
        public PaginationViewModel Pagination { get; set; }
        public SidebarViewModel Sidebar { set; get; }
    }
}
