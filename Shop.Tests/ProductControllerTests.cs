﻿using AutoMapper;
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
            _productRepo = new Mock<IProductRepository>();
            _categoriesRepo = new Mock<ICategoryRepository>();
            _mock = GetConfiguredMockObject();
            _productController = new ProductController(_mock.Object, new Mapper(CreateConfiguration()));
        }

        private Mock<IUnitOfWork> _mock;
        private Mock<IProductRepository> _productRepo;
        private Mock<ICategoryRepository> _categoriesRepo;
        private ProductController _productController;


        private MapperConfiguration CreateConfiguration()
        {
            var config = new MapperConfiguration(cfg =>
            {
                // Add all profiles in current assembly
                cfg.AddProfile(new AutoMapperProfile());
            });

            return config;
        }

        private Mock<IUnitOfWork> GetConfiguredMockObject()
        {
            Mock<IUnitOfWork> mock = new Mock<IUnitOfWork>();
            _categoriesRepo.Setup(c => c.GetAllAsync()).ReturnsAsync(new List<Category>
            {
               new Category {Id = 1,Name = "Cat1"},
               new Category {Id = 2,Name = "Cat2"}
            }); 
            var productsList = new List<Product>
            {
               new Product {Id =1,Name="prod1",CategoryId = 1 },
               new Product {Id =2,Name="prod2",CategoryId = 2 }
            };
            _productRepo.Setup(p => p.GetProductsWthCategoriesAsync()).ReturnsAsync(new List<Product>());
            _productRepo.Setup(p => p.GetProductWthCategorieAsync(1)).ReturnsAsync(new Product());
            _productRepo.Setup(r => r.Update(new Product()));
            _productRepo.Setup(r => r.AddAsync(new Product())).Returns(Task.CompletedTask);
            _productRepo.Setup(r => r.Remove(new Product()));

            mock.Setup(c => c.Categories).Returns(_categoriesRepo.Object);
            mock.Setup(c => c.Products).Returns(_productRepo.Object);
            return mock;
        }


        [Test]
        public async Task Can_Get_Products()
        {
            var resultFromController = await _productController.GetProductsAsync();
            var okResult = resultFromController as OkObjectResult;

            Assert.IsTrue(okResult != null && okResult.StatusCode == 200);
        }

        [Test]
        public async Task Can_Get_Product()
        {
            var resultFromController = await _productController.GetProductAsync(1);
            var okResult = resultFromController as OkObjectResult;

            Assert.IsTrue(okResult != null && okResult.StatusCode == 200);
        }

        [Test]
        public async Task Can_Add_Product()
        {
            var product = new Product { Id = 3, Name = "Prod3", CategoryId = 1 };
            var productDto  = new ProductDto { Id = 3, Name = "Prod3", CategoryId = 1 };

            var resultFromController = await _productController.PostProductAsync(productDto);
            var okResult = resultFromController as OkResult;

            Assert.IsTrue(okResult != null && okResult.StatusCode == 200);
        }


        [Test]
        public async Task Can_Update_Product()
        {
            Mock<IUnitOfWork> mock = GetConfiguredMockObject();
            ProductController productController = new ProductController(mock.Object, new Mapper(CreateConfiguration()));
            var updatedProduct= new ProductDto { Id = 1, Name = "prodnew", CategoryId = 3 };
           
            var resultFromController = await productController.EditProductAsync(updatedProduct, 1);
            var okResult = resultFromController as OkResult;

            Assert.IsTrue(okResult != null && okResult.StatusCode == 200);
        }

        [Test]
        public async Task Can_Delete_Product()
        {
            Mock<IUnitOfWork> mock = GetConfiguredMockObject();
            ProductController productController = new ProductController(mock.Object, new Mapper(CreateConfiguration()));
            var resultFromController = await productController.DeleteProductAsync(1);
            var okResult = resultFromController as OkResult;

            Assert.IsTrue(okResult != null && okResult.StatusCode == 200);
        }

    }
}
