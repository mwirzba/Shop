using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data.Repositories;
using Shop.Data.Repositories.Interfaces;
using Shop.Dtos;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public OrdersController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrdersAsync()
        {
            var ordersInDb = await _unitOfWork.Orders.GetOrdersWithLines();
            var ordersDto = _mapper.Map<IEnumerable<Order>, IEnumerable<OrderDto>>(ordersInDb);
            return Ok(ordersDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderAsync(long id)
        {
            var orderInDb = await _unitOfWork.Orders.GetOrderWithLines(id);
            var orderDto = _mapper.Map<Order,OrderDto>(orderInDb);
            return Ok(orderDto);
        }

        [HttpPost]
        public async Task<IActionResult> PostOrderAsync(OrderDto order)
        {           
            if (!order.CartLines.Any())
            {
                return BadRequest();
            }
            var orderToSave = _mapper.Map<OrderDto, Order>(order);
            _mapper.Map(order.CartLines, orderToSave.CartLines);
            await _unitOfWork.Orders.AddAsync(orderToSave);
            try
            {
                await _unitOfWork.CompleteAsync();
            }
            catch (DbUpdateException e)
            {
                return BadRequest(e.Message);
            }
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderAsync(long id,OrderDto order)
        {
            if (!order.CartLines.Any())
            {
                return BadRequest();
            }
            var orderInDb = await _unitOfWork.Orders.GetOrderWithLines(id);
            var linesInDb = orderInDb.CartLines;

            //if(linesInDb.)


            _mapper.Map(order, orderInDb);

            try
            {
                await _unitOfWork.CompleteAsync();
            }
            catch (DbUpdateException e)
            {
                return BadRequest(e.InnerException);
            }
            return Ok();
        }

        private void RemoveCartLineFromOrder()
        {

        }

        private void AddCartLineFromOrder()
        {

        }


    }
}
