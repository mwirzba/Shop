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
        [Route("product/new")]
        public async Task<IActionResult> PostProductAsync([FromBody]ProductDto product)
        {
            if (product == null)
                return BadRequest();
            var productToSave = _mapper.Map<ProductDto, Product>(product);

            await _unitOfWork.Products.AddAsync(productToSave);
            await _unitOfWork.CompleteAsync();
            return Ok();
        }

        [Route("product/products")]
        [HttpGet]
        public async Task<IActionResult> GetProductsAsync()
        {
            var productsInDb = await _unitOfWork.Products.GetProductsWthCategoriesAsync();
            var productsDto = _mapper.Map<IEnumerable<Product>,IEnumerable<ProductDto>>(productsInDb);
            return Ok(productsDto);
        }
    }
}
