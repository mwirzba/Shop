﻿using AutoMapper;
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
using System.Threading.Tasks;

namespace Shop.Tests
{
    [TestFixture]
    class UserControllerTests
    {
        private Mock<FakeUserManager> _userManagerMock;
        private UserController _userController;
        private Mock<HttpContext> _httpContext;
        private Mock<IUnitOfWork> _unitOfWork;
        public UserControllerTests()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _userManagerMock = new Mock<FakeUserManager>();
            _userController = new UserController(_userManagerMock.Object, new Mapper(MapperHelpers.GetMapperConfiguration()), _unitOfWork.Object);
            _httpContext = new Mock<HttpContext>(); 
        }

        [Test]
        public async Task GetUserInformations_AuthorizedUser_ShouldReturnOkResultWithUserData()
        {
            User userInDb = A.User.WithId("123")
                                  .WithUserName("user1");

            _httpContext.Setup(d => d.User.Identity.IsAuthenticated).Returns(true);
            _userController.ControllerContext = new ControllerContext
            {
                HttpContext = _httpContext.Object
            };

            _userManagerMock.Setup(um => um.GetUserAsync(_userController.User)).ReturnsAsync(userInDb);

            var result = await _userController.GetUserInformations();
            var okResult = result as OkObjectResult;

            //Assert
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
            okResult.Value.As<UserDto>().UserName.Should().Be(userInDb.UserName);
        }

        [Test]
        public async Task GetUserInformations_UnAuthorizedUser_ShouldReturnUnauthorized()
        {   
            _httpContext.Setup(d => d.User.Identity.IsAuthenticated).Returns(false);
            _userController.ControllerContext = new ControllerContext
            {
                HttpContext = _httpContext.Object
            };

            var result = await _userController.GetUserInformations();
            var unauthorizedResult = result as UnauthorizedResult;

            //Assert
            unauthorizedResult.Should().NotBeNull();
            unauthorizedResult.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }

        [Test]
        public async Task PatchUserInformations_LggedUserAndCorrectDataWithNotUsedUserName_ShouldReturnOkResult()
        {
            User userInDb = A.User.WithId("123")
                                  .WithUserName("user1");

            var userDto = new UserDto
            {
                UserName = "user1"
            };

            _httpContext.Setup(d => d.User.Identity.IsAuthenticated).Returns(true);
            _userController.ControllerContext = new ControllerContext
            {
                HttpContext = _httpContext.Object
            };

            _userManagerMock.Setup(um => um.GetUserAsync(_userController.User)).ReturnsAsync(userInDb);
            _userManagerMock.Setup(um => um.FindByNameAsync(userInDb.UserName)).ReturnsAsync((User)null);


            var result = await _userController.PatchUserInformations(userDto);
            var okResult = result as OkResult;

            //Assert
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        }


        [Test]
        public async Task PatchUserInformations_AuthorizedUserAndCorrectDataWithUsedUserName_ShouldReturnConflictResultWithMessage()
        {
            User userInDb = A.User.WithId("123")
                                  .WithUserName("user1");

            User userInDb2 = A.User.WithId("1233")
                                  .WithUserName("user2");

            UserDto userDto = new UserDto
            {
                UserName = "user3"
            };

            _httpContext.Setup(d => d.User.Identity.IsAuthenticated).Returns(true);
            _userController.ControllerContext = new ControllerContext
            {
                HttpContext = _httpContext.Object
            };

            _userManagerMock.Setup(um => um.GetUserAsync(_userController.User)).ReturnsAsync(userInDb);
            _userManagerMock.Setup(um => um.FindByNameAsync(userDto.UserName)).ReturnsAsync(userInDb2);


            var result = await _userController.PatchUserInformations(userDto);
            var conflictResult = result as ConflictObjectResult;


            //Assert
            conflictResult.Should().NotBeNull();
            conflictResult.StatusCode.Should().Be(StatusCodes.Status409Conflict);
        }

        [Test]
        public async Task PatchUserInformations_UserNotLogged_ShouldReturnUnAuthorized()
        {
            var userDto = new UserDto
            {
                UserName = "user3"
            };

            _httpContext.Setup(d => d.User.Identity.IsAuthenticated).Returns(false);
            _userController.ControllerContext = new ControllerContext
            {
                HttpContext = _httpContext.Object
            };

            var result = await _userController.PatchUserInformations(userDto);
            var unauthorizedResult = result as UnauthorizedResult;

            //Assert
            unauthorizedResult.Should().NotBeNull();
            unauthorizedResult.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }



    }
}
