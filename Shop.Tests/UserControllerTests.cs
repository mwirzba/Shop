using AutoMapper;
using AutoMapper.Configuration;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Shop.Controllers;
using Shop.Data;
using Shop.Dtos;
using Shop.Models;
using Shop.Tests.MockClasses;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Shop.Tests
{
    [TestFixture]
    class UserControllerTests
    {
        private Mock<FakeUserManager> _userManagerMock;
        private UserController _userController;
        private Mock<HttpContext> _httpContext;
        public UserControllerTests()
        {
            _userManagerMock = new Mock<FakeUserManager>();
            _userController = new UserController(_userManagerMock.Object, new Mapper(CreateConfiguration()));
            _httpContext = new Mock<HttpContext>(); 
        }

        private MapperConfiguration CreateConfiguration()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });

            return config;
        }

        [Test]
        public async Task GetUserInformations_AuthorizedUser_ShouldReturnOkResultWithUserData()
        {
            var userInDb = new User
            {
                Id = "hjgsfdghfd",
                UserName = "user1"
            };

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
            var userInDb = new User
            {
                Id = "hjgsfdghfd",
                UserName = "user1"
            };

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
            var userInDb = new User
            {
                Id = "hjgsfdghfd",
                UserName = "user1"
            };

            var userInDb2 = new User
            {
                Id = "hjgsfdghfdaqd",
                UserName = "user2"
            };

            var userDto = new UserDto
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
