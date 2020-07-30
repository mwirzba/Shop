using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop.Dtos;
using Shop.Models;

namespace Shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UserController (UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserInformations()
        {
            if(!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            var userFromDb = await _userManager.GetUserAsync(User);
            var mappedUser = _mapper.Map<User, UserDto>(userFromDb);
            return Ok(mappedUser);
        }

        [HttpPatch]
        public async Task<IActionResult> PatchUserInformations(UserDto userDto)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser.UserName != userDto.UserName)
            {
                var userWithChoosenUserName = await _userManager.FindByNameAsync(userDto.UserName);

                if (userWithChoosenUserName != null)
                {
                    return StatusCode(409, $"User '{userDto.UserName}' already exists.");
                }
            }

            _mapper.Map(userDto, currentUser);

            await _userManager.UpdateAsync(currentUser);

            return Ok();
        }

    }
}
