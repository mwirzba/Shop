﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Shop.Controllers;
using Shop.Data;
using Shop.Data.Repositories;
using Shop.Dtos;
using Shop.Models;
using Shop.Respn;
using Shop.ResponseHelpers;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Shop.Tests.Bulders;

namespace Shop.Tests
{
    [TestFixture]
    class ProductControllerTests
    {
        public ProductControllerTests()
        {
            _productRepo = new Mock<IProductRepository>();
            _categoriesRepo = new Mock<ICategoryRepository>();       
        }

        [SetUp]
        public void MockData()
        {
            _mock = GetConfiguredMockObject();
            _productController = new ProductsController(_mock.Object, new Mapper(MapperHelpers.GetMapperConfiguration()));
        }

        private Mock<IUnitOfWork> _mock;
        private Mock<IProductRepository> _productRepo;
        private Mock<ICategoryRepository> _categoriesRepo;
        private ProductsController _productController;

        private Mock<IUnitOfWork> GetConfiguredMockObject()
        {
            Mock<IUnitOfWork> mock = new Mock<IUnitOfWork>();
            _categoriesRepo.Setup(c => c.GetAllAsync()).
                ReturnsAsync(
                    new List<Category>
                    {
                        A.Category.WithId(1).WithName("Cat1"),
                        A.Category.WithId(2).WithName("Cat1")
                    }); 
            var productsList = new List<Product>
            {
               A.Product.WithId(1).WithName("prod1"),
               A.Product.WithId(2).WithName("prod2"),
               A.Product.WithId(2).WithName("prod2")
            };
            var pagedList = new PagedList<Product>(productsList,7,1,5);

            _productRepo.Setup(p => p.GetProductsWthCategoriesAsync()).ReturnsAsync(productsList);
            _productRepo.Setup(p => p.GetProductWthCategorieAsync(1)).ReturnsAsync(productsList[0]);
            _productRepo.Setup(p => p.GetPagedProductsWthCategoriesByFiltersAsync(new PaginationQuery(),null)).ReturnsAsync(pagedList);
            _productRepo.Setup(r => r.GetAsync(1)).ReturnsAsync(new Product());
            _productRepo.Setup(r => r.AddAsync(new Product())).Returns(Task.CompletedTask);
            _productRepo.Setup(r => r.Remove(new Product()));

            mock.Setup(c => c.Categories).Returns(_categoriesRepo.Object);
            mock.Setup(c => c.Products).Returns(_productRepo.Object);
            return mock;
        }

        private List<ProductDto> GetProductsList()
        {
            return new List<ProductDto>
            {
               A.ProductDto.WithId(1).WithName("prod1").WithCategoryId(1),
               A.ProductDto.WithId(2).WithName("prod2").WithCategoryId(1),
               A.ProductDto.WithId(3).WithName("prod3").WithCategoryId(1)
            };
        }


        [Test]
        public async Task GetProductsAsync_ShouldReturnAllProductsDto()
        {
            var resultFromController = await _productController.GetProductsAsync();
            var result = resultFromController as OkObjectResult;
            List<ProductDto> productsList = result.Value as List<ProductDto>;

            //Assert
            productsList.Should().NotBeNull();
            productsList.Should().HaveCount(GetProductsList().Count);
        }

        [Test]
        public async Task GetProductsAsync_IfRepoReturnsNull_ShouldReturnNotFound()
        {
            var emptyProductsList = new List<Product>();
            _productRepo.Setup(p => p.GetProductsWthCategoriesAsync()).ReturnsAsync((IEnumerable<Product>)null);
            var resultFromController = await _productController.GetProductsAsync();
            var result = resultFromController as NotFoundResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Test]
        public async Task GetProductsAsync_ShouldReturnOk()
        {
            var resultFromController = await _productController.GetProductsAsync();
            var result = resultFromController as OkObjectResult;

            //Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Test]
        public async Task GetProductAsync_ValidProductId_ShouldReturnProductDto()
        {
            var resultFromController = await _productController.GetProductAsync(1);
            var okResult = resultFromController as OkObjectResult;
            ProductDto productDto = okResult.Value as ProductDto;
            var products = GetProductsList();
            var productWithIdOne = products.Find(p => p.Id == 1);

            //Assert
            productWithIdOne.Should().NotBeNull();
            productDto.Name.Should().Be(productWithIdOne.Name);
        }

        [Test]
        public async Task GetProductAsync_ValidProductId_ShouldReturnOk()
        {
            var resultFromController = await _productController.GetProductAsync(1);
            var result = resultFromController as OkObjectResult;

            //Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Test]
        public async Task GetProductAsync_InValidProductId_ShouldReturnNotFound()
        {
            var resultFromController = await _productController.GetProductAsync(2);
            var result = resultFromController as NotFoundResult;

            //Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Test]
        public async Task PostProductAsync_ValidProduct_ShouldReturnOK()
        {
            ProductDto productDto  = A.ProductDto.WithId(3);

            var resultFromController = await _productController.PostProductAsync(productDto);
            var result = resultFromController as OkResult;

            //Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
        }


        [Test]
        public async Task PostProductAsync_NullProduct_ShouldReturnBadRequest()
        {
            var resultFromController = await _productController.PostProductAsync(null);
            var result = resultFromController as BadRequestResult;

            //Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }


        [Test]
        public async Task PutProductAsync_ValidProduct_ShouldReturnOK()
        {
            var updatedProduct = A.ProductDto.WithId(1)
                                             .WithName("prodnew")
                                             .WithCategoryId(3);

            var resultFromController = await _productController.PutProductAsync(updatedProduct,1);
            var result = resultFromController as NoContentResult;

            //Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }

        [Test]
        public async Task DeleteProductAsync_ValidId_ShouldReturn200StatusCode()
        {
            var resultFromController = await _productController.DeleteProductAsync(1);
            var result = resultFromController as OkResult;

            //Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);

        }


        [Test]
        public async Task DeleteProductAsync_InValidId_ShouldReturnNotFound()
        {
            Mock<IUnitOfWork> mock = GetConfiguredMockObject();
            ProductsController productController = new ProductsController(mock.Object, new Mapper(MapperHelpers.GetMapperConfiguration()));
            var resultFromController = await productController.DeleteProductAsync(2);
            var result = resultFromController as NotFoundResult;

            //Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }
    }
}
 