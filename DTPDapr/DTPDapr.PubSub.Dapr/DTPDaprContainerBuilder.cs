using Autofac;
using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc.Routing;
//using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
//using RPCDapr.Client.ServerSymbol.Events;
//using RPCDapr.Common.Implements;
//using RPCDapr.Common.Interface;
using DTPDapr.HandleBuilder;
using System;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Text.Json;
//using System.Threading;
using System.Threading.Tasks;

namespace DTPDapr.PubSub.Dapr
{
    public static class DTPDaprContainerBuilder
    {
        public static void AddDTPDapr(this IServiceCollection services, DTPDaprConfiguration DTPDaprConfiguration)
        {
            services.AddSingleton<IDTPDaprManager, DTPDaprManagerDaprImpl>();
            services.AddSingleton<IDTPDaprEventHandler, DTPDaprEventHandlerDaprImpl>();
            ConfigurationManager.SetConfig(DTPDaprConfiguration);
        }
        public static void RegisterDTPDaprHandler(this IApplicationBuilder applicationbuilder, Func<IServiceProvider, ErrorModel, Task> errorHandle)
        {
            //注册中间件用于写入DTPDapr特定的eventhandle到dapr的/dapr/subscribe路由终结点用于dapr注册订阅器
            applicationbuilder.UseMiddleware<DparSubscribeMiddleware>();
            HandleProxyFactory.RegiterAllHandle(errorHandle);
            applicationbuilder.ApplicationServices.GetService<IDTPDaprEventHandler>().ConsumerReceivedHandle(applicationbuilder);
        }
    }
}
