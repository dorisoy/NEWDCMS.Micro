using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class CreateOrderDeductionGoodsStockDto
    {
        public CreateOrderDeductionGoodsStockDto(int goodsId, int count)
        {
            GoodsId = goodsId;
            DeductionStock = count;
        }
        public int GoodsId { get; set; }
        public int DeductionStock { get; set; }
    }
}
