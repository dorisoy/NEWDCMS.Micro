using RPCDapr.Client.ServerProxyFactory.Interface;
using RPCDapr.Client.ServerSymbol.Events;
using RPCDapr.Common.Implements;
using RPCDapr.PrRPCDaprerator.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RPCDapr.Client.ServerProxyFactory.Implements
{
    public class ServiceProxyFactory : IServiceProxyFactory
    {
        public T CreateProxy<T>() where T : class
        {
            return RPCDaprIocContainer.Resolve<T>();
        }
        public T CreateActorProxy<T>() where T : class
        {
            return RPCDaprIocContainer.ResolveNamed<T>($"{typeof(T).FullName}ActorProxy");
        }
    }
}
