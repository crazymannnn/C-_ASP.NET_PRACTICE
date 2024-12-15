using Ecommerce.DALs;
using Ecommerce.Models;
using Ecommerce.Repositories;
using Ecommerce.Repositories.IRepositories;
using Ecommerce.Services;
using Ecommerce.Services.IService;
using Ecommerce.Webmodels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Ecommerce.Controllers
{
    [Route("[Controller]")]
    [Authorize(Roles = "Guest")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly IUserService _userService;
        public readonly IEmailService _emailService;
        public OrderController(IOrderService orderService, IProductService productService, IUserService userService, IEmailService emailService = null)
        {
            _orderService = orderService;
            _productService = productService;
            _userService = userService;
            _emailService = emailService;
        }

        [HttpPost("PlaceOrder")]
        public IActionResult PlaceOrder([FromBody] OrderRequest request) {           
            string currentName = User.FindFirstValue(ClaimTypes.Name);
            if (currentName == null)
            {
                return Unauthorized("User not found");
            }
            var result = _orderService.PlaceOrder(request, currentName);          
            return Ok(result);
        }
        //private readonly ILogger<OrderController> _logger;
        //private readonly EcommerceContext _context;
        //private readonly IConfiguration _configuration;
        //public OrderController(ILogger<OrderController> logger, EcommerceContext context, IConfiguration configuration)
        //{
        //    _context = context;
        //    _logger = logger;
        //    _configuration = configuration;
        //}

        //[HttpPost("PlaceOrder")]
        //public async Task<IActionResult> PlaceOrder([FromBody] OrderRequest request)
        //{   
        //    //find customer throw token
        //    var userName = User.FindFirstValue(ClaimTypes.Name);
        //    if (userName == null) {
        //        return Unauthorized("Invalid user");
        //    }

        //    var currentUser = await _context.Users.FirstOrDefaultAsync(x => x.Name == userName);

        //    if (request == null || request.OrderItems.Count == 0)
        //    {
        //        return BadRequest("Invalid order data");
        //    }

        //    var order = new Order
        //    {
        //        UserId = currentUser.Id,
        //        OrderDate = DateTime.UtcNow,
        //        TotalAmount = CalculateTotalAmount(request.OrderItems),
        //        OrderProducts = new List<OrderProduct>()
        //    };

        //    foreach (var item in request.OrderItems)
        //    {
        //        var product = _context.Products.Find(item.ProductId); //tim sp trong database
        //        if (product == null || product.Quantity < item.Quantity)
        //        {
        //            return BadRequest($"Product {item.ProductId} is unavailable or insufficient stock.");
        //        }

        //        product.Quantity -= item.Quantity;

        //        order.OrderProducts.Add(new OrderProduct
        //        {
        //            ProductId = item.ProductId,
        //            Quantity = item.Quantity
        //        });
        //    }

        //    _context.Orders.Add(order);
        //    _context.SaveChanges();

        //    return Ok(new { Message = "Order placed successfully", OrderId = order.OrderId });
        //}

        //private int CalculateTotalAmount(List<OrderItem> orderItems)
        //{
        //    int totalAmount = 0;
        //    foreach (var item in orderItems)
        //    {
        //        var product = _context.Products.Find(item.ProductId);
        //        if (product != null)
        //        {
        //            totalAmount += product.Price * item.Quantity;
        //        }
        //    }
        //    return totalAmount;
        //}
    }
}
