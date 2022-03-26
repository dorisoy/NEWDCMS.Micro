using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTPDapr;
using DTPDapr.HandleBuilder;
using Microsoft.AspNetCore.Builder;

namespace DTPDapr.PubSub.Rabbitmq
{
    public static class DTPDaprContainerBuilder
    {
        public static void AddDTPDapr(this IServiceCollection services, DTPDaprConfiguration DTPDaprConfiguration)
        {   
            services.AddSingleton<IDTPDaprManager, DTPDaprManagerRabbitmqImpl>();
            services.AddSingleton<IDTPDaprEventHandler, DTPDaprEventHandlerRabbitmqImpl>();
            ConfigurationManager.SetConfig(DTPDaprConfiguration);
        }
        public static void RegisterDTPDaprHandler(this IApplicationBuilder applicationbuilder, Func<IServiceProvider, ErrorModel, Task> errorHandle)
        {
            HandleProxyFactory.RegiterAllHandle(errorHandle);
            applicationbuilder.ApplicationServices.GetService<IDTPDaprEventHandler>().ConsumerReceivedHandle(applicationbuilder);
        }
    }
}
