using System.Collections.Generic;
using System.Linq;
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
    /// Controller responsible for product categories CRUD
    /// </summary>
    [ApiController]
    [AllowAnonymous]
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

        /// <summary>
        /// Returns list of all categories.
        /// </summary>
        /// <response code="200">Returned categories</response>
        /// <response code="404">No categories found in db.</response>
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categoriesInDb = await _unitOfWork.Categories.GetAllAsync();
            if (categoriesInDb == null || categoriesInDb.Count() == 0)
                return NotFound();
            var categoryDto = _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryDto>>(categoriesInDb);
            return Ok(categoryDto);
        }


        /// <summary>
        /// Retrieves category by unique id.
        /// </summary>
        /// <param name="id">Category id</param>
        /// <response code="200">Category found.</response>
        /// <response code="404">Category with given id not found.</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory([FromBody]byte id)
        {
            var categoryInDb = await _unitOfWork.Categories.SingleOrDefaultAsync(c => c.Id == id);

            if (categoryInDb == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<Category,CategoryDto>(categoryInDb));
        }



        /// <summary>
        /// Updates category by unique id.
        /// </summary>
        /// <param name="id">Category id</param>
        /// <param name="category">category</param>
        /// <response code="200">Category updated</response>
        /// <response code="404">Category not found</response>
        /// <response code="400">Exception during database update happened</response>
        /// <response code="422">Missing category parameter</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory([FromQuery]byte id,[FromBody] CategoryDto category)
        {
            if (category == null)
            {
                return UnprocessableEntity();
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

            return Ok();
        }

        /// <summary>
        /// Creates new category
        /// </summary>
        /// <param name="category">category</param>
        /// <response code="200">Category created</response>
        /// <response code="400">Exception during database update happened</response>
        /// <response code="422">Missing category parameter</response>
        [HttpPost]
        public async Task<IActionResult> PostCategory([FromBody]CategoryDto category)
        {
            if (category == null)
            {
                return UnprocessableEntity();
            }
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


        /// <summary>
        /// Deletes category with given id.
        /// </summary>
        /// <param name="id">Category id</param>
        /// <response code="200">Category deleted</response>
        /// <response code="404">Category with id not found</response>
        /// <response code="400">Exception during database update happened</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory([FromQuery]byte id)
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
            return Ok();
        }

    }
}
