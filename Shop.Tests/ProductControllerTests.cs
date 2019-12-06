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
        public void Can_Add_Product()
        {
            Mock<IUnitOfWork> mock = new Mock<IUnitOfWork>();
            var categoriesRepo = new Mock<CategoryRepository>();
            categoriesRepo.Setup(c => c.GetAll()).Returns(new List<Category>
            {
                new Category {Id = 1,Name = "Cat1"},
                new Category {Id = 2,Name = "Cat2"}
            });


            var productRepoMock = new Mock<ProductRepository>();

           
            mock.Setup(c => c.Products).Returns(productRepoMock.Object);
            mock.Setup(c => c.Categories).Returns(categoriesRepo.Object);

                
        }
    }
}
