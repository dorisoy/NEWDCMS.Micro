using RPCDapr.Client.ServerSymbol.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPCDapr.Client.ServerProxyFactory.Interface
{
    /// <summary>
    /// 用于发布事件
    /// </summary>
    public interface IEventBus
    {
        /// <summary>
        /// 发布事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="topic"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<DefaultResponse> SendEvent<T>(string topic, T input);
    }
}
