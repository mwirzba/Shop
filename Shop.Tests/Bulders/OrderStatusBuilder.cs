using Shop.Models;

namespace Shop.Tests.Bulders
{
    public class OrderStatusBuilder: Builder<OrderStatus>
    {
        public OrderStatusBuilder WithId(int id)
        {
            _object.Id = id;
            return this;
        }

        public OrderStatusBuilder WithName(string name)
        {
            _object.Name = name;
            return this;
        }
    }
}
