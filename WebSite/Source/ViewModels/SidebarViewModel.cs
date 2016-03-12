using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driver.WebSite.ViewModels
{
    public class SidebarViewModel
    {
        public DriversRankingViewModel DriversRanking { set; get; }
    }

    public class DriversRankingViewModel
    {
        public IEnumerable<DriverRankingViewModel> Drivers { set; get; } 
    }

    public class DriverRankingViewModel
    {
        public int Id { set; get; }
        public string Plate { set; get; }
        public string Description { set; get; }
        public int DownVotesCount { set; get; } 
    }
}
