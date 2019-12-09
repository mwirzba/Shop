using AutoMapper;
using Shop.Models;
using Shop.Models.ProductDtos;

namespace Shop.Data
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>()
                .ForMember(p=>p.Category,opt => opt.Ignore())
                .ForMember(p=>p.Id,opt=>opt.Ignore());
            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>().ForMember(c => c.Products, opt => opt.Ignore());
        }
    }
}
