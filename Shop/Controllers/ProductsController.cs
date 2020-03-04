using Shop.Data.Repositories;
using Shop.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Shop.Models.ProductDtos;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Shop.Controllers
{

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

        [HttpPost]
        public async Task<IActionResult> PostProductAsync(ProductDto product)
        {
            if (product == null)
                return BadRequest();           

            var productToSave = _mapper.Map<ProductDto, Product>(product);
            await _unitOfWork.Products.AddAsync(productToSave);

            try
            {
                await _unitOfWork.CompleteAsync();
            }
            catch (DbUpdateException)
            {
                 return BadRequest();
            }

            return Ok();
        }


        [HttpGet]
        public async Task<IActionResult> GetProductsAsync()
        {
            var productsInDb = (List<Product>) await _unitOfWork.Products.GetProductsWthCategoriesAsync();
            if (productsInDb==null)
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
        public async Task<IActionResult> PutProductAsync(ProductDto product,int id)
        {
            if (product == null)
                return BadRequest();

            var productInDb = await _unitOfWork.Products.GetProductWthCategorieAsync(id);

            if (productInDb == null)
                return NotFound();

            _mapper.Map(product,productInDb);


            //_unitOfWork.Products.Update(productInDb);
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
