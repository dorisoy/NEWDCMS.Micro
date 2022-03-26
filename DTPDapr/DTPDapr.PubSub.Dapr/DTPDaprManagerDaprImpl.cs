using RPCDapr.Client.ServerProxyFactory.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTPDapr.PubSub.Dapr
{
    public class DTPDaprManagerDaprImpl : IDTPDaprManager
    {
        private readonly IEventBus eventBus;
        private readonly DTPDaprConfiguration DTPDaprConfiguration;
        public DTPDaprManagerDaprImpl(IEventBus eventBus)
        {
            DTPDaprConfiguration = ConfigurationManager.GetConfig();
            this.eventBus = eventBus;
        }
        public async Task StartOrNext<T>(string topic, T data)
        {
            await eventBus.SendEvent($"{topic.Split(".")[0]}", new DTPDaprData(DTPDaprConfiguration.GetflowNameByTopic(topic), topic, data));
        }
    }
}
