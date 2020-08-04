using Shop.Models;

namespace Shop.Tests.Bulders
{
    public class OrderBuilder: Builder<Order>
    {
        public OrderBuilder WithId(long id)
        {
            _object.Id = id;
            return this;
        }

        public OrderBuilder WithUserFirstName(string name)
        {
            _object.Name = name;
            return this;
        }

        public OrderBuilder WithUserSurname(string surname)
        {
            _object.SurName = surname;
            return this;
        }
        public OrderBuilder WithEmail(string email)
        {
            _object.Email = email;
            return this;
        }
        public OrderBuilder WithCity(string city)
        {
            _object.City = city;
            return this;
        }
        public OrderBuilder WithStatusId(int id)
        {
            _object.StatusId = id;
            return this;
        }

        public OrderBuilder WithUser(User user)
        {
            _object.User = user;
            return this;
        }

        public OrderBuilder WithUserId(string userId)
        {
            _object.UserId = userId;
            return this;
        }
        public OrderBuilder WithGiftWrap(bool giftWrap)
        {
            _object.GiftWrap = giftWrap;
            return this;
        }
        public OrderBuilder WithCartLines(CartLine[] cartlines)
        {
            _object.CartLines = cartlines;
            return this;
        }
    }
}