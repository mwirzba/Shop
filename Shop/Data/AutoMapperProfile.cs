using AutoMapper;
using Shop.Dtos;
using Shop.Models;
using System.Collections.Concurrent;

namespace Shop.Data
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(p => p.Category, opt => opt.MapFrom(src => src.Category.Name));

            CreateMap<ProductDto, Product>()
                .ForMember(p => p.Category, opt => opt.Ignore())
                .ForMember(p => p.Id, opt => opt.Ignore())
                .ForMember(p => p.CartLines, opt => opt.Ignore());

            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>()
                .ForMember(c => c.Products, opt => opt.Ignore())
                .ForMember(p => p.Id, opt => opt.Ignore());

            CreateMap<UserDto, User>();
            CreateMap<User, UserDto>();

            CreateMap<OrderDto, Order>()
                .ForMember(o => o.Id, opt => opt.Ignore())
                .ForMember(o => o.CartLines, opt => opt.Ignore());


            CreateMap<Order, OrderDto>();


            CreateMap<CartLine, CartLineDto>()
                .ForMember(p => p.Product, opt => opt.MapFrom(src => src.Product));


            CreateMap<OrderStatus, OrderStatusDto>();
            CreateMap<OrderStatusDto, OrderStatus>();

            CreateMap<OrderRequest, Order>()
                     .ForMember(o => o.Id, opt => opt.Ignore());

            CreateMap<CartLineRequest, CartLine>();

        }
    }
}
