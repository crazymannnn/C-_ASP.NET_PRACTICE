using Ecommerce.Models;
using Ecommerce.Repositories.IRepositories;
using Ecommerce.Repositories.Repositories;
using Ecommerce.Services.IService;
using Ecommerce.Webmodels;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Services.Service
{
    public class ProductService2 : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IDistributedCache _cache;
        private readonly ILogger<ProductService> _logger;
        public ProductService2(IProductRepository productRepository,IDistributedCache cache, ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _cache = cache;
            _logger = logger;
        }
        public void CreateProduct(CreateProductRequest request)
        {
            Product product = new Product()
            {
                Name = request.Name,
                Price = request.Price,
                Quantity = request.Quantity,
            };

            _productRepository.Create(product);

            var cacheKey = "products";
            _logger.LogInformation("invalidating cache for key: {CacheKey} from cache.", cacheKey);
            _cache.Remove(cacheKey);
        }

        public List<Product> GetAll()
        {
            //var cacheKey = "products";
            //_logger.LogInformation("fetching data for key: {CacheKey} from cache.", cacheKey);
            //var cacheOptions = new DistributedCacheEntryOptions()
            //        .SetAbsoluteExpiration(TimeSpan.FromMinutes(20))
            //        .SetSlidingExpiration(TimeSpan.FromMinutes(2));
            //var products = _cache.GetOrSetAsync(
            //    cacheKey,
            //    async () =>
            //    {
            //        _logger.LogInformation("cache miss. fetching data for key: {CacheKey} from database.", cacheKey);
            //        return _productRepository.GetAll();
            //    },
            //    cacheOptions)!;
            //return products!;
            return null;
        }
    }
}
