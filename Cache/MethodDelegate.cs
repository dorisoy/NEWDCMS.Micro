using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MultilevelCache
{
    internal class MethodDelegate<Tobj, Tout, TaskTOut> : IMethodDelegate where Tout : class where TaskTOut : class
    {
        static AutoResetEvent autoCacheResetEvent = new AutoResetEvent(false);
        static SemaphoreSlim autoSemSlim = new SemaphoreSlim(1);
        private IL1CacheServiceFactory L1;
        private IL2CacheServiceFactory L2;
        private Tobj Service;
        private readonly MethodInfo Method;
        private readonly bool IsTaskMethod = false;
        private readonly Func<Tobj, object[], Tout> MethodFunCall;
        private string CacheKey { get; set; }
        private SystemCachedAttribute CachedAttr { get; set; }
        public MethodDelegate(MethodInfo method)
        {
            Method = method;
            IsTaskMethod = typeof(Tout) != typeof(TaskTOut);
            MethodFunCall = DelegateBuilder.CreateMethodDelegate<Tobj, Tout>(method);
            //获取方法缓存配置
            CachedAttr = Common.GetCachedAttrDir(method);
        }
        TaskTOut TryGetCache(MethodInfo methodInfo, object[] args)
        {
            //如果没有配置或者配置ttl设置为0
            if (CachedAttr == null || CachedAttr.ExpireSecond == 0)
                return default;
            else
            {
                //获取缓存key
                CacheKey = CachedAttr.CachedType == SystemCachedType.MethodAndParams ? Common.GetCachedKey(methodInfo, args) : Common.GetCachedKey(methodInfo);
                //从L1同步读取缓存
                var cacheResult = L1.Get<TaskTOut>(CacheKey);
                if (cacheResult == null)
                {
                    //创建一个同步对象用于接收L2线程回调的数据
                    var realResult = new TaskCompletionSource<TaskTOut>();
                    //创建一个任务并设置取消token
                    var cancelSource = new CancellationTokenSource();
                    var task = Task.Run(() => L2.GetAsync<TaskTOut>(CacheKey), cancelSource.Token);
                    //启动任务
                    task.ContinueWith(t =>
                    {
                        if (!cancelSource.Token.IsCancellationRequested)
                        {
                            if (t.Exception == null)
                                //如果任务顺利回调，则写缓存并阻止中断器
                                realResult.SetResult(t.Result);
                            autoCacheResetEvent.Set();
                        }
                    });
                    //中断器开始中断当前线程并监听阻止信号
                    if (CachedAttr.TimeOutMillisecond > 0)
                        autoCacheResetEvent.WaitOne(CachedAttr.TimeOutMillisecond);
                    else
                        autoCacheResetEvent.WaitOne();
                    cacheResult = realResult.Task.Result;
                    if (cacheResult == null)
                        return default;
                    else
                    {
                        //覆写L1
                        L1.Set(CacheKey, cacheResult, CachedAttr.ExpireSecond);
                        return cacheResult;
                    }
                }
                else
                    return cacheResult;
            }
        }
        async Task SetCache(Tout cacheResult)
        {
            if (CachedAttr != null && CachedAttr.ExpireSecond > 0)
            {
                if (IsTaskMethod)
                {
                    var _out = await (cacheResult as Task<TaskTOut>); 
                    L1.Set(CacheKey, _out, CachedAttr.ExpireSecond);
                    await L2.SetAsync(CacheKey, _out, CachedAttr.ExpireSecond);
                }
                else
                {
                    L1.Set(CacheKey, cacheResult, CachedAttr.ExpireSecond);
                    await L2.SetAsync(CacheKey, cacheResult, CachedAttr.ExpireSecond);
                }
            }
        }
        public object Excute(object[] args)
        {
            var servicePorivder = Common.GetServiceProvider();
            //注入需要的构造函数和需要调用的服务类型实例
            Service = servicePorivder.GetService<Tobj>();
            if (CachedAttr == null)
                return MethodFunCall(Service, args);
            L1 = servicePorivder.GetService<IL1CacheServiceFactory>();
            L2 = servicePorivder.GetService<IL2CacheServiceFactory>();
            //调用缓存
            var realResult = new TaskCompletionSource<Tout>();
            if (CachedAttr.SyncVisit)
            {
                Task.Run(() =>
                {
                    autoSemSlim.Wait();
                    var cache = TryGetCache(Method, args);
                    return cache;
                }).ContinueWith(async task =>
                {
                    if (task.Exception == null)
                    {
                        var cacheResult = await (task as Task<TaskTOut>);
                        //如果任务顺利回调，则写缓存并停止等待
                        if (cacheResult == null)
                        {
                            var serviceResult = await Task.Run(() => MethodFunCall(Service, args));
                            if (serviceResult != null)
                                await SetCache(serviceResult);
                            realResult.SetResult(serviceResult);
                        }
                        else
                        {
                            realResult.SetResult(IsTaskMethod ? task as Tout : task.Result as Tout);
                        }
                    }
                    autoSemSlim.Release();
                });
                return realResult.Task.Result;
            }
            else
            {
                var cacheResult = TryGetCache(Method, args);
                if (cacheResult != null)
                    return IsTaskMethod ? Task.FromResult(cacheResult) : cacheResult;
                else
                {
                    var task = MethodFunCall(Service, args);
                    _ = SetCache(task);
                    return task;
                }
            }
        }
    }
}
