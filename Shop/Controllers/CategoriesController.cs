using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data.Repositories;
using Shop.Dtos;
using Shop.Models;

namespace Shop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
        public async Task<IActionResult> GetCategories()
        {
            var categoryInDb = await _unitOfWork.Categories.GetAllAsync();
            if (categoryInDb == null)
                return NotFound();
            var categoryDto = _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryDto>>(categoryInDb);
            return Ok(categoryDto);
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(byte id)
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
            if (category == null)
            {
                return BadRequest();
            }

            var categoryInDb = await _unitOfWork.Categories.SingleOrDefaultAsync(c => c.Id == id);

            if (categoryInDb == null)
            {
                return NotFound();
            }

            _mapper.Map(category,categoryInDb);

            try
            {
                await _unitOfWork.CompleteAsync();
            }
            catch (DbUpdateException e)
            {
                return BadRequest(e.Message);
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> PostCategory(CategoryDto category)
        {
            var categoryToSave = _mapper.Map<CategoryDto,Category>(category);
            await _unitOfWork.Categories.AddAsync(categoryToSave);
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

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(byte id)
        {
            var category = await _unitOfWork.Categories.SingleOrDefaultAsync(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            try
            {
                _unitOfWork.Categories.Remove(category);
                await _unitOfWork.CompleteAsync();
            }
            catch (DbUpdateException e)
            {
                return BadRequest(e.Message);
            }
            return NoContent();
        }

    }
}
