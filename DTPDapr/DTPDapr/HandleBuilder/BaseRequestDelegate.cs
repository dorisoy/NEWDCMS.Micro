using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTPDapr.HandleBuilder
{
    public abstract class BaseRequestDelegate
    {
        public string Topic { get; set; }
        public HandleType HandleType { get; set; }
        public abstract Task Excute(DTPDaprData jsonData, IServiceProvider lifetimeScope);
    }
}
