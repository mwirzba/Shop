using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Shop.Controllers;
using Shop.Data;
using Shop.Data.Repositories;
using Shop.Models;
using Shop.Models.ProductDtos;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Shop.Tests
{
    class ProductControllerTests
    {
        public ProductControllerTests()
        {
            _productRepo = new Mock<IProductRepository>();
            _categoriesRepo = new Mock<ICategoryRepository>();       
        }

        private void MockData()
        {
            _mock = GetConfiguredMockObject();
            _productController = new ProductsController(_mock.Object, new Mapper(CreateConfiguration()));
        }

        private Mock<IUnitOfWork> _mock;
        private Mock<IProductRepository> _productRepo;
        private Mock<ICategoryRepository> _categoriesRepo;
        private ProductsController _productController;


        private MapperConfiguration CreateConfiguration()
        {
            var config = new MapperConfiguration(cfg =>
            {
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
            _productRepo.Setup(p => p.GetProductsWthCategoriesAsync()).ReturnsAsync(productsList);
            _productRepo.Setup(p => p.GetProductWthCategorieAsync(1)).ReturnsAsync(productsList[0]);
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
               new ProductDto {Id =1,Name="prod1",CategoryId = 1 },
               new ProductDto {Id =2,Name="prod2",CategoryId = 2 }
            };
        }


        [Test]
        public async Task GetProductsAsync_ShouldReturnAllProductsDto()
        {
            MockData();
            var resultFromController = await _productController.GetProductsAsync();
            var result = resultFromController as OkObjectResult;
            List<ProductDto> productsList = result.Value as List<ProductDto>;

            Assert.IsNotNull(productsList);
            Assert.AreEqual(GetProductsList().Count, productsList.Count);
        }

        [Test]
        public async Task GetProductsAsync_IfRepoReturnsNull_ShouldReturnNotFound()
        {

            var emptyProductsList = new List<Product>();
            _productRepo.Setup(p => p.GetProductsWthCategoriesAsync()).ReturnsAsync((IEnumerable<Product>)null);
            var resultFromController = await _productController.GetProductsAsync();
            var result = resultFromController as NotFoundResult;
  
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status404NotFound,result.StatusCode);
        }

        [Test]
        public async Task GetProductsAsync_ShouldReturnOk()
        {
            MockData();
            var resultFromController = await _productController.GetProductsAsync();
            var okResult = resultFromController as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(StatusCodes.Status200OK,okResult.StatusCode);
        }

        [Test]
        public async Task GetProductAsync_ValidProductId_ShouldReturnProductDto()
        {

            MockData();
            var resultFromController = await _productController.GetProductAsync(1);
            var okResult = resultFromController as OkObjectResult;
            ProductDto productDto = okResult.Value as ProductDto;
            var products = GetProductsList();
            var productWithIdOne = products.Find(p => p.Id == 1);

            Assert.IsNotNull(productWithIdOne);
            Assert.AreEqual(productWithIdOne.Name, productDto.Name);
        }

        [Test]
        public async Task GetProductAsync_ValidProductId_ShouldReturnOk()
        {
            MockData();
            var resultFromController = await _productController.GetProductAsync(1);
            var okResult = resultFromController as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [Test]
        public async Task GetProductAsync_InValidProductId_ShouldReturnNotFound()
        {
            MockData();
            var resultFromController = await _productController.GetProductAsync(2);
            var result = resultFromController as NotFoundResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
        }

        [Test]
        public async Task PostProductAsync_ValidProduct_ShouldReturnOK()
        {
            MockData();
            var productDto  = new ProductDto { Id = 3,Price=1, CategoryId = 1 };

            var resultFromController = await _productController.PostProductAsync(productDto);
            var okResult = resultFromController as OkResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(StatusCodes.Status200OK,okResult.StatusCode);
        }


        [Test]
        public async Task PostProductAsync_NullProduct_ShouldReturnBadRequest()
        {
            MockData();
            var resultFromController = await _productController.PostProductAsync(null);
            var result = resultFromController as BadRequestResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
        }


        [Test]
        public async Task EditProductAsync_ValidProduct_ShouldReturnOK()
        {
            MockData();
            Mock<IUnitOfWork> mock = GetConfiguredMockObject();
            ProductsController productController = new ProductsController(mock.Object, new Mapper(CreateConfiguration()));
            var updatedProduct= new ProductDto { Id = 1, Name = "prodnew", CategoryId = 3 };
           
            var resultFromController = await productController.PutProductAsync(updatedProduct,1);
            var okResult = resultFromController as NoContentResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(StatusCodes.Status204NoContent,okResult.StatusCode);
        }

        [Test]
        public async Task DeleteProductAsync_ValidId_ShouldReturn204StatusCode()
        {
            MockData();
            Mock<IUnitOfWork> mock = GetConfiguredMockObject();
            ProductsController productController = new ProductsController(mock.Object, new Mapper(CreateConfiguration()));
            var resultFromController = await productController.DeleteProductAsync(1);
            var okResult = resultFromController as NoContentResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(StatusCodes.Status204NoContent,okResult.StatusCode);

        }


        [Test]
        public async Task DeleteProductAsync_InValidId_ShouldReturnNotFound()
        {
            MockData();
            Mock<IUnitOfWork> mock = GetConfiguredMockObject();
            ProductsController productController = new ProductsController(mock.Object, new Mapper(CreateConfiguration()));
            var resultFromController = await productController.DeleteProductAsync(2);
            var okResult = resultFromController as NotFoundResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(StatusCodes.Status404NotFound,okResult.StatusCode);

        }

        [Test]
        public void Can_Validate_Wrong_Product()
        {
            var invalidProduct = new ProductDto{ Name = "", Id = 1 , Price =-1};
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(invalidProduct, new System.ComponentModel.DataAnnotations.ValidationContext(invalidProduct), validationResults);
            Assert.IsFalse(isValid);
        }
    }
}
