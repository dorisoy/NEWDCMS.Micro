using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MultilevelCache
{
    public static class WebApplicationL2CachedExtension
    {
        public static void InjectionCached<L1Cache, L2Cache>(this IServiceCollection services) where L1Cache : class, IL1CacheServiceFactory where L2Cache : class, IL2CacheServiceFactory
        {
            //注入多级缓存实现
            services.AddSingleton<IL1CacheServiceFactory, L1Cache>();
            services.AddSingleton<IL2CacheServiceFactory, L2Cache>();
            //为所有注解方法的类型注入代理
            Common.GetSystemCachedAttributeService().ForEach(x =>
            {
                //获取包含注解的方法
                var cacheMethods = x.tImpl.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).ToList();
                cacheMethods.ForEach(x => {
                    //将方法和注解以kv的形式保存，避免使用时反射
                    if (x.GetCustomAttribute<SystemCachedAttribute>() != null)
                        Common.SetCachedAttrDir(x, x.GetCustomAttribute<SystemCachedAttribute>());
                    //为实现的缓存方法注入委托
                    DelegateBuilder.CreateDelegate(x);
                });
                //将实现注入容器
                services.AddScoped(x.tImpl);
                //为接口注入代理
                var proxy = Common.DispatchProxyCreate(x.tInterface, x.tImpl);
                services.AddScoped(x.tInterface, y => {
                    Common.SetServiceProvider(y);
                    return proxy;
                });
            });
        }
    }
}
