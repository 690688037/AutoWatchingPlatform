using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoWatchingPlatform.Model
{
    public class SharesApi
    {
        public const string getSharesDataByCode = "http://qt.gtimg.cn/q={0}";
        public const string getAllShares = "http://87.push2.eastmoney.com/api/qt/clist/get?pn=1&pz=10000&po=1&np=1&fltt=2&invt=2&fid=f3&fs=m:0+t:6,m:0+t:80,m:1+t:2,m:1+t:23&fields=f12,f14%27";

        //public static string urlHeader = "http://localhost:8080";
        public const string urlShares = "/sharesdata/shares";
        public const string urlWatching = "/sharesdata/watching";
        public const string urlPrice = "/sharesdata/price";
        public const string urlDatabaseConfiguration = "/sharesdata/databaseconfiguration";
    }
}
