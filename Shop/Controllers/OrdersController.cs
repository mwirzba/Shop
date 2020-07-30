using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data.Repositories;

using Shop.Dtos;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shop.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public OrdersController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrdersAsync()
        {
            try
            {
                var ordersInDb = await _unitOfWork.Orders.GetOrdersWithLines();
                var ordersDto = _mapper.Map<IEnumerable<Order>, IEnumerable<OrderDto>>(ordersInDb);
                return Ok(ordersDto);
            }
            catch(Exception)
            {
                return BadRequest("Something went wrong");
            }
        
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderAsync(long id)
        {
            try
            {
                var orderInDb = await _unitOfWork.Orders.GetFullOrder(id);
                var orderDto = _mapper.Map<Order, OrderDto>(orderInDb);
                return Ok(orderDto);
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong");
            }
        }



        [HttpPost]
        public async Task<IActionResult> PostOrderAsync(OrderDto order)
        {           
            var orderToSave = _mapper.Map<OrderDto, Order>(order);
            orderToSave.CartLines = (ICollection<CartLine>)_mapper.Map<IEnumerable<CartLineDto>,IEnumerable<CartLine>>(order.CartLines);
            await _unitOfWork.Orders.AddAsync(orderToSave);
            if (User.Identity.IsAuthenticated)
            {
                var loggedUser = await _userManager.GetUserAsync(User);
                orderToSave.UserId = loggedUser.Id;
                orderToSave.Email = loggedUser.Email;
            }
            try
            {
                await _unitOfWork.CompleteAsync();
            }
            catch (DbUpdateException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderInformationsAsync(long id,OrderDto order)
        {
            var orderInDb = await _unitOfWork.Orders.GetOrderWithLines(id);
            //var linesInDb = orderInDb.CartLines;

            _mapper.Map(order, orderInDb);

            try
            {
                await _unitOfWork.CompleteAsync();
            }
            catch (DbUpdateException e)
            {
                return BadRequest(e.InnerException);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok();
        }
    }
}
