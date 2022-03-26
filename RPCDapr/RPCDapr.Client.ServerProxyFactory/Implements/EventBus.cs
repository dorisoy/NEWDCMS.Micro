using RPCDapr.Client.ServerProxyFactory.Interface;
using RPCDapr.Client.ServerSymbol.Events;
using RPCDapr.Common;
using RPCDapr.PrRPCDaprerator.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPCDapr.Client.ServerProxyFactory.Implements
{

    /// <summary>
    /// 用于发布事件
    /// </summary>
    public class EventBus : IEventBus
    {
        private readonly IRemoteMessageSender messageSender;
        public EventBus(IRemoteMessageSender messageSender)
        {
            this.messageSender = messageSender;
        }

        /// <summary>
        /// 发布事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="topic"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<DefaultResponse> SendEvent<T>(string topic, T input)
        {
            return await messageSender.SendMessage<DefaultResponse>(DaprConfig.GetCurrent().PubSubCompentName, $"/{topic}", input, SendType.publish);
        }
    }
}