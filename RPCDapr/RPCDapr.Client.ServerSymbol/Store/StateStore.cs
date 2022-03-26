using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPCDapr.Client.ServerSymbol.Store
{
    /// <summary>
    /// Dapr状态存储
    /// </summary>
    public abstract class StateStore
    {
        public abstract string Key { get; set; }
        public abstract object Data { get; set; }

        public int TtlInSeconds { get; set; }
    }
}
