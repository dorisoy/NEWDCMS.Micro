using IApplicationService.DTPDaprs.CreateOrder.Dtos;
using DTPDapr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.DTPDaprs.CreateOrder.Handles
{
    public interface IGoodsHandler
    {
        [DTPDaprLogicHandler(Topics.GoodsHandler.PreDeductInventory, HandleType.Handle)]
        Task<DeductionStockDto> DeductInventory(DeductionStockDto dto);
        [DTPDaprLogicHandler(Topics.GoodsHandler.InventoryRollback, HandleType.Rollback)]
        Task InventoryRollback(DeductionStockDto dto);
    }
}
