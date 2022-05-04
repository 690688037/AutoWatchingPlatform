using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoWatchingPlatform.Model
{
    public class JsonResult<T>
    {
        public bool isSuccess { get; set; }
        public T data { get; set; }
    }
}
