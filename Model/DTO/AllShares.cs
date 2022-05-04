using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoWatchingPlatform.Model
{
    public class AllShares
    {
        public AllSharesData data;
    }
    public class AllSharesData
    {
        public List<SharesTemplate> diff;
    }
    public class SharesTemplate
    {
        public string f12 { get; set; }
        public string f14 { get; set; }
    }
}
