using Moq;
using NUnit.Framework;
using Shop.Data.Repositories;
using Shop.Models;
using System.Collections.Generic;
using System.Linq;

namespace Shop.Tests
{
    class ProductControllerTests
    {
        [Test]
        public void Can_Get_Products()
        {
            Mock<IUnitOfWork> mock = new Mock<IUnitOfWork>();
            //var categoriesRepo = new Mock<CategoryRepository>();
            //categoriesRepo.Setup(c => c.GetAll()).Returns(new List<Category>
            //{
            //    new Category {Id = 1,Name = "Cat1"},
            //    new Category {Id = 2,Name = "Cat2"}
            //});
            //
            //var categeries = categoriesRepo.Object.GetAll();
            //var productRepoMock = new Mock<ProductRepository>();
            //productRepoMock.Setup(p => p.GetProductsWthCategories()).Returns(new List<Product>
            //{
            //    new Product {Id =1,Name="prod1",Category = categeries.First(c=>c.Id == 1) },
            //    new Product {Id =1,Name="prod1",Category = categeries.First(c=>c.Id == 2) }
            //});
            //
           // mock.Setup(c => c.Products).Returns(productRepoMock.Object);
           // mock.Setup(c => c.Categories).Returns(categoriesRepo.Object);
           //
                
        }
    }
}
