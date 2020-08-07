using Shop.Dtos;

namespace Shop.Tests.Bulders
{
    public class CartLineRequestBuilder: Builder<CartLineRequest>
    {
        public CartLineRequestBuilder WithId(int id)
        {
            _object.Id = id;
            return this;
        }
        public CartLineRequestBuilder WithQuantity(int quantity)
        {
            _object.Quantity = quantity;
            return this;
        }

        public CartLineRequestBuilder WithProductId(int Id)
        {
            _object.ProductId = Id;
            return this;
        }
    }
}
