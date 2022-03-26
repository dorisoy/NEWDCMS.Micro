using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace DTPDapr.PubSub.Rabbitmq
{
    public class DTPDaprManagerRabbitmqImpl : IDTPDaprManager
    {
        IModel RabbitClient;
        private readonly DTPDaprConfiguration DTPDaprConfiguration;
        public DTPDaprManagerRabbitmqImpl()
        {
            DTPDaprConfiguration = ConfigurationManager.GetConfig();
            RabbitClient = RabbitmqClientFactory.GetClient();
        }
        public async Task StartOrNext<T>(string topic, T data)
        {
            RabbitClient.ExchangeDeclare(DTPDaprConfiguration.ClusterName, "topic");
            RabbitClient.BasicPublish(DTPDaprConfiguration.ClusterName, topic, null, JsonSerializer.SerializeToUtf8Bytes(new DTPDaprData(DTPDaprConfiguration.GetflowNameByTopic(topic), topic, data)));
            await Task.CompletedTask;
        }
    }
}
