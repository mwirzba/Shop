using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Shop.Controllers;
using Shop.Data.Repositories;
using Shop.Dtos;
using Shop.Models;
using Shop.Tests.Bulders;
using Shop.Tests.MockClasses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shop.Tests
{
    [TestFixture]
    class OrderControllerTests
    {
        private OrdersController _orderController;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly IMapper _mapper;
        private Mock<FakeUserManager> _userManagerMock;
        public OrderControllerTests()
        {
            _mapper = new Mapper(MapperHelpers.GetMapperConfiguration());
            _unitOfWork = new Mock<IUnitOfWork>();
            _userManagerMock = new Mock<FakeUserManager>();
            _orderController = new OrdersController(
                _unitOfWork.Object,
                _mapper,
                _userManagerMock.Object
            );
        }

        [Test]
        public async Task GetOrdersAsync_ShouldReturnOkObjectResultWithOrders()
        {
            IEnumerable<Order> orders= new List<Order>
            {
                An.Order.WithId(0)
                        .WithCity("Gdansk2")
                        .WithEmail("email@wp.pl")
                        .WithStatusId(1),

                An.Order.WithId(0)
                        .WithCity("Gdanskds")
                        .WithEmail("email2@wp.pl")
                        .WithStatusId(1)
            };

            _unitOfWork.Setup(u => u.Orders.GetOrdersWithLines()).ReturnsAsync(orders);

            var result = await _orderController.GetOrdersAsync();
            var okObjectResult = result as OkObjectResult;
            IEnumerable<Order> resultFromController = _mapper.Map<IEnumerable<OrderDto>, IEnumerable<Order>>((IEnumerable<OrderDto>)okObjectResult.Value);

            //Assert
            okObjectResult.Should().NotBeNull();
            okObjectResult.StatusCode.Should().Be(StatusCodes.Status200OK);
            resultFromController.Should().BeEquivalentTo(orders);
        }
    }
}
