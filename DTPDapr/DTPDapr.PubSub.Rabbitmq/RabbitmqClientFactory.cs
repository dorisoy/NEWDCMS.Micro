using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTPDapr.PubSub.Rabbitmq
{
    public class RabbitmqClientFactory
    {
        static Lazy<IModel> _RabbitClient = new Lazy<IModel>(() => 
        {
            try
            {
                var url = ConfigurationManager.GetConfig()?.MessageQueueConnectionString;
                if (!string.IsNullOrEmpty(url))
                {
                    var channel = new ConnectionFactory()
                    {
                        Uri = new Uri(url)
                    }
                    .CreateConnection()
                    .CreateModel();
                    return channel;
                }
                return null;
            }
            catch(Exception)
			{
                return null;
            }

        });
        public static IModel GetClient()
        {
            return _RabbitClient.Value;
        }
    }
}
