using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace AutoWatchingPlatform.Model
{
    internal class InputData : PropertyChangedBase
    {
        public InputData(long id, string code, bool isSky, string amount)
        {
            this.id = id;
            this.code = code;
            this.isSky = isSky;
            this.amount = string.IsNullOrEmpty(amount) ? "10000" : amount;//默认1w
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

        private bool _isSky;
        public bool isSky
        {
            get { return _isSky; }
            set
            {
                _isSky = value;
                NotifyOfPropertyChange("isSky");
            }
        }

        private string _amount;
        public string amount
        {
            get { return _amount; }
            set
            {
                _amount = value;
                NotifyOfPropertyChange("amount");
            }
        }
    }
}
