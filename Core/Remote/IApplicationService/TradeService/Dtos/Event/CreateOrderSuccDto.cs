using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.TradeService.Dtos.Event
{
    public class OperateOrderSuccDto
    {
        public int OrderId { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
    }
}
