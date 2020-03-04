using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Data
{
    public class Cart
    {
        private readonly List<CartLine> _lineCollection = new List<CartLine>();

        public virtual void AddItem(Product product, int quantity)
        {
            CartLine line = _lineCollection.FirstOrDefault(p => p.Product.Id == product.Id);

            if (line == null)
            {
                _lineCollection.Add(new CartLine()
                {
                    Product = product,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }


        }

        public virtual void RemoveLine(Product product) =>
            _lineCollection.RemoveAll(l => l.Product.Id == product.Id);

        public virtual decimal ComputeTotalValue() => _lineCollection.Sum(e => e.Product.Price * e.Quantity);

        public virtual void Clear() => _lineCollection.Clear();

        public virtual IEnumerable<CartLine> Lines => _lineCollection;
    }

    public class CartLine
    {
        public int CartLineId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }

    }
}
