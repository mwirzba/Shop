using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using Shop.Controllers;
using Shop.Models;
using Shop.Tests.MockClasses;
using System.Threading.Tasks;
using Shop.Data;
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
        private Mock<IConfiguration> _configuration;
        private Mock<ITokenGenerator> _tokenGenetaror;

        public AuthControllerTests()
        {
            _userManagerMock = new Mock<FakeUserManager>();
            _signInManagerMock = new Mock<FakeSignInManager>();
            _tokenGenetaror = new Mock<ITokenGenerator>();
            _configuration = new Mock<IConfiguration>();
            _tokenGenetaror.Setup(t => t.GenerateToken(It.IsAny<User>(),It.IsAny<IConfiguration>())).Returns("asduykasgdyukasfgasdkuy");
            _authController = new AuthController(new Mock<IConfiguration>().Object, _userManagerMock.Object, _signInManagerMock.Object, new Mapper(CreateConfiguration()), _tokenGenetaror.Object);
        }

        private MapperConfiguration CreateConfiguration()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });

            return config;
        }

        [OneTimeSetUp]
        public void Initialize()
        {
            
            //_userManagerMock.Setup(m => m.FindByNameAsync(It.IsAny<string>())).Returns(true)
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

            _userManagerMock.Setup(m => m.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(It.IsAny<User>());
            _userManagerMock.Setup(m => m.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed());
           

            UserDto userDto = new UserDto
            {
                UserName = "user1"
            };

            var resultFromController = await _authController.Register(userDto);
            var conflictResult = resultFromController as ObjectResult;
            string message = $"User user1 already exists.";
           
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
            var conflictResult = resultFromController as OkObjectResult;

            conflictResult.StatusCode.Should().Be(StatusCodes.Status200OK);
            Assert.IsTrue(HasProperty(conflictResult.Value, "token"));
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
            var conflictResult = resultFromController as OkObjectResult;

            conflictResult.StatusCode.Should().Be(StatusCodes.Status200OK);
            Assert.IsTrue(HasProperty(conflictResult.Value, "token"));
        }

    }
}
