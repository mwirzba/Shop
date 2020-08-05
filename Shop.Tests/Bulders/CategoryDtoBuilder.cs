using Shop.Dtos;

namespace Shop.Tests.Bulders
{
    public class CategoryDtoBuilder: Builder<CategoryDto>
    {
        public CategoryDtoBuilder WithName(string name)
        {
            _object.Name = name;
            return this;
        }

        public CategoryDtoBuilder WithId(int id)
        {
            _object.Id = id;
            return this;
        }
    }
}
