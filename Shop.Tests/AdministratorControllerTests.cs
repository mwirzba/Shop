using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Tests
{
    [TestFixture]
    class AdministratorControllerTests
    {
        private readonly Mock<FakeUserManager> _userManagerMock;
        private readonly Mock<FakeRoleManager> _roleManager;
        private readonly IMapper _mapper;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly AdministrationController _administrationController;
        private Mock<HttpContext> _httpContext;
        public AdministratorControllerTests()
        {
            _httpContext = new Mock<HttpContext>();
            _userManagerMock = new Mock<FakeUserManager>();
            _roleManager = new Mock<FakeRoleManager>();
            _mapper = new Mapper(MapperHelpers.GetMapperConfiguration());
            _unitOfWork = new Mock<IUnitOfWork>();      

            _administrationController = new AdministrationController(
                _userManagerMock.Object,
                _mapper,
                _unitOfWork.Object,
                _roleManager.Object
             );
        }

        [Test]
        public async Task GetUsersInRole_ValidRoleIdUserAuthorized_ReturnsOkResultWithUsers()
        {
            //setup
            _roleManager.Setup(r => r.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(
                new IdentityRole
                {
                    Name = "Admin"
                });

            var usersMock = new List<User>
                {
                    A.User.WithUserName("user1"),
                    A.User.WithUserName("user2")
                };

            _userManagerMock.Setup(u => u.Users)
                .Returns(usersMock.AsQueryable());

            _userManagerMock.Setup
                (r => r.IsInRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            _httpContext.Setup(d => d.User.Identity.IsAuthenticated).Returns(true);
            _administrationController.ControllerContext = new ControllerContext
            {
                HttpContext = _httpContext.Object
            };

            var resultFromController = await _administrationController.GetUsersInRole("string");
            var okObjectResult = resultFromController as OkObjectResult;

            //Assert
            okObjectResult.Should().NotBeNull();
            okObjectResult.Value.As<IEnumerable<UserDto>>().ToList().Should().HaveCount(2);
            okObjectResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Test]
        public async Task GetUsersInRole_InValidRoleIdUserAuthorized_ReturnsNotFoundResult()
        {
            //setup
            _roleManager.Setup(r => r.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((IdentityRole)null);

            var usersMock = new List<User>
                {
                    A.User.WithUserName("user1"),
                    A.User.WithUserName("user2")
                };

            _httpContext.Setup(d => d.User.Identity.IsAuthenticated).Returns(true);
            _administrationController.ControllerContext = new ControllerContext
            {
                HttpContext = _httpContext.Object
            };

            var resultFromController = await _administrationController.GetUsersInRole("string");
            var notFoundResult = resultFromController as NotFoundResult;

            //Assert
            notFoundResult.Should().NotBeNull();
            notFoundResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Test]
        public async Task GetUsersInRole_UserUnAuthorized_ReturnsUnAuthorizedResult()
        {
            //setup
            _roleManager.Setup(r => r.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((IdentityRole)null);

            _httpContext.Setup(d => d.User.Identity.IsAuthenticated).Returns(false);
            _administrationController.ControllerContext = new ControllerContext
            {
                HttpContext = _httpContext.Object
            };

            var resultFromController = await _administrationController.GetUsersInRole("string");
            var unauthorizedResult = resultFromController as UnauthorizedResult;

            //Assert
            unauthorizedResult.Should().NotBeNull();
            unauthorizedResult.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }

        [Test]
        public void GetUsers_UserAuthorized_ReturnsOkResultWithUsers()
        {
            //setup
            var usersMock = new List<User>
                {
                    A.User.WithUserName("user1"),
                    A.User.WithUserName("user2")
                };

            _userManagerMock.Setup(u => u.Users)
                .Returns(usersMock.AsQueryable());

            _userManagerMock.Setup
                (r => r.IsInRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            _httpContext.Setup(d => d.User.Identity.IsAuthenticated).Returns(true);
            _administrationController.ControllerContext = new ControllerContext
            {
                HttpContext = _httpContext.Object
            };

            var resultFromController =  _administrationController.GetUsers();
            var okObjectResult = resultFromController as OkObjectResult;

            //Assert
            okObjectResult.Should().NotBeNull();
            okObjectResult.Value.As<IEnumerable<UserDto>>().ToList().Should().HaveCount(2);
            okObjectResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Test]
        public async Task GetUserByUserName_UserAuthorizedAndValidUserName_ReturnsOkResultWithUser()
        {
            //setup
            User userInDb = A.User.WithUserName("user1");

            _userManagerMock.Setup(u => u.FindByNameAsync("user1"))
                .ReturnsAsync(userInDb);

            _userManagerMock.Setup
                (r => r.IsInRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            _httpContext.Setup(d => d.User.Identity.IsAuthenticated).Returns(true);
            _administrationController.ControllerContext = new ControllerContext
            {
                HttpContext = _httpContext.Object
            };

            var resultFromController = await _administrationController.GetUserByUserName("user1");
            var okObjectResult = resultFromController as OkObjectResult;

            //Assert
            okObjectResult.Should().NotBeNull();
            okObjectResult.Value.As<UserDto>().Should().NotBeNull();
            okObjectResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Test]
        public async Task GetUserByUserName_UserAuthorizedAndInValidUserName_ReturnsNotFoundResult()
        {
            //setup
            _userManagerMock.Setup(u => u.FindByNameAsync("user1"))
                .ReturnsAsync((User)null);

            _httpContext.Setup(d => d.User.Identity.IsAuthenticated).Returns(true);
            _administrationController.ControllerContext = new ControllerContext
            {
                HttpContext = _httpContext.Object
            };

            var resultFromController = await _administrationController.GetUserByUserName("user1");
            var notFoundResult = resultFromController as NotFoundResult;

            //Assert
            notFoundResult.Should().NotBeNull();
            notFoundResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Test]
        public async Task AddUserToRole_UserAuthorizedAndValidUserNameAndValidRoleId_ReturnsOKResult()
        {
            //setup
            _roleManager.Setup(r => r.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(
                new IdentityRole
                {
                    Name = "Admin"
                });

            User userInDb = A.User.WithUserName("user1");
            _userManagerMock.Setup(u => u.FindByNameAsync("user1"))
                .ReturnsAsync(userInDb);

            _userManagerMock.Setup
                (r => r.IsInRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            _userManagerMock.Setup(um => um.AddToRoleAsync(userInDb, "user1"));

            _httpContext.Setup(d => d.User.Identity.IsAuthenticated).Returns(true);
            _administrationController.ControllerContext = new ControllerContext
            {
                HttpContext = _httpContext.Object
            };

            var resultFromController = await _administrationController.AddUserToRole("string","user1");
            var okResult = resultFromController as OkResult;

            //Assert
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Test]
        public async Task AddUserToRole_UserAuthorizedAndInValid_ReturnsOKResult()
        {
            //setup
            _roleManager.Setup(r => r.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(
                new IdentityRole
                {
                    Name = "Admin"
                });

            User userInDb = A.User.WithUserName("user1");
            _userManagerMock.Setup(u => u.FindByNameAsync("user1"))
                .ReturnsAsync(userInDb);

            _userManagerMock.Setup
                (r => r.IsInRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            _userManagerMock.Setup(um => um.AddToRoleAsync(userInDb, "user1"));

            _httpContext.Setup(d => d.User.Identity.IsAuthenticated).Returns(true);
            _administrationController.ControllerContext = new ControllerContext
            {
                HttpContext = _httpContext.Object
            };

            var resultFromController = await _administrationController.AddUserToRole("string", "user1");
            var okResult = resultFromController as OkResult;

            //Assert
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        }
    }
}
