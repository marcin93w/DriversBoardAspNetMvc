using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driver.WebSite.Models
{
    public interface IVotable
    {
        int UpVotesCount { set; get; }
        int DownVotesCount { set; get; }
    }
}
