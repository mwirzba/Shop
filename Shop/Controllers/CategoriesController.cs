using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Data.Repositories;
using Shop.Models;
using Shop.Models.ProductDtos;

namespace Shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CategoriesController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
        {
            var categoryInDb = await _unitOfWork.Categories.GetAllAsync();
            var categoryDto = _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryDto>>(categoryInDb);
            return Ok(categoryDto);
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetCategory(byte id)
        {
            var categoryInDb = await _unitOfWork.Categories.SingleOrDefaultAsync(c => c.Id == id);

            if (categoryInDb == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<Category,CategoryDto>(categoryInDb));
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(byte id, CategoryDto category)
        {
            if (id != category.Id)
            {
                return BadRequest();
            }
            
            var categoryToSave = _mapper.Map<CategoryDto, Category>(category);

            try
            {
                await _unitOfWork.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await CategoryExists(id)==false)
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
        public async Task<ActionResult> PostCategory(Category category)
        {
            await _unitOfWork.Categories.AddAsync(category);
            try
            {
                await _unitOfWork.CompleteAsync();
            }
            catch (DbUpdateException)
            {
                if (await CategoryExists(category.Id))
                {
                    return Conflict();
                }
                else
                {
                    return BadRequest();
                }
            }

            return Ok();
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(byte id)
        {
            var category = await _unitOfWork.Categories.SingleOrDefaultAsync(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            _unitOfWork.Categories.Remove(category);
            await _unitOfWork.CompleteAsync();

            return Ok();
        }

        private async Task<bool> CategoryExists(byte id)
        {
            var rtn =  await _unitOfWork.Categories.SingleOrDefaultAsync(c => c.Id == id);
            if (rtn == null)
                return false;
            return true;
        }

    }
}
