using Autofac;
using RPCDapr.Common.Implements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPCDapr.Common
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
