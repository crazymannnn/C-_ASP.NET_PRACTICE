using Ecommerce.Models;
using Ecommerce.Webmodels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Services.IService
{
    public interface IOrderService
    {
        string PlaceOrder(OrderRequest request, string currentName);
        int Calculate(List<OrderItem> orderItems);
    }
}
