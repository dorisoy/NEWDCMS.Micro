using RPCDapr.Client.ServerSymbol.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTPDapr.Store.Dapr
{
    public class DaprStateStoreDTPDaprModel<T> : StateStore
    {
        public DaprStateStoreDTPDaprModel(string key, T data, int expireTimeSecond)
        {
            Key = $"DaprStateStoreDTPDaprModel_{key}";
            this.Data = data;
            TtlInSeconds = expireTimeSecond;
        }
        public DaprStateStoreDTPDaprModel(string key)
        {
            Key = $"DaprStateStoreDTPDaprModel_{key}";
        }
        public override string Key { get; set; }
        public override object Data { get; set; }
    }
}
