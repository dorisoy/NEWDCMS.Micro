using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.TradeService.Dtos.Output
{
    public class GetOrderSellCountResponse
    {
        public int GoodsId { get; set; }
        public int CellCount { get; set; }
    }
}
