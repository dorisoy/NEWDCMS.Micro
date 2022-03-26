using RPCDapr.Client.ServerSymbol.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RPCDapr.Client.ServerProxyFactory.Interface
{

    /// <summary>
    /// 用于发起对远程rpc和actor的调用
    /// </summary>
    public interface IServiceProxyFactory
    {
        T CreateProxy<T>() where T : class;
        T CreateActorProxy<T>() where T : class;
    }
}
