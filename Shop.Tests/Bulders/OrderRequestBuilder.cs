using Shop.Dtos;

namespace Shop.Tests.Bulders
{
    public class OrderRequestBuilder:  Builder<OrderRequest>
    {
        public OrderRequestBuilder WithId(long id)
        {
            _object.Id = id;
            return this;
        }

        public OrderRequestBuilder WithUserFirstName(string name)
        {
            _object.Name = name;
            return this;
        }

        public OrderRequestBuilder WithUserSurname(string surname)
        {
            _object.SurName = surname;
            return this;
        }
        public OrderRequestBuilder WithEmail(string email)
        {
            _object.Email = email;
            return this;
        }
        public OrderRequestBuilder WithCity(string city)
        {
            _object.City = city;
            return this;
        }
        public OrderRequestBuilder WithStatusId(int id)
        {
            _object.StatusId = id;
            return this;
        }

        public OrderRequestBuilder WithUser(string userName)
        {
            _object.UserName = userName;
            return this;
        }
        public OrderRequestBuilder WithGiftWrap(bool giftWrap)
        {
            _object.GiftWrap = giftWrap;
            return this;
        }
        public OrderRequestBuilder WithCartLines(CartLineRequest[] cartlines)
        {
            _object.CartLines = cartlines;
            return this;
        }
    }
}
