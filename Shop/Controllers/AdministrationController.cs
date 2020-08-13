using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop.Data;
using Shop.Data.Repositories;
using Shop.Dtos;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shop.Controllers
{
    /// <summary>
    /// Controller responsible for user management
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = Roles.Admin + "," + Roles.UserManager)]
    public class AdministrationController: ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public AdministrationController(UserManager<User> userManager, IMapper mapper, IUnitOfWork unitOfWork,RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Returns list of users with given role id.
        /// </summary>
        /// <response code="200">Returned users</response>
        /// <response code="404">Role with given id not found in db.</response>
        /// <response code="403">User is unauthorized.</response>
        /// <response code="400">Exception during code execution</response>
        [HttpGet("usersInRole/{roleId}")]
        public async Task<IActionResult> GetUsersInRole(string roleId)
        {
            try
            {

                if (!User.Identity.IsAuthenticated)
                {
                    return Unauthorized();
                }

                var role = await _roleManager.FindByIdAsync(roleId);

                if (role == null)
                {
                    return NotFound();
                }

                var users = new List<UserDto>();

                foreach (var user in _userManager.Users)
                {
                    if (await _userManager.IsInRoleAsync(user, role.Name))
                    {
                        var userDto = new UserDto
                        {
                            UserName = user.UserName
                        };

                        users.Add(userDto);
                    }
                }

                return Ok(users);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Returns all users.
        /// </summary>
        /// <response code="200">Returned users</response>
        /// <response code="404">Users not found in database.</response>
        /// <response code="403">User is unauthorized.</response>
        /// <response code="400">Exception during code execution</response>
        [HttpGet]
        public IActionResult GetUsers()
        {
            try
            {
                var users = _userManager.Users;

                if(users == null)
                {
                    return NotFound();
                }

                var usersDto = _mapper.Map<IEnumerable<User>, IEnumerable<UserDto>>(users); 

                return Ok(usersDto);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Returns user with given username.
        /// </summary>
        /// <response code="200">Returned user</response>
        /// <response code="404">User not found in database.</response>
        /// <response code="403">User is unauthorized.</response>
        /// <response code="400">Exception during code execution</response>
        [HttpGet("{username}")]
        public async Task<IActionResult> GetUserByUserName(string username)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(username);

                if(user == null)
                {
                    return NotFound();
                }

                var userDto = _mapper.Map<User,UserDto>(user);

                return Ok(userDto);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Adds role to user.
        /// </summary>
        /// <param name="roleId">Role id</param>
        /// <param name="userName">User name of the user to whom we want to assign role.</param>
        /// <response code="200">Added role to user.</response>
        /// <response code="404">User not found in database or role not found.</response>
        /// <response code="403">User is unauthorized to add roles to users.</response>
        /// <response code="400">Exception during code execution</response>
        [HttpPatch("addToRole")]
        public async Task<IActionResult> AddUserToRole(string roleId,string userName)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(roleId);

                if (role == null)
                {
                    return NotFound("Can not find role with given id.");
                }

                var userInDb = await _userManager.FindByNameAsync(userName);

                if (userInDb == null)
                {
                    return NotFound("Can not find user with given username.");
                }


                bool isInRole = await _userManager.IsInRoleAsync(userInDb, role.Name);
                if (!isInRole)
                {
                    await _userManager.AddToRoleAsync(userInDb, role.Name);

                    await _unitOfWork.CompleteAsync();
                }

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Removes role from user.
        /// </summary>
        /// <param name="roleId">Role id</param>
        /// <param name="userName">User name of the user to whom we want to remove role.</param>
        /// <response code="200">Removed role from user.</response>
        /// <response code="404">User not found in database or role not found.</response>
        /// <response code="403">User is unauthorized to add roles to users.</response>
        /// <response code="400">Exception during code execution</response>
        [HttpPatch("removeFromRole")]
        public async Task<IActionResult> RemoveUserFromRole(string roleId, string userName)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(roleId);

                if (role == null)
                {
                    return NotFound("Can not find role with given id.");
                }

                var userInDb = await _userManager.FindByNameAsync(userName);

                if (userInDb == null)
                {
                    return NotFound("Can not find user with given username.");
                }

                bool isInRole = await _userManager.IsInRoleAsync(userInDb, role.Name);
                if (isInRole)
                {
                    await _userManager.RemoveFromRoleAsync(userInDb, role.Name);
                }

                await _unitOfWork.CompleteAsync();

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
