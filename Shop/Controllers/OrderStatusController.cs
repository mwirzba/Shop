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

        [HttpGet]
        public async Task<IActionResult> GetOrderStatuses()
        {
            var orderStatuses =  await _unitOfWork.OrderStatuses.GetAllAsync();
            var orderStatusesDto = _mapper.Map<IEnumerable<OrderStatusDto>>(orderStatuses);
            return Ok(orderStatusesDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderStatus(int id)
        {
            var orderStatus = await _unitOfWork.OrderStatuses.GetAsync(id);

            var orderStatusDto = _mapper.Map<OrderStatusDto>(orderStatus);

            if (orderStatus == null)
            {
                return NotFound();
            }

            return Ok(orderStatusDto);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderStatus(int id, OrderStatusDto orderStatusDto)
        {
            if (id != orderStatusDto.Id)
            {
                return BadRequest();
            }

            var orderStatusInDb = await _unitOfWork.OrderStatuses.GetAsync(orderStatusDto.Id);
            _mapper.Map(orderStatusDto, orderStatusInDb);

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

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> PostOrderStatus(OrderStatusDto orderStatus)
        {
            var orderStatusToSave = _mapper.Map<OrderStatusDto, OrderStatus>(orderStatus);
            await _unitOfWork.CompleteAsync();

            return CreatedAtAction("GetOrderStatus", new { id = orderStatus.Id }, orderStatus);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderStatus(int id)
        {
            var orderStatus = await _unitOfWork.OrderStatuses.GetAsync(id);
            if (orderStatus == null)
            {
                return NotFound();
            }

            _unitOfWork.OrderStatuses.Remove(orderStatus);
            await _unitOfWork.CompleteAsync();

            return Ok(_mapper.Map<OrderStatusDto>(orderStatus));
        }

    }
}
