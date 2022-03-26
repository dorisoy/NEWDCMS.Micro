using Autofac;
using RPCDapr.PrRPCDaprerator.Implements;
using RPCDapr.PrRPCDaprerator.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using RPCDapr.Common.Implements;

namespace RPCDapr.PrRPCDaprerator
{
    public class Module : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly).Where(x => !ReflectionHelper.IsSystemType(x))
                .AsImplementedInterfaces().Where(x =>!(x is IRemoteMessageSenderDelegate))
                .InstancePerLifetimeScope();
            RemotePrRPCDaprerator.CreateRemoteProxyAndRegisterInIocContainer(builder);
        }
    }
}
