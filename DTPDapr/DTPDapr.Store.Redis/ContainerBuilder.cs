using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTPDapr;
using DTPDapr.HandleBuilder;

namespace DTPDapr.Store.Redis
{
    public static class DTPDaprContainerBuilder
    {
        public static void AddDTPDaprStore(this IServiceCollection services)
        {
            services.AddSingleton<IStoreProvider, StoreProviderRedisImpl>();
        }
    }
}