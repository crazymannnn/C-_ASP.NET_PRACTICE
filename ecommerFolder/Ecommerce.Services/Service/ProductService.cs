using Ecommerce.Models;
using Ecommerce.Repositories.IRepositories;
using Ecommerce.Repositories.Repositories;
using Ecommerce.Services.IService;
using Ecommerce.Webmodels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Services.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IConfiguration _configuration;

        public ProductService(IProductRepository productRepository, IConfiguration configuration)
        {
            _productRepository = productRepository;
            _configuration = configuration;
        }
        public void CreateProduct(CreateProductRequest request)
        {
            Product product = new Product() { 
                Name = request.Name,
                Price = request.Price,
                Quantity = request.Quantity,
            };

            _productRepository.Create(product);
        }

        public List<Product> GetAll()
        {
            return _productRepository.GetAll();
        }
    }
}
