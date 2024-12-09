using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Passwordhash { get; set; }
        public string Role { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
