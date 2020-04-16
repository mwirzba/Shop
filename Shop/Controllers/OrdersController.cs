using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data.Repositories;

using Shop.Dtos;
using Shop.Models;
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
        public OrdersController(IUnitOfWork unitOfWork, IMapper mapper,
            UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
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
            var orderToSave = _mapper.Map<OrderDto, Order>(order);
            orderToSave.CartLines = (ICollection<CartLine>)_mapper.Map<IEnumerable<CartLineDto>,IEnumerable<CartLine>>(order.CartLines);
            await _unitOfWork.Orders.AddAsync(orderToSave);
            if (User.Identity.IsAuthenticated)
            {
                orderToSave.UserId = _userManager.GetUserId(User);
            }
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
