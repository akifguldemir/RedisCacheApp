using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedisCacheApp.API.Models;
using RedisCacheApp.API.Repository;

namespace RedisCacheApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _productRepository.GetAsync()); 
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(Product product)
        {
            if (product == null)
            {
                return BadRequest();
            }
            var createdProduct = await _productRepository.CreateAsync(product);
            return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
        }
    }
}
