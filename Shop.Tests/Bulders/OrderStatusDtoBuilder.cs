using Shop.Dtos;

namespace Shop.Tests.Bulders
{
    public class OrderStatusDtoBuilder: Builder<OrderStatusDto>
    {
        public OrderStatusDtoBuilder WithId(int id)
        {
            _object.Id = id;
            return this;
        }

        public OrderStatusDtoBuilder WithName(string name)
        {
            _object.Name = name;
            return this;
        }
    }
}