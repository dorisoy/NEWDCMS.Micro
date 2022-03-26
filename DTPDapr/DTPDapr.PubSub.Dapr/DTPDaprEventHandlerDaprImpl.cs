using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using RPCDapr.Client.ServerSymbol.Events;
using RPCDapr.Common.Implements;
using RPCDapr.Common.Interface;
using DTPDapr.HandleBuilder;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DTPDapr.PubSub.Dapr
{
    public class DTPDaprEventHandlerDaprImpl : IDTPDaprEventHandler
    {
        public void ConsumerReceivedHandle(IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.Map($"/DTPDaprSubscribe/{ConfigurationManager.GetConfig().ServiceName}", builder => builder.Run(async ctx => await DTPDaprSubscribeHandle(ctx)));
        }

        async Task DTPDaprSubscribeHandle(HttpContext context)
        {
            HttpContextExtension.ContextWapper.Value = new RPCDaprHttpContextWapper($"/DTPDaprSubscribe/{ConfigurationManager.GetConfig().ServiceName}", context.RequestServices.GetService<ILifetimeScope>(), context);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 200;
            var storeProvider = context.RequestServices.GetService<IStoreProvider>();
            var serialize = context.RequestServices.GetService<ISerialize>();
            RPCDaprIocContainer.BuilderIocContainer(context.RequestServices.GetService<ILifetimeScope>());
            using (var stream = new MemoryStream())
            {
                await context.Request.Body.CopyToAsync(stream);
                DTPDaprData data = default;
                try
                {
                    data = serialize.DeserializesJson<TempDataByEventHandleInput<DTPDaprData>>(Encoding.UTF8.GetString(stream.ToArray())).GetData();
                    var oldData = await storeProvider.GetKey(data.StoreKey);
                    if (oldData == null || oldData.StoreState == DTPDaprDataState.Error)
                    {
                        data.SetState(DTPDaprDataState.Processing);
                        await storeProvider.SetDataByKey(data.StoreKey, data, DateTime.Now.AddDays(1));
                        await HandleProxyFactory.GetDelegate().FirstOrDefault(x => x.Topic == data.Topic).Excute(data, context.RequestServices);
                        data.SetState(DTPDaprDataState.Done);
                        await storeProvider.SetDataByKey(data.StoreKey, data, DateTime.Now.AddDays(1));
                    }
                }
                catch (Exception e)
                {
                    if (data != default)
                    {
                        data.SetState(DTPDaprDataState.Error);
                        await storeProvider.SetDataByKey(data.StoreKey, data, DateTime.Now.AddDays(1));
                    }
                }
                finally
                {
                    await context.Response.WriteAsync(JsonSerializer.Serialize(DefaultEventHandlerResponse.Default()));
                }
            }
        }
    }
}