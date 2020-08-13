using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop.Data.Repositories;
using Shop.Dtos;
using Shop.Models;

namespace Shop.Controllers
{
    /// <summary>
    /// Controller responsible for management user account
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UserController(UserManager<User> userManager, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Returns user account informations
        /// </summary>
        /// <response code="200">Returned user informations</response>
        /// <response code="401">User is not logged</response>
        /// <response code="400">Exception during operation.</response>
        /// <response code="403">User is unauthorized.</response>
        [HttpGet]
        public async Task<IActionResult> GetUserInformations()
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return Unauthorized();
                }

                var userFromDb = await _userManager.GetUserAsync(User);
                var mappedUser = _mapper.Map<User, UserDto>(userFromDb);
                return Ok(mappedUser);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
           
        }

        /// <summary>
        /// Updates user account informations
        /// </summary>
        /// <param name="userDto">User data</param>
        /// <response code="200">Returned user informations</response>
        /// <response code="401">User is not logged</response>     
        /// <response code="400">Exception during operation.</response>
        /// <response code="403">User is unauthorized.</response>
        [HttpPatch("orders")]
        public async Task<IActionResult> PatchUserInformations([FromBody] UserDto userDto)
        {
            try
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
                        return Conflict($"User '{userDto.UserName}' already exists.");
                    }
                }

                _mapper.Map(userDto, currentUser);

                await _userManager.UpdateAsync(currentUser);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Returns user orders list
        /// </summary>
        /// <response code="200">Returned user informations</response>
        /// <response code="401">User is not logged</response>
        /// <response code="404">User not found in database</response>
        /// <response code="400">Exception during operation.</response>
        /// <response code="403">User is unauthorized.</response>
        [HttpGet("orders")]
        public async Task<IActionResult> GetUserOrders()
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return Unauthorized();
                }
                var userFromDb = await _userManager.GetUserAsync(User);

                if (userFromDb == null)
                {
                    return NotFound();
                }

                var orders = await _unitOfWork.Orders.GetUserOrders(userFromDb.Id);

                var ordersDto = _mapper.Map<IEnumerable<Order>,IEnumerable<OrderDto>>(orders);

                return Ok(ordersDto);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
