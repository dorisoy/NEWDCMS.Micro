using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using RPCDapr.Common.Interface;
using RPCDapr.Server.Kestrel.Implements;
using RPCDapr.Server.Kestrel.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPCDapr.Mesh.Dapr
{
    public class RPCDaprActorStartup : RPCDaprStartup
    {
        public static new void ConfigureServices(IServiceCollection service)
        {
            RPCDaprStartup.ConfigureServices(service);
            ActorServiceFactory.RegisterActorService(service);
        }
        public static new void Configure(IApplicationBuilder appBuilder, IServiceProvider serviceProvider)
        {
            RPCDaprStartup.Configure(appBuilder, serviceProvider);
            ActorServiceFactory.UseActorService(appBuilder, serviceProvider.GetService<ILifetimeScope>());
        }
    }
}
