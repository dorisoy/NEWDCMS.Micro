using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using DTPDapr.HandleBuilder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;

namespace DTPDapr.PubSub.Rabbitmq
{
    public class DTPDaprEventHandlerRabbitmqImpl : IDTPDaprEventHandler
    {
        IModel RabbitClient;
        EventingBasicConsumer eventingBasicConsumer;
        DTPDaprConfiguration DTPDaprConfiguration;
        public DTPDaprEventHandlerRabbitmqImpl()
        {
            DTPDaprConfiguration = ConfigurationManager.GetConfig();
            RabbitClient = RabbitmqClientFactory.GetClient();
            eventingBasicConsumer = new EventingBasicConsumer(RabbitClient);
        }
        public void ConsumerReceivedHandle(IApplicationBuilder applicationBuilder)
        {
            eventingBasicConsumer.Received += async (ch, args) => await HandAsync(args, applicationBuilder.ApplicationServices);
            RabbitClient?.ExchangeDeclare(DTPDaprConfiguration.ClusterName, "topic");
            RabbitClient?.QueueDeclare(DTPDaprConfiguration.ServiceName, false, false, false, null);
            RabbitClient?.QueueBind(DTPDaprConfiguration.ServiceName, DTPDaprConfiguration.ClusterName, $"{DTPDaprConfiguration.ServiceName}.#");
            RabbitClient?.BasicConsume(DTPDaprConfiguration.ServiceName, false, eventingBasicConsumer);
        }
        async Task HandAsync(BasicDeliverEventArgs args, IServiceProvider rootProvider)
        {
            //为每次订阅处理器构建独立的生命周期
            using (var scope = rootProvider.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                DTPDaprData data = default;
                var storeProvider = serviceProvider.GetService<IStoreProvider>();
                try
                {
                    data = JsonSerializer.Deserialize<DTPDaprData>(args.Body.Span);
                    var oldData = await storeProvider.GetKey(data.StoreKey);
                    if (oldData == null || oldData.StoreState == DTPDaprDataState.Error)
                    {
                        data.SetState(DTPDaprDataState.Processing);
                        await storeProvider.SetDataByKey(data.StoreKey, data, DateTime.Now.AddDays(1));
                        await HandleProxyFactory.GetDelegate().FirstOrDefault(x => x.Topic == args.RoutingKey).Excute(data, serviceProvider);
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
                    RabbitClient?.BasicAck(args.DeliveryTag, false);
                }
            }
        }
    }
}
