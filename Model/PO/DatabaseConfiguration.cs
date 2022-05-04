using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoWatchingPlatform.Model.PO
{
    public class DatabaseConfiguration
    {
        public DateTime date;

        public string server_address;

        public int shares_record_max;

        public int price_record_max;

        public int watching_record_max;

        public int fail_max;
    }
}
