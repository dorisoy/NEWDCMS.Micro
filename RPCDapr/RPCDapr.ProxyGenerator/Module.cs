using Autofac;
using RPCDapr.ProxyGenerator.Implements;
using RPCDapr.ProxyGenerator.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using RPCDapr.Common.Implements;

namespace RPCDapr.ProxyGenerator
{
    public class Module : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly).Where(x => !ReflectionHelper.IsSystemType(x))
                .AsImplementedInterfaces().Where(x =>!(x is IRemoteMessageSenderDelegate))
                .InstancePerLifetimeScope();
            RemoteProxyGenerator.CreateRemoteProxyAndRegisterInIocContainer(builder);
        }
    }
}
