
using Shop.Dtos;

namespace Shop.Tests.Bulders
{
    public class ProductDtoBuilder:  Builder<ProductDto>
    {
        public ProductDtoBuilder WithId(int id)
        {
            _object.Id = id;
            return this;
        }

        public ProductDtoBuilder WithName(string name)
        {
            _object.Name = name;
            return this;
        }

        public ProductDtoBuilder WithCategoryId(int id)
        {
            _object.CategoryId = id;
            return this;
        }
    }
}
