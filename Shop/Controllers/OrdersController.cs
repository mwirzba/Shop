﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    /// Controller responsible for orders management.
    /// </summary>
    [ApiController]
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

        /// <summary>
        /// Retrives all ordes
        /// </summary>
        /// <response code="200">Order rerurned</response>
        /// <response code="400">Exception occurred</response>
        /// <response code="404">Orders in database not found</response>
        /// <response code="403">User is unauthorized.</response>
        [HttpGet]
        [Authorize(Roles = Roles.Admin + "," + Roles.OrdersManager)]
        public async Task<IActionResult> GetOrdersAsync()
        {
            try
            {
                var ordersInDb = await _unitOfWork.Orders.GetFullOrders();
                if(ordersInDb == null)
                {
                    return NotFound();
                }
                var ordersDto = _mapper.Map<IEnumerable<Order>, IEnumerable<OrderDto>>(ordersInDb);
                return Ok(ordersDto);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        /// <summary>
        /// Retrives order by unique id.
        /// </summary>
        /// <param name="id">Order id</param>
        /// <response code="200">Order returned</response>
        /// <response code="400">Exception occurred</response>
        /// <response code="404">Order with given id not found</response>
        /// <response code="403">User is unauthorized.</response>
        [HttpGet("{id}")]
        [Authorize(Roles = Roles.Admin + "," + Roles.OrdersManager)]
        public async Task<IActionResult> GetOrderAsync(long id)
        {
            try
            {
                var orderInDb = await _unitOfWork.Orders.GetFullOrder(id);
                if (orderInDb ==  null)
                {
                    return NotFound();
                }
                var orderDto = _mapper.Map<Order, OrderDto>(orderInDb);
                return Ok(orderDto);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        /// <summary>
        /// Creates new order.
        /// </summary>
        /// <param name="order">New Order data</param>
        /// <response code="200">Order created</response>
        /// <response code="422">Missing order paramether</response>
        /// <response code="400">Exception during database update happened or another exception</response>
        /// <response code="403">User is unauthorized.</response>
        [HttpPost]
        public async Task<IActionResult> PostOrderAsync([FromBody]OrderRequest order)
        {
            
            if(order == null)
            {
                return UnprocessableEntity();
            }

            var orderToSave = _mapper.Map<OrderRequest, Order>(order);
            orderToSave.CartLines = (ICollection<CartLine>)_mapper.Map<IEnumerable<CartLineRequest>, IEnumerable<CartLine>>(order.CartLines);
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


        /// <summary>
        /// Updates order
        /// </summary>
        /// <param name="order">Updated order new data</param>
        /// <param name="id">Order id</param>
        /// <response code="200">Order has been updated</response>
        /// <response code="422">Missing order paramether</response>
        /// <response code="400">Exception during database update happened or another exception</response>
        /// <response code="404">Order with given id not found</response>
        /// <response code="403">User is unauthorized.</response>
        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Admin + "," + Roles.OrdersManager)]
        public async Task<IActionResult> PutOrderInformationsAsync(long id,[FromBody]OrderRequest order)
        {
            if (order == null)
            {
                return UnprocessableEntity();
            }

            var orderInDb = await _unitOfWork.Orders.GetOrderWithLines(id);
            //var linesInDb = orderInDb.CartLines;
            if (orderInDb == null)
            {
                return NotFound();
            }

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


        /// <summary>
        /// Updates order status.
        /// </summary>
        /// <param name="id">Order id</param>
        /// <param name="orderStatusId">Order status id</param>
        /// <response code="200">Order status has been updated.</response>
        /// <response code="400">Exception during database update happened or another exception</response>
        /// <response code="404">Order or orderStatus with given id not found.</response>
        /// <response code="403">User is unauthorized.</response>
        [HttpPut("status/{id}")]
        [Authorize(Roles = Roles.Admin + "," + Roles.OrdersManager)]
        public async Task<IActionResult> UpdateOrderStatus(long id,[FromQuery]int orderStatusId)
        {
            var orderInDb = await _unitOfWork.Orders.GetAsync(id);
            if(orderInDb == null)
            {
                return NotFound("Wrong order id.");
            }

            var orderStatus = await _unitOfWork.OrderStatuses.GetAsync(orderStatusId);

            if (orderStatus == null)
            {
                return NotFound("Wrong order status id.");
            }

            if (orderInDb.StatusId != orderStatusId)
            {
                orderInDb.StatusId = orderStatusId;
                await _unitOfWork.CompleteAsync();
            }

            return Ok();
        }
    }
}
