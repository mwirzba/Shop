using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data.Repositories;
using Shop.Dtos;
using Shop.Models;

namespace Shop.Controllers
{
    /// <summary>
    /// Controller responsible for order status CRUD actions.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class OrderStatusController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public OrderStatusController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        /// <summary>
        /// Retrives all orders statuses
        /// </summary>
        /// <response code="200">Returned order list</response>
        /// <response code="404">Not Found order status</response>
        [HttpGet]
        public async Task<IActionResult> GetOrderStatus()
        {
            var orderStatuses =  await _unitOfWork.OrderStatuses.GetAllAsync();
            if (orderStatuses == null)
                return NotFound();

            var orderStatusesDto = _mapper.Map<IEnumerable<OrderStatusDto>>(orderStatuses);
            return Ok(orderStatusesDto);
        }
        /// <summary>
        /// Retrives order status by unique id
        /// </summary>
        /// <param name="id">Order status id</param>
        /// <response code="200">Found order status</response>
        /// <response code="404">Not Found order status</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderStatus([FromQuery]int id)
        {
            var orderStatus = await _unitOfWork.OrderStatuses.GetAsync(id);

            var orderStatusDto = _mapper.Map<OrderStatusDto>(orderStatus);

            if (orderStatus == null)
            {
                return NotFound();
            }

            return Ok(orderStatusDto);
        }


        /// <summary>
        /// Updates order status by unique id
        /// </summary>
        /// <param name="id">Order status id</param>
        /// <param name="orderStatus">Order status new data.</param>
        /// <response code="200">Order status updated</response>
        /// <response code="422">Missing orderstatus paramether.</response>
        /// <response code="400">Wrong id of updated order is not equal to given id.</response>
        /// <response code="404">Not Found order status</response>
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateOrderStatus([FromQuery]int id,[FromBody]OrderStatusDto orderStatus)
        {

            if (orderStatus == null)
            {
                return UnprocessableEntity(orderStatus);
            }

            if (id != orderStatus.Id)
            {
                return UnprocessableEntity();
            }

            var orderStatusInDb = await _unitOfWork.OrderStatuses.GetAsync(orderStatus.Id);
            _mapper.Map(orderStatus, orderStatusInDb);

            try
            {
                await _unitOfWork.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                bool exists = await _unitOfWork.OrderStatuses.ExistsAsync(o => o.Id == id);
                if (!exists)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }


        /// <summary>
        /// Creates order status
        /// </summary>
        /// <param name="orderStatus">Order status data.</param>
        /// <response code="200">Created order status</response>
        /// <response code="422">Missing orderstatus paramether.</response>
        /// <response code="400">Exception during database update.</response>
        [HttpPost]
        public async Task<IActionResult> PostOrderStatus(OrderStatusDto orderStatus)
        {
            if(orderStatus ==  null)
            {
                return UnprocessableEntity(orderStatus);
            }

            var orderStatusToSave = _mapper.Map<OrderStatusDto, OrderStatus>(orderStatus);
            
            try
            {
                await _unitOfWork.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return Ok();
        }

        /// <summary>
        /// Deletes status with unique id
        /// </summary>
        /// <param name="id">Order status id.</param>
        /// <response code="200">Deleted order status</response>
        /// <response code="404">Order status not found.</response>
        /// <response code="400">Exception during database update.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderStatus(int id)
        {
            var orderStatus = await _unitOfWork.OrderStatuses.GetAsync(id);
            if (orderStatus == null)
            {
                return NotFound();
            }

            _unitOfWork.OrderStatuses.Remove(orderStatus);
            try
            {
                await _unitOfWork.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }
            return Ok(_mapper.Map<OrderStatusDto>(orderStatus));
        }

    }
}
