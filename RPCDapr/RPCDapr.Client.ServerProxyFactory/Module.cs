using Autofac;
using RPCDapr.ProxyGenerator.Implements;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using RPCDapr.Common.Implements;
using Autofac.Features.ResolveAnything;

namespace RPCDapr.Client.ServerProxyFactory
{
    public class Module : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource(t => t.IsAssignableTo<IEventBus>()));
            //builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource(t => t.IsAssignableTo<IServiceProxyFactory>()));
            //builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource(t => t.IsAssignableTo<IStateManager>()));

            builder.RegisterAssemblyTypes(ThisAssembly).Where(x => !ReflectionHelper.IsSystemType(x))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}
