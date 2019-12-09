using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Shop.Controllers;
using Shop.Data;
using Shop.Data.Repositories;
using Shop.Models;
using Shop.Models.ProductDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shop.Tests
{


    class ProductControllerTests
    {

        public ProductControllerTests()
        {
            productRepo = new Mock<IProductRepository>();
        }


        private Mock<IProductRepository> productRepo { get; set; }
        Mock<ICategoryRepository> categoriesRepo = new Mock<ICategoryRepository>();
        private MapperConfiguration CreateConfiguration()
        {
            var config = new MapperConfiguration(cfg =>
            {
                // Add all profiles in current assembly
                cfg.AddProfile(new AutoMapperProfile());
            });

            return config;
        }

        private Mock<IUnitOfWork> ConfigureRepositoryForTests()
        {
            Mock<IUnitOfWork> mock = new Mock<IUnitOfWork>();
            categoriesRepo.Setup(c => c.GetAllAsync()).ReturnsAsync(new List<Category>
            {
               new Category {Id = 1,Name = "Cat1"},
               new Category {Id = 2,Name = "Cat2"}
            }); 
            var productsList = new List<Product>
            {
               new Product {Id =1,Name="prod1",CategoryId = 1 },
               new Product {Id =2,Name="prod2",CategoryId = 2 }
            };
            productRepo.Setup(p => p.GetProductsWthCategoriesAsync()).ReturnsAsync(productsList);
            productRepo.Setup(p => p.GetProductWthCategorieAsync(1)).ReturnsAsync(productsList[0]);

            mock.Setup(c => c.Categories).Returns(categoriesRepo.Object);
            mock.Setup(c => c.Products).Returns(productRepo.Object);
            return mock;
        }


        [Test]
        public async Task Can_Get_Products()
        {
            Mock<IUnitOfWork> mock = ConfigureRepositoryForTests();

            ProductController productController = new ProductController(mock.Object, new Mapper(CreateConfiguration()));

            var resultFromController = await productController.GetProductsAsync();
            var okResult = resultFromController as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [Test]
        public async Task Can_Get_Product()
        {
            Mock<IUnitOfWork> mock = ConfigureRepositoryForTests();
            ProductController productController = new ProductController(mock.Object, new Mapper(CreateConfiguration()));

            var resultFromController = await productController.GetProductAsync(1);
            var okResult = resultFromController as OkObjectResult; 
            Assert.NotNull(okResult);
            Assert.AreEqual(200,okResult.StatusCode);
        }

        [Test]
        public async Task Can_Add_Product()
        {
            Mock<IUnitOfWork> mock = ConfigureRepositoryForTests();
            ProductController productController = new ProductController(mock.Object, new Mapper(CreateConfiguration()));
            var product = new Product { Id = 3, Name = "Prod3", CategoryId = 1 };
            var productDto  = new ProductDto { Id = 3, Name = "Prod3", CategoryId = 1 };

            productRepo.Setup(r => r.AddAsync(product));

            var resultFromController = await productController.PostProductAsync(productDto);
            var okResult = resultFromController as OkObjectResult;
            Assert.AreEqual(200, res.StatusCode);
        }
    }
}
