using RPCDapr.Client.ServerSymbol.Events;
using RPCDapr.Client.ServerSymbol.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPCDapr.Client.ServerProxyFactory.Interface
{

    /// <summary>
    /// 用于调用dapr的状态管理器
    /// </summary>
    public interface IStateManager
    {
        /// <summary>
        /// 设置状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<DefaultResponse> SetState(StateStore input);

        /// <summary>
        /// 删除状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<DefaultResponse> DelState(StateStore input);

        /// <summary>
        /// 获取状态对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<T> GetState<T>(StateStore input) where T : new();

        /// <summary>
        /// 获取状态对象
        /// </summary>
        /// <param name="input"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<object> GetState(StateStore input, Type type);
    }
}
