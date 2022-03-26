using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTPDapr
{
    /// <summary>
    /// 事件订阅器
    /// </summary>
    public interface IDTPDaprEventHandler
    {
        /// <summary>
        /// 订阅处理方法
        /// </summary>
        /// <param name="DTPDaprConfiguration"></param>
        /// <param name="rootProvider"></param>
        /// <returns></returns>
        void ConsumerReceivedHandle(IApplicationBuilder applicationBuilder);
    }
}
