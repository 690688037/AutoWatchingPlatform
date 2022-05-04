using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoWatchingPlatform.Model
{
    public class PriceData : PropertyChangedBase
    {
        public PriceData(long id, string name, string code, string price, string range, ComparisonOperator comparisonOperator)
        {
            this.id = id;
            this.name = name;
            this.code = code;
            this.price = price;
            //this.isArrived = false;
            this.range = range;
            this.comparisonOperator = comparisonOperator;
        }

        public enum ComparisonOperator
        {
            bigger,
            smaller
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

        //private bool _isArrived;
        //public bool isArrived
        //{
        //    get { return _isArrived; }
        //    set
        //    {
        //        _isArrived = value;
        //        NotifyOfPropertyChange("isArrived");
        //    }
        //}

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

        private ComparisonOperator _comparisonOperator;
        public ComparisonOperator comparisonOperator
        {
            get { return _comparisonOperator; }
            set
            {
                _comparisonOperator = value;
                NotifyOfPropertyChange("comparisonOperator");
            }
        }
    }
}
