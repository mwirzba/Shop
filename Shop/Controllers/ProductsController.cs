using Shop.Data.Repositories;
using Shop.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Shop.Dtos;
using System;
using Shop.ResponseHelpers;
using Newtonsoft.Json;
using Shop.Respn;

namespace Shop.Controllers
{
    /// <summary>
    /// Controller responsible for product CRUD actions
    /// </summary>
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates new product
        /// </summary>
        /// <param name="product">Product data.</param>
        /// <response code="200">Created new product</response>
        /// <response code="422">Missing product in body</response>
        /// <response code="400">Exception during database update or other proccess</response>
        [HttpPost]
        public async Task<IActionResult> PostProductAsync([FromBody]ProductDto product)
        {
            if (product == null)
                return BadRequest();

            try
            {
                var productToSave = _mapper.Map<ProductDto, Product>(product);
                await _unitOfWork.Products.AddAsync(productToSave);
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
        ///  Returnes paginaged list of produtcs
        /// </summary>
        /// <param name="paginationQuery">Pagination settings</param>
        /// <param name="filterParams">Params for filtering paginatied list</param>
        /// <response code="200">Returned paginated products</response>
        /// <response code="404">Not found products in database</response>
        /// <response code="400">Exception during database update or other proccess</response>
        [HttpGet]
        public async Task<IActionResult> GetProductsByPageAsync([FromQuery]PaginationQuery paginationQuery,[FromQuery] FilterParams filterParams)
        {
            try
            {
                PagedList<Product> productsInDb = await _unitOfWork.Products.GetPagedProductsWthCategoriesByFiltersAsync(paginationQuery, filterParams);
                
                if (productsInDb == null)
                    return NotFound();
                var productsDtoPage = PagedListMapper<Product, ProductDto>.Map(productsInDb, _mapper);

                var metadata = new
                {
                    productsDtoPage.TotalCount,
                    productsDtoPage.PageSize,
                    productsDtoPage.CurrentPage,
                    productsDtoPage.TotalPages,
                    productsDtoPage.HasNext,
                    productsDtoPage.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

                return Ok(productsDtoPage);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        /// <summary>
        /// Retrives all products
        /// </summary>
        /// <response code="200">Returned list of products</response>
        /// <response code="404">Not found products in db</response>
        /// <response code="400">Exception during database update or other proccess.</response>
        [HttpGet("all")]
        public async Task<IActionResult> GetProductsAsync()
        {
            try
            {
                var productsInDb = await _unitOfWork.Products.GetProductsWthCategoriesAsync();
                if (productsInDb == null)
                    return NotFound();
                var productsDto = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(productsInDb);
                return Ok(productsDto);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        /// <summary>
        /// Retrives product by unique id
        /// </summary>
        /// <param name="id">Product id</param>
        /// <response code="200">Returned list of products</response>
        /// <response code="404">Not found products in db</response>
        /// <response code="400">Exception during database update or other proccess.</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductAsync(int id)
        {
            try
            {
                var productInDb = await _unitOfWork.Products.GetProductWthCategorieAsync(id);
                if (productInDb == null)
                    return NotFound();
                var productDto = _mapper.Map<Product, ProductDto>(productInDb);
                return Ok(productDto);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        /// <summary>
        /// Updates product with given id
        /// </summary>
        /// <param name="product"></param>
        /// <param name="id">Product id</param>
        /// <response code="200">Returned list of products</response>
        /// <response code="404">Not found product with given id</response>
        /// <response code="400">Exception during database update or other proccess.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductAsync(ProductDto product,int id)
        {
            if (product == null)
                return BadRequest();

            var productInDb = await _unitOfWork.Products.GetProductWthCategorieAsync(id);

            if (productInDb == null)
                return NotFound();

            _mapper.Map(product,productInDb);
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

            return NoContent();
        }


        /// <summary>
        /// Deletes product with given id
        /// </summary>
        /// <param name="id">Product id</param>
        /// <response code="200">Deleted product</response>
        /// <response code="200">Not found product with given id.</response>
        /// <response code="400">Exception during database update or other proccess</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductAsync(int id)
        {
            var productInDb = await _unitOfWork.Products.GetAsync(id);
            if (productInDb == null)
                return NotFound();

            _unitOfWork.Products.Remove(productInDb);
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
    }
}
