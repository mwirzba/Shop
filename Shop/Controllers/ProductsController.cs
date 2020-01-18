using Shop.Data.Repositories;
using Shop.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Shop.Models.ProductDtos;
using System.Collections.Generic;

namespace Shop.Controllers
{
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

        [HttpPost]
        public async Task<IActionResult> PostProductAsync([FromBody]ProductDto product)
        {
            if (product == null || !ModelState.IsValid)
                return BadRequest();

            var productToSave = _mapper.Map<ProductDto, Product>(product);

            await _unitOfWork.Products.AddAsync(productToSave);
            await _unitOfWork.CompleteAsync();
            return Ok();
        }


        [HttpGet]
        public async Task<IActionResult> GetProductsAsync()
        {
            var productsInDb = await _unitOfWork.Products.GetProductsWthCategoriesAsync();
            if (productsInDb == null)
                return NotFound();
            var productsDto = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(productsInDb);
            return Ok(productsDto);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductAsync(int id)
        {
        var productInDb = await _unitOfWork.Products.GetProductWthCategorieAsync(id);
        if (productInDb == null)
            return NotFound();
        var productDto = _mapper.Map<Product, ProductDto>(productInDb);
        return Ok(productDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditProductAsync([FromBody]ProductDto product,int id)
        {
            if (product == null || !ModelState.IsValid)
                return BadRequest();

            var productInDb = await _unitOfWork.Products.GetProductWthCategorieAsync(id);

            if (productInDb == null)
                return NotFound();

            _mapper.Map(product,productInDb);
            _unitOfWork.Products.Update(productInDb);
            await _unitOfWork.CompleteAsync();
         
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductAsync(int id)
        {
            var productInDb = await _unitOfWork.Products.GetAsync(id);
            if (productInDb == null)
                return NotFound();

            _unitOfWork.Products.Remove(productInDb);
            await _unitOfWork.CompleteAsync();
            return NoContent();
        }
    }
}
