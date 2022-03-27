using Microsoft.AspNetCore.Http;
using Microsoft.IO;
using RPCDapr.Common.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using RPCDapr.Common;
using System.Text.Json;

namespace DTPDapr.PubSub.Dapr
{
    public class DparSubscribeMiddleware
    {
        RequestDelegate next;
        private readonly RecyclableMemoryStreamManager recyclableMemoryStreamManager;
        public DparSubscribeMiddleware(RequestDelegate next)
        {
            this.next = next;
            this.recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.Value == "/dapr/subscribe")
            {
                using (var newResponse = recyclableMemoryStreamManager.GetStream())
                {
                    var originalResponseBody = context.Response.Body;
                    List<SubscribeModel> submodellist = default;
                    try
                    {
                        context.Response.Body = newResponse;
                        await next(context);
                        newResponse.Seek(0, SeekOrigin.Begin);
                        string responseBody = new StreamReader(newResponse).ReadToEnd();
                        submodellist = context.RequestServices.GetService<ISerialize>().DeserializesJson<List<SubscribeModel>>(responseBody) ?? new List<SubscribeModel>();

                        var cfg = DaprConfig.GetCurrent();
                        var cfm = ConfigurationManager.GetConfig();

                        //SagaSubscribe
                        submodellist.Add(new SubscribeModel(cfg?.PubSubCompentName, cfm?.ServiceName, $"/DTPDaprSubscribe/{cfm?.ServiceName}"));

                        newResponse.Seek(0, SeekOrigin.Begin);
                    }
                    catch (Exception ex)
                    {
						System.Diagnostics.Debug.Print(ex.Message);
						//"[{\"pubsubname\":\"pubsub\",\"topic\":\"InitTestUserSuccess\",\"route\":\"/eventhandler/eventhandlesetdefmallsetting\"},{\"pubsubname\":\"pubsub\",\"topic\":\"UpdateGoodsToEs\",\"route\":\"/eventhandler/eventhandleeventupdategoodstoes\"}]"
					}
                    finally
                    {
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = 200;
                        context.Response.Body = originalResponseBody;
                        newResponse.Position = 0;
                        await context.Response.WriteAsync(JsonSerializer.Serialize(submodellist));
                    }
                }
            }
            else
            {
                await next(context);
            }
        }
        internal class SubscribeModel
        {
            public SubscribeModel(string pubsubname, string topic, string route)
            {
                this.pubsubname = pubsubname;
                this.topic = topic;
                this.route = route;
            }
            public string pubsubname { get; set; }
            public string topic { get; set; }
            public string route { get; set; }
        }
    }

}
