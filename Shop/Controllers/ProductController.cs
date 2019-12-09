using Shop.Data.Repositories;
using Shop.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using Shop.Models.ProductDtos;
using System.Collections.Generic;

namespace Shop.Controllers
{
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProductController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("products/new")]
        public async Task<IActionResult> PostProductAsync([FromBody]ProductDto product)
        {
            if (product == null)
                return BadRequest();
            var productToSave = _mapper.Map<ProductDto, Product>(product);

            await _unitOfWork.Products.AddAsync(productToSave);
            await _unitOfWork.CompleteAsync();
            return Ok();
        }

       
        [HttpGet]
        [Route("products/list")]
        public async Task<IActionResult> GetProductsAsync()
        {
            var productsInDb = await _unitOfWork.Products.GetProductsWthCategoriesAsync();
            if (productsInDb == null)
                return NotFound();
            var productsDto = _mapper.Map<IEnumerable<Product>,IEnumerable<ProductDto>>(productsInDb);
            return Ok(productsDto);
        }


        [HttpGet]
        [Route("products/{id}")]
        public async Task<IActionResult> GetProductAsync(int id)
        {
            var productInDb = await _unitOfWork.Products.GetProductWthCategorieAsync(id);
            if (productInDb == null)
                return NotFound();
            var productDto = _mapper.Map<Product,ProductDto>(productInDb);
            return Ok(productDto);
        }

        [HttpPut]
        [Route("products/edit/{id}")]
        public async Task<IActionResult> PutProductAsync([FromBody]ProductDto product,int id)
        {
            var productInDb = await _unitOfWork.Products.GetProductWthCategorieAsync(id);
            if (productInDb == null)
                return NotFound();

            _mapper.Map(product,productInDb);
            _unitOfWork.Products.Update(productInDb);
            await _unitOfWork.CompleteAsync();
         
            return Ok();
        }

        [HttpDelete]
        [Route("products/delete/{id}")]
        public async Task<IActionResult> DeleteProductAsync(int id)
        {
            var productInDb = await _unitOfWork.Products.GetProductWthCategorieAsync(id);
            if (productInDb == null)
                return NotFound();

            _unitOfWork.Products.Remove(productInDb);
            await _unitOfWork.CompleteAsync();
            return Ok();
        }
    }
}
