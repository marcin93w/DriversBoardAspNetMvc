using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driver.WebSite.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<ItemPanelViewModel> Items { get; set; }
        public bool DisplayAddedInfo { get; set; }
        public SidebarViewModel Sidebar { set; get; }
    }
}
