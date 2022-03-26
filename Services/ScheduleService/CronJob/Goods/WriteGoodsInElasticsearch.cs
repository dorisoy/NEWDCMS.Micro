using Hangfire;
using IApplicationService.AppEvent;
using IApplicationService.GoodsService.Dtos.Event;
using RPCDapr.Client.ServerProxyFactory.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScheduleService.CronJob.Goods
{
    public class WriteGoodsInElasticsearch : CronJobBase
    {
        public override void RunCornJob()
        {
            RecurringJob.AddOrUpdate<IEventBus>((bus) => bus.SendEvent(EventTopicDictionary.Goods.UpdateGoodsToEs, new UpdateGoodsToEsDto()), "*/5 * * * *");
        }
    }
}
