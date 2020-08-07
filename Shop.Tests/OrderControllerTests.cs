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
        private Mock<HttpContext> _httpContext;
        public OrderControllerTests()
        {
            _mapper = new Mapper(MapperHelpers.GetMapperConfiguration());
            _unitOfWork = new Mock<IUnitOfWork>();
            _httpContext = new Mock<HttpContext>();
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
            IEnumerable<Order> orders = new List<Order>
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

        [Test]
        public async Task GetOrderAsync_ValidOrderId_ShouldReturnOkObjectResultWithOrder()
        {
            Order order = An.Order
                            .WithId(0)
                            .WithCity("Gdansk2")
                            .WithEmail("email@wp.pl")
                            .WithStatusId(1);

            _unitOfWork.Setup(u => u.Orders.GetFullOrder(1)).ReturnsAsync(order);

            var result = await _orderController.GetOrderAsync(1);
            var okObjectResult = result as OkObjectResult;
            var resultFromController = _mapper.Map<OrderDto, Order>((OrderDto)okObjectResult.Value);

            //Assert
            okObjectResult.Should().NotBeNull();
            okObjectResult.StatusCode.Should().Be(StatusCodes.Status200OK);
            resultFromController.Should().BeEquivalentTo(order);
        }

        [Test]
        public async Task GetOrderAsync_InvalidOrderId_ShouldReturnNotFound()
        {
            _unitOfWork.Setup(u => u.Orders.GetFullOrder(1)).ReturnsAsync((Order)null);

            var result = await _orderController.GetOrderAsync(1);
            var notFoundResult = result as NotFoundResult;

            //Assert
            notFoundResult.Should().NotBeNull();
            notFoundResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Test]
        public async Task PostOrderAsync_ValidOrderRequest_ShouldOkStatus()
        {
            _unitOfWork.Setup(u => u.Orders.GetFullOrder(1)).ReturnsAsync((Order)null);
            _httpContext.Setup(d => d.User.Identity.IsAuthenticated).Returns(true);
            _orderController.ControllerContext = new ControllerContext
            {
                HttpContext = _httpContext.Object
            };

            User user = A.User
                         .WithId("123")
                         .WithUserName("user1")
                         .WithEmail("mwirzba@wp.pl");

            ProductDto productDto1 = A.ProductDto.WithId(1).WithName("prod1");
            ProductDto productDto2 = A.ProductDto.WithId(2).WithName("prod2");

            var cartLines = new CartLineRequest[]
            {
                A.CartlineRequest.WithId(1).WithQuantity(2).WithProductId(1),
                A.CartlineRequest.WithId(2).WithQuantity(3).WithProductId(2)
            };

            OrderRequest orderReq = An.OrderRequest.WithCartLines(cartLines);
            _userManagerMock.Setup(u => u.GetUserAsync(_httpContext.Object.User)).ReturnsAsync(user);
           

            var result = await _orderController.PostOrderAsync(orderReq);
            var okResult = result as OkResult;

            //Assert
            _unitOfWork.Verify(u => u.Orders.AddAsync(It.IsAny<Order>()), Times.Once);
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Test]
        public async Task PostOrderAsync_NullOrderDto_ShouldReturn_UnprocessableEntity()
        {
            var result = await _orderController.PostOrderAsync(null);
            var unprocessableEntityResult = result as UnprocessableEntityResult;


            //Assert
            unprocessableEntityResult.Should().NotBeNull();
            unprocessableEntityResult.StatusCode.Should().Be(StatusCodes.Status422UnprocessableEntity);
        }
    }
}
