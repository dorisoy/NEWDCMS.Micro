using IApplicationService.DTPDaprs.CreateOrder.Dtos;
using IApplicationService.TradeService.Dtos.Input;
using DTPDapr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.DTPDaprs.CreateOrder.Handles
{
    public interface IOrderHandle
    {

        [DTPDaprLogicHandler(Topics.OrderHandler.OrderCreate, HandleType.Handle)]
        Task OrderCreate(OrderCreateDto dto);
    }
}
