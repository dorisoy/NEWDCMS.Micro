using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Elasticsearch
{
    public class EsGoodsDto
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal OldPrice { get; set; }
        public string Description { get; set; }
        public int SellCount { get; set; }
        public string Info { get; set; }
        public string Icon { get; set; }
    }
}
