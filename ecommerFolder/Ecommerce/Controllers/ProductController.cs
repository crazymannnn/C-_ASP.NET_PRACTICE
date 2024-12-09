using Ecommerce.DALs;
using Ecommerce.Models;
using Ecommerce.Repositories;
using Ecommerce.Repositories.IRepositories;
using Ecommerce.Services.IService;
using Ecommerce.Services.Service;
using Ecommerce.Webmodels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("GetAllProduct"), Authorize(Roles = "Admin,Guest")]
        public IActionResult Get()
        {
            var products = _productService.GetAll();
            return Ok(products);
        }

        [HttpPost("CreateProduct"), Authorize(Roles = "Admin")]
        public IActionResult Create(CreateProductRequest request) { 
            _productService.CreateProduct(request);
            return Ok();
        }
        //private readonly ILogger<ProductController> _logger;
        //private readonly EcommerceContext _context;
        //public static Product product = new Product();
        //private readonly IConfiguration _configuration;

        //public ProductController(ILogger<ProductController> logger, EcommerceContext context, IConfiguration configuration)
        //{
        //    _context = context;
        //    _logger = logger;
        //    _configuration = configuration;
        //}

        //[HttpGet("GetAllProduct"), Authorize(Roles = "Admin,Guest")]
        //public async Task<IActionResult> GetAllProduct()
        //{
        //    var products = await _context.Products.ToListAsync();
        //    return Ok(products);
        //}

        //[HttpPost("CreateProduct"), Authorize(Roles = "Admin")]
        //public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request)
        //{
        //    _context.Products.Add(new Product
        //    {
        //        Name = request.Name,
        //        Price = request.Price,
        //        Quantity = request.Quantity,
        //    });

        //    await _context.SaveChangesAsync();
        //    return Ok(await GetAllProduct());
        //}
    }
}
