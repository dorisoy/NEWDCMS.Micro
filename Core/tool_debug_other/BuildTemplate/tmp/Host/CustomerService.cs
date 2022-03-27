using IApplicationService.AccountService.Dtos.Input;
using IApplicationService.AppEvent;
using Infrastructure.EfDataAccess;
using InfrastructureBase.AuthBase;
using Microsoft.Extensions.Hosting;
using RPCDapr.Client.ServerProxyFactory.Interface;
using RPCDapr.Client.ServerSymbol.Events;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Host
{
    public class CustomerService : IHostedService
    {
        private readonly EfDbContext dbContext;
        private readonly IEventBus eventBus;
        public CustomerService(EfDbContext dbContext, IEventBus eventBus)
        {
            this.dbContext = dbContext;
            this.eventBus = eventBus;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            dbContext.Database.EnsureCreated();//�Զ�Ǩ�����ݿ�
            _ = Task.Run(async () =>
            {
                while (true)
                {
                    Thread.Sleep(20000);//�ȴ�sidercar����
                    var sender = await eventBus.SendEvent(EventTopicDictionary.Common.InitAuthApiList, new InitPermissionApiEvent<List<AuthenticationInfo>>(AuthenticationManager.AuthenticationMethods));//����ǰ��������Ȩ�ӿڷ��͸��û�����
                    if (sender != default(DefaultResponse))
                        break;
                    else
                        Console.WriteLine("�¼���ʼ��ʧ�ܣ�20�������!");
                }
            });
            await Task.CompletedTask;
        }
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}
