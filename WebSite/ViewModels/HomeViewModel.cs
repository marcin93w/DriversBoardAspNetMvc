using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driver.WebSite.ViewModels
{
    public class HomeViewModel
    {
        public HomeViewModel(IEnumerable<ItemPanelViewModel> items)
        {
            Items = items;
        }

        public IEnumerable<ItemPanelViewModel> Items { get; }
        public bool DisplayAddedInfo { get; set; }
    }
}
