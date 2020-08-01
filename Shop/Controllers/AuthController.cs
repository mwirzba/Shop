using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Shop.Dtos;
using Shop.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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

        public AuthController(IConfiguration config,UserManager<User> userManager,SignInManager<User> signInManager,IMapper mapper)
        {   
            _config = config;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
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
            var userExists = _userManager.FindByNameAsync(userDto.UserName);
            if(userExists!=null)
            {
                return StatusCode(409, $"User '{userDto.UserName}' already exists.");
            }

            var userToCreate = _mapper.Map<User>(userDto);

            var result = await _userManager.CreateAsync(userToCreate, userDto.Password);

            if(!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok();
        }


        /// <summary>
        /// Allows user to login
        /// </summary>
        /// <param name="userDto">The user account dto which contains user login and password</param>
        /// <response code="200">User has been logged</response>
        /// <response code="401">Invalid user name or password</response>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserDto userDto)
        {
            var userInDb = await _userManager.FindByNameAsync(userDto.UserName);

            var result = await _signInManager.CheckPasswordSignInAsync(userInDb, userDto.Password, false);

            if (!result.Succeeded)
                return Unauthorized();

            return Ok(new
            {
                token = GenerateToken(userInDb), 
            });   
        }

        private string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = cred
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
