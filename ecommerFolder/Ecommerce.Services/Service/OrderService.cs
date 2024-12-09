using Ecommerce.Models;
using Ecommerce.Repositories.IRepositories;
using Ecommerce.Services.IService;
using Ecommerce.Webmodels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;


namespace Ecommerce.Services.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        private readonly IEmailService _emailService;
        public OrderService(IOrderRepository orderRepository, IUserRepository userRepository
            , IProductRepository productRepository, IConfiguration configuration, IEmailService emailService) { 
            
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
            _configuration = configuration;
            _emailService = emailService;
        }
        public int Calculate(List<OrderItem> orderItems)
        {
            int totalAmount = 0;
            var products = _productRepository.GetAll();
            foreach (var item in orderItems) {
                var currentItem = products.Find(x => x.Id == item.ProductId);
                if (currentItem != null) {
                    totalAmount += item.Quantity * currentItem.Price;
                }
            }
            return totalAmount;
        }

        public string PlaceOrder(OrderRequest request, string currentName)
        {

            if (currentName == null)
            {
                return "User not found";
            }

            var currentUser1 = _userRepository.GetAll();
            var currentUser = currentUser1.FirstOrDefault(x => x.Name == currentName);
            string currentEmail = currentUser.Email;

            if (request == null || request.OrderItems.Count == 0)
            {
                return "Invalid order data";
            }
            var order = new Order
            {
                UserId = currentUser.Id,
                OrderDate = DateTime.UtcNow,
                TotalAmount = Calculate(request.OrderItems),
                OrderProducts = new List<OrderProduct>()
            };

            foreach (var item in request.OrderItems)
            {
                var product = _productRepository.GetAll().Find(x => x.Id == item.ProductId); //tim sp trong database
                if (product == null || product.Quantity < item.Quantity)
                {
                    return $"Product {item.ProductId} is unavailable or insufficient stock.";
                }

                product.Quantity -= item.Quantity;

                order.OrderProducts.Add(new OrderProduct
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                });
            }
            var emailRequest = new CreateEmailRequest()
            {
                To = currentEmail,
                Subject = "email was sent",
                Body = "email was sent",
                OrderProducts = order.OrderProducts,
            };
            _orderRepository.Create(order);
            _emailService.SendEmail(emailRequest);
            return "Order successfuly";
        }
    }
}
