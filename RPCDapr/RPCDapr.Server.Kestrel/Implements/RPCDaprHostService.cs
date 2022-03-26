using Autofac;
using Microsoft.Extensions.Hosting;
using RPCDapr.Common.Implements;
using RPCDapr.PrRPCDaprerator.Implements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RPCDapr.Server.Kestrel.Implements
{
    /// <summary>
    /// 自定义主机服务
    /// </summary>
    internal class RPCDaprHostService : IHostedService
    {
        public RPCDaprHostService(ILifetimeScope lifetimeScope)
        {
            RPCDaprIocContainer.BuilderIocContainer(lifetimeScope);
            //初始化消息发送代理
            RemotePrRPCDaprerator.InitRemoteMessageSenderDelegate();
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}
