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
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Tests
{
    class CategoryControllerTests
    {
        public CategoryControllerTests()
        {
            _categoriesRepo = new Mock<ICategoryRepository>();
        }

        private Mock<IUnitOfWork> _mock;
        private Mock<ICategoryRepository> _categoriesRepo;
        private CategoriesController _categoryController;

        private void MockData()
        {
            _mock = GetConfiguredMockObject();
            _categoryController = new CategoriesController(_mock.Object, new Mapper(CreateConfiguration()));
        }

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
            var categories = new List<Category>
            {
               new Category {Id = 1,Name = "Cat1"},
               new Category {Id = 2,Name = "Cat2"}
            }; 
            _categoriesRepo.Setup(c => c.GetAllAsync()).ReturnsAsync(categories);
            _categoriesRepo.Setup(p => p.GetAsync(1)).ReturnsAsync(categories[0]);
            _categoriesRepo.Setup(p => p.SingleOrDefaultAsync(c=> c.Id == 1)).ReturnsAsync(categories[0]);
            _categoriesRepo.Setup(r => r.AddAsync(new Category())).Returns(Task.CompletedTask);
            _categoriesRepo.Setup(r => r.Remove(new Category()));

            mock.Setup(c => c.Categories).Returns(_categoriesRepo.Object);
            return mock;
        }

        private List<CategoryDto> GetCategoriesList()
        {
            return new List<CategoryDto>
            {
               new CategoryDto {Id = 1,Name = "Cat1"},
               new CategoryDto {Id = 2,Name = "Cat2"}
            };
        }

        [Test]
        public async Task GetCategories_ShouldReturnAllCategoriesDto()
        {
            MockData();
            var resultFromController = await _categoryController.GetCategories();
            var result = resultFromController as OkObjectResult;
            List<CategoryDto> categories = result.Value as List<CategoryDto>;

            Assert.IsNotNull(categories);
            Assert.AreEqual(GetCategoriesList().Count, categories.Count);
        }

        [Test]
        public async Task GetCategories_IfRepoReturnsNull_ShouldReturnNotFound()
        {

            var emptyProductsList = new List<Category>();
            _categoriesRepo.Setup(p => p.GetAllAsync()).ReturnsAsync((IEnumerable<Category>)null);
            var resultFromController = await _categoryController.GetCategories();
            var result = resultFromController as NotFoundResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
        }

        [Test]
        public async Task GetCategories_ShouldReturnOk()
        {
            MockData();
            var resultFromController = await _categoryController.GetCategories();
            var okResult = resultFromController as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [Test]
        public async Task GetCategory_ValidCategoryId_ShouldReturnCategoryDto()
        {

            MockData();
            var resultFromController = await _categoryController.GetCategory(1);
            var okResult = resultFromController as OkObjectResult;
            CategoryDto categoryDto = okResult.Value as CategoryDto;
            var categories = GetCategoriesList();
            var categoryWithIdOne = categories.Find(p => p.Id == 1);

            Assert.IsNotNull(categoryWithIdOne);
            Assert.AreEqual(categoryWithIdOne.Name, categoryDto.Name);
        }

        [Test]
        public async Task GetCategory_ValidCategoryId_ShouldReturnOk()
        {
            MockData();
            var resultFromController = await _categoryController.GetCategory(1);
            var okResult = resultFromController as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [Test]
        public async Task GetCategory_InValidCategoryId_ShouldReturnNotFound()
        {
            MockData();
            var resultFromController = await _categoryController.GetCategory(2);
            var result = resultFromController as NotFoundResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
        }

        [Test]
        public async Task PostCategory_ValidCategory_ShouldReturnOK()
        {
            MockData();
            var categoryDto = new CategoryDto { Id = 3, Name ="name1"};

            var resultFromController = await _categoryController.PostCategory(categoryDto);
            var okResult = resultFromController as OkResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
        }


    }
}
