using Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Webmodels
{
    public class OrderRequest
    {
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
