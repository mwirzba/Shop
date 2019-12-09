using AutoMapper;
using Moq;
using NUnit.Framework;
using Shop.Controllers;
using Shop.Data;
using Shop.Data.Repositories;
using Shop.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shop.Tests
{


    class ProductControllerTests
    {
        private MapperConfiguration CreateConfiguration()
        {
            var config = new MapperConfiguration(cfg =>
            {
                // Add all profiles in current assembly
                cfg.AddProfile(new AutoMapperProfile());
            });

            return config;
        }


        [Test]
        public async Task Can_Get_Products()
        {
           Mock<IUnitOfWork> mock = new Mock<IUnitOfWork>();
           var categoriesRepo = new Mock<ICategoryRepository>();
           categoriesRepo.Setup(c => c.GetAllAsync()).ReturnsAsync(new List<Category>
           {
               new Category {Id = 1,Name = "Cat1"},
               new Category {Id = 2,Name = "Cat2"}
           });
           var productRepo = new Mock<IProductRepository>();
           var productsList = new List<Product>
           {
               new Product {Id =1,Name="prod1",CategoryId = 1 },
               new Product {Id =1,Name="prod1",CategoryId = 2 }
           };
           productRepo.Setup(p => p.GetProductsWthCategoriesAsync()).ReturnsAsync(productsList);
         
           mock.Setup(c => c.Categories).Returns(categoriesRepo.Object);
           mock.Setup(c => c.Products).Returns(productRepo.Object);
         
           ProductController productController = new ProductController(mock.Object,new Mapper(CreateConfiguration()));
         
           var resultFromController = (List<Product>)await mock.Object.Products.GetProductsWthCategoriesAsync();
         
         
           Assert.AreEqual(productsList[0].Name,resultFromController[0].Name);
               
        }
    }
}
