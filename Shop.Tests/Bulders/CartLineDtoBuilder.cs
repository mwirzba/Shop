using Shop.Dtos;

namespace Shop.Tests.Bulders
{
    public class CartLineDtoBuilder: Builder<CartLineDto>
    {
        public CartLineDtoBuilder WithId(int id)
        {
            _object.Id = id;
            return this;
        }
        public CartLineDtoBuilder WithQuantity(int quantity)
        {
            _object.Quantity = quantity;
            return this;
        }

        public CartLineDtoBuilder WithProduct(ProductDto product)
        {
            _object.Product = product;
            return this;
        }
        public CartLineDtoBuilder WithOrder(OrderDto order)
        {
            _object.Order = order;
            return this;
        }
    }
}
