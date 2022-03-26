using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class DeductionStockDto
    {
        public List<GoodsInfo> Items { get; set; }
        public class GoodsInfo
        {
            public int GoodsId { get; set; }
            public int Count { get; set; }
        }
    }
}
