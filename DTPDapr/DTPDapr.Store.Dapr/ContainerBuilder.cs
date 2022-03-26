using Microsoft.Extensions.DependencyInjection;
using System;

namespace DTPDapr.Store.Dapr
{
    public static class DTPDaprContainerBuilder
    {
        public static void AddDTPDaprStore(this IServiceCollection services)
        {
            services.AddSingleton<IStoreProvider, StoreProviderDaprImpl>();
        }
    }
}
