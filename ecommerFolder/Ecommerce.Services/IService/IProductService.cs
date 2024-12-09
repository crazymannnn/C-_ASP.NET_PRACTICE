using Ecommerce.Models;
using Ecommerce.Webmodels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Services.IService
{
    public interface IProductService
    {
        List<Product> GetAll();
        void CreateProduct(CreateProductRequest request);

    }
}
