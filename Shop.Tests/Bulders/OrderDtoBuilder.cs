using Shop.Dtos;

namespace Shop.Tests.Bulders
{
    public class OrderDtoBuilder: Builder<OrderDto>
    {
        public OrderDtoBuilder WithId(long id)
        {
            _object.Id = id;
            return this;
        }

        public OrderDtoBuilder WithUserFirstName(string name)
        {
            _object.Name = name;
            return this;
        }

        public OrderDtoBuilder WithUserSurname(string surname)
        {
            _object.SurName = surname;
            return this;
        }
        public OrderDtoBuilder WithEmail(string email)
        {
            _object.Email = email;
            return this;
        }
        public OrderDtoBuilder WithCity(string city)
        {
            _object.City = city;
            return this;
        }
        public OrderDtoBuilder WithStatusId(int id)
        {
            _object.StatusId = id;
            return this;
        }

        public OrderDtoBuilder WithUser(string userName)
        {
            _object.UserName = userName;
            return this;
        }
        public OrderDtoBuilder WithGiftWrap(bool giftWrap)
        {
            _object.GiftWrap = giftWrap;
            return this;
        }
        public OrderDtoBuilder WithCartLines(CartLineDto[] cartlines)
        {
            _object.CartLines = cartlines;
            return this;
        }

    }
}
