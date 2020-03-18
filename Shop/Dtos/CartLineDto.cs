using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Dtos
{
    public class CartLineDto
    {
        public long Id { get; set; }
        public int Quantity { get; set; }
        public virtual ProductDto Product { get; set; }
        public virtual OrderDto Order { get; set; }
    }
}
