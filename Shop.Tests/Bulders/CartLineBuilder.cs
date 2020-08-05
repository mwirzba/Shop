using Shop.Models;

namespace Shop.Tests.Bulders
{
    public class CartLineBuilder: Builder<CartLine>
    {
        public CartLineBuilder WithId(int id)
        {
            _object.Id = id;
            return this;
        }
        public CartLineBuilder WithQuantity(int quantity)
        {
            _object.Quantity = quantity;
            return this;
        }
        public CartLineBuilder WithProductId(int productId)
        {
            _object.ProductId = productId;
            return this;
        }
        public CartLineBuilder WithProduct(Product product)
        {
            _object.Product = product;
            return this;
        }
        public CartLineBuilder WithOrder(Order order)
        {
            _object.Order = order;
            return this;
        }
        public CartLineBuilder WithOrderId(int orderId)
        {
            _object.OrderId = orderId;
            return this;
        }
    }
}
