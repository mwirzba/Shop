using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Shop.Dtos;
using Shop.Models;
using Shop.Services;
using System.Threading.Tasks;

namespace Shop.Controllers
{
    /// <summary>
    /// Authorization controller responsible for register new user,login;
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly ITokenGenerator _tokenGenerator;

        public AuthController(IConfiguration config, UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper, ITokenGenerator tokenGenarator)
        {
            _config = config;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _tokenGenerator = tokenGenarator;
        }

        /// <summary>
        /// Allows user to create account
        /// </summary>
        /// <param name="userDto">The user account dto which contains user login and password</param>
        /// <response code="200">Account created</response>
        /// <response code="409">User name is already used. Account has not been created.</response>
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            var userExists = await _userManager.FindByNameAsync(userDto.UserName);
            if (userExists != null)
            {
                return StatusCode(409, $"User {userDto.UserName} already exists.");
            }

            var userToCreate = _mapper.Map<User>(userDto);

            var result = await _userManager.CreateAsync(userToCreate, userDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok();
        }

        /// <summary>
        /// Allows user to login
        /// </summary>
        /// <param name="userDto">The user account dto which contains user login and password</param>
        /// <response code="200">
        /// User has been logged. Token is returned which has to be passed in actions that require to be logged.
        /// </response>
        /// <response code="401">Invalid user name or password</response>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserDto userDto)
        {
            var userInDb = await _userManager.FindByNameAsync(userDto.UserName);

            if (userInDb ==  null)
            {
                return NotFound();
            }

            var result = await _signInManager.CheckPasswordSignInAsync(userInDb, userDto.Password, false);

            if (!result.Succeeded)
                return Unauthorized();

            return Ok(new
            {
                token = _tokenGenerator.GenerateToken(userInDb, _config),
            });
        }


    }
}
