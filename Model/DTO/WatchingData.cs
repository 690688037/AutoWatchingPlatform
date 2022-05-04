using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace AutoWatchingPlatform.Model
{
    public class WatchingData:PropertyChangedBase
    {
        public WatchingData(long id, string name, string code, BoardType boardType, string amount)
        {
            this.id = id;
            this.name = name;
            this.code = code;
            this.boardType = boardType;
            this.amount = string.IsNullOrEmpty(amount) ? "10000" : amount;//默认1w
        }

        public WatchingData Clone()
        {
            return (WatchingData)this.MemberwiseClone();
        }

        public enum BoardType
        {
            sky,
            land
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

        private BoardType _boardType;
        public BoardType boardType
        {
            get { return _boardType; }
            set
            {
                _boardType = value;
                NotifyOfPropertyChange("boardType");
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
