using Shop.Models;

namespace Shop.Tests.Bulders
{
    public class ProductBuilder: Builder<Product>
    {
        public ProductBuilder WithId(int id)
        {
            _object.Id = id;
            return this;
        }

        public ProductBuilder WithName(string name)
        {
            _object.Name = name;
            return this;
        }

    }
}
