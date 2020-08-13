using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using Shop.Controllers;
using Shop.Models;
using Shop.Tests.MockClasses;
using System.Threading.Tasks;
using Shop.Dtos;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Shop.Services;

namespace Shop.Tests
{
    [TestFixture]
    class AuthControllerTests
    {
        private Mock<FakeUserManager> _userManagerMock;
        private Mock<FakeSignInManager> _signInManagerMock;
        private AuthController _authController;
        private Mock<ITokenGenerator> _tokenGenetaror;
        private Mock<HttpContext> _httpContext;

        public AuthControllerTests()
        {
            _userManagerMock = new Mock<FakeUserManager>();
            _signInManagerMock = new Mock<FakeSignInManager>();
            _tokenGenetaror = new Mock<ITokenGenerator>();
            _httpContext = new Mock<HttpContext>();
            _tokenGenetaror.Setup(t => 
                t.GenerateTokenAsync(It.IsAny<User>(),
                It.IsAny<IConfiguration>(),
                _userManagerMock.Object
                )).ReturnsAsync("asduykasgdyukasfgasdkuy");

            _authController = new AuthController(
                new Mock<IConfiguration>().Object,
                _userManagerMock.Object, _signInManagerMock.Object, 
                new Mapper(MapperHelpers.GetMapperConfiguration()),
                _tokenGenetaror.Object
            );
        }

        [Test]
        public async Task Register_IfUserNameIsNotUsed_RegisterNewUser_AndThen_ShouldReturn_Ok()
        {
            
            _userManagerMock.Setup(m => m.FindByNameAsync(It.IsAny<string>())).ReturnsAsync((User)null);
            _userManagerMock.Setup(m => m.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            UserDto userDto = new UserDto
            {
                UserName = "user1"
            };

            var resultFromController =  await _authController.Register(userDto);
            var okResult = resultFromController as OkResult;


            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Test]
        public async Task Register_IfUserNameIsUsed_ShouldReturn_StatusCode409WithValidErrorMessage()
        {
            var newUser = new User
            {
                Id = "asdasd",
                UserName = "user1"
            };

            _userManagerMock.Setup(m => m.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(newUser);
            _userManagerMock.Setup(m => m.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed());
           
            UserDto userDto = new UserDto
            {
                UserName = "user1"
            };

            var resultFromController = await _authController.Register(userDto);
            var conflictResult = resultFromController as ObjectResult;
            string message = $"User user1 already exists.";
           
            //Assert
            conflictResult.StatusCode.Should().Be(StatusCodes.Status409Conflict);
            conflictResult.Value.Should().Be(message);
        }

        public static bool HasProperty(object obj, string name)
        {
            var property = obj.GetType().GetProperty("token");
            if (property == null)
                return false;
            return true;
        }

        [Test]
        public async Task Login_IfUserDataIsValid_ShouldReturn_OKResultWithGeneratedToken()
        {

            var newUser = new User
            {
                Id = "asdasd",
                UserName = "user1"
            };
            _userManagerMock.Setup(m => m.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(newUser);
            _signInManagerMock.Setup(m => m.CheckPasswordSignInAsync(It.IsAny<User>(), It.IsAny<string>(), false)).ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);
            

            UserDto userDto = new UserDto
            {
                UserName = "user1",
                Password = "123"
            };

            var resultFromController = await _authController.Login(userDto);
            var okResult = resultFromController as OkObjectResult;

            //Assert
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
            Assert.IsTrue(HasProperty(okResult.Value, "token"));
        }

        [Test]
        public async Task Login_IfUserWithUserNameNotExists_ShouldReturnNotFound()
        {
            _userManagerMock.Setup(m => m.FindByNameAsync(It.IsAny<string>())).ReturnsAsync((User)null);
            
            UserDto userDto = new UserDto
            {
                UserName = "user1",
                Password = "123"
            };

            var resultFromController = await _authController.Login(userDto);
            var notFoundResult = resultFromController as NotFoundResult;

            //Assert
            notFoundResult.Should().NotBeNull();
            notFoundResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Test]
        public async Task Login_PasswordInvalid_ShouldReturnUnathorized()
        {
            var newUser = new User
            {
                Id = "asdasd",
                UserName = "user1"
            };

            _userManagerMock.Setup(m => m.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(newUser);
            _signInManagerMock.Setup(m => m.CheckPasswordSignInAsync(It.IsAny<User>(), It.IsAny<string>(), false)).ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);


            UserDto userDto = new UserDto
            {
                UserName = "user1",
                Password = "123"
            };

            var resultFromController = await _authController.Login(userDto);
            var unathorizedResult = resultFromController as UnauthorizedResult;

            //Assert
            unathorizedResult.Should().NotBeNull();
            unathorizedResult.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }

        [Test]
        public async Task Logout_AuthorizedUser_ShouldCall_SignOutAsyncAndReturnNoContentResult()
        {
            _httpContext.Setup(d => d.User.Identity.IsAuthenticated).Returns(true);
            _authController.ControllerContext = new ControllerContext
            {
                HttpContext = _httpContext.Object
            };

            _signInManagerMock.Setup(s => s.SignOutAsync());
            
            var resultFromController = await _authController.Logout();
            var noContentResult = resultFromController as NoContentResult;

            //Assert
            _signInManagerMock.Verify(m => m.SignOutAsync());
            noContentResult.Should().NotBeNull();
            noContentResult.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }

    }
}
