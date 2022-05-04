using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace AutoWatchingPlatform.Model
{
    public class SharesData:PropertyChangedBase
    {
        public SharesData(long id, string name, string code, string price, string range, string buyDetail, string sellDetail, string skyPrice, string landPrice)
        {
            this.id = id;
            this.name = name;
            this.code = code;
            this.price = price;
            this.range = range;
            this.buyDetail = buyDetail;
            this.sellDetail = sellDetail;
            this.skyPrice = skyPrice;
            this.landPrice = landPrice;
        }
        
        private long _id;
        public long id
        {
            get { return _id; }
            set
            {
                _id = value;
                NotifyOfPropertyChange("id");
            }
        }

        private string _name;
        public string name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyOfPropertyChange("name");
            }
        }

        private string _code;
        public string code
        {
            get { return _code; }
            set
            {
                _code = value;
                NotifyOfPropertyChange("code");
            }
        }


        private string _price;
        public string price
        {
            get { return _price; }
            set
            {
                _price = value;
                NotifyOfPropertyChange("price");
            }
        }

        private string _range;
        public string range
        {
            get { return _range; }
            set
            {
                _range = value;
                NotifyOfPropertyChange("range");
            }
        }

        private string _buyDetail;
        public string buyDetail
        {
            get { return _buyDetail; }
            set
            {
                _buyDetail = value;
                NotifyOfPropertyChange("buyDetail");
            }
        }

        private string _sellDetail;
        public string sellDetail
        {
            get { return _sellDetail; }
            set
            {
                _sellDetail = value;
                NotifyOfPropertyChange("sellDetail");
            }
        }

        private string _skyPrice;
        public string skyPrice
        {
            get { return _skyPrice; }
            set
            {
                _skyPrice = value;
                NotifyOfPropertyChange("skyPrice");
            }
        }

        private string _landPrice;
        public string landPrice
        {
            get { return _landPrice; }
            set
            {
                _landPrice = value;
                NotifyOfPropertyChange("landPrice");
            }
        }
    }
}
