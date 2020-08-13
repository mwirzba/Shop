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
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;

namespace Shop.Tests
{
    [TestFixture]
    class OrderStatusControllerTests
    {
        public OrderStatusControllerTests()
        {
            _mapper = new Mapper(MapperHelpers.GetMapperConfiguration());
            _unitOfWork = new Mock<IUnitOfWork>();
            _orderStatusController = new OrderStatusController(_unitOfWork.Object,_mapper);
        }

        private Mapper _mapper;
        private Mock<IUnitOfWork> _unitOfWork;
        private OrderStatusController _orderStatusController;

        [Test]
        public async Task GetOrderStatus_ShouldReturnOkResultWithOrderStatuses()
        {
            var ordersInDb = new List<OrderStatus>
            {
                An.OrderStatus,
                An.OrderStatus
            };

            _unitOfWork.Setup(u => u.OrderStatuses.GetAllAsync())
                .ReturnsAsync(ordersInDb);
            var resultFromController = await _orderStatusController.GetOrderStatus();
            var okResult = resultFromController as OkObjectResult;

            //Assert
            okResult.Should().NotBeNull();
            okResult.Value.As<IEnumerable<OrderStatusDto>>().Should().HaveCount(2);
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Test]
        public async Task GetOrderStatus_ValidOrderId_ShouldReturnOkResultWithOrderStatus()
        {
            OrderStatus order = An.OrderStatus.WithName("status1");

            _unitOfWork.Setup(u => u.OrderStatuses.GetAsync(1))
                .ReturnsAsync(order);

            var resultFromController = await _orderStatusController.GetOrderStatus(1);
            var okResult = resultFromController as OkObjectResult;

            //Assert
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
            okResult.Value.As<OrderStatusDto>().Name.Should().Be("status1");
        }

        [Test]
        public async Task UpdateOrderStatus_ValidOrderIdAndValidOrderDtoStatus_ShouldReturnOkResult()
        {
            OrderStatusDto orderDto = An.OrderStatusDto.WithName("status1").WithId(1);
            OrderStatus order = An.OrderStatus.WithName("status2");

            _unitOfWork.Setup(u => u.OrderStatuses.GetAsync(1))
                .ReturnsAsync(order);
           
            var resultFromController = await _orderStatusController.UpdateOrderStatus(1, orderDto);
            var okResult = resultFromController as OkResult;

            //Assert
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Test]
        public async Task PostOrderStatus_ValidOrderDto_ShouldReturnOkResult()
        {
            OrderStatusDto orderDto = An.OrderStatusDto.WithName("status1").WithId(1);

            var resultFromController = await _orderStatusController.PostOrderStatus(orderDto);
            var okResult = resultFromController as OkResult;

            //Assert
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Test]
        public async Task DeleteOrderStatus_ValidOrderId_ShouldReturnOkResult()
        {
            OrderStatus order= An.OrderStatus.WithName("status1").WithId(1);
            _unitOfWork.Setup(o => o.OrderStatuses.GetAsync(1)).ReturnsAsync(order);

            var resultFromController = await _orderStatusController.DeleteOrderStatus(1);
            var okResult = resultFromController as OkResult;

            //Assert
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

    }
}
