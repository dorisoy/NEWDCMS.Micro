using Autofac;
using RPCDapr.Common.Implements;
using RPCDapr.Server.Kestrel.Implements;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace RPCDapr.Server.Kestrel
{
    public class Module : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly).Where(x => !ReflectionHelper.IsSystemType(x))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}
