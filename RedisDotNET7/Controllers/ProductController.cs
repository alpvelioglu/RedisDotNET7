using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedisDotNET7.Models;
using RedisDotNET7.Redis;
using RedisDotNET7.Repository;
using RedisDotNET7.Services;

namespace RedisDotNET7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        //private readonly IProductRepo _productRepo;
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _productService.GetAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _productService.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            return Created(string.Empty, await _productService.CreateAsync(product));
        }
    }
}
