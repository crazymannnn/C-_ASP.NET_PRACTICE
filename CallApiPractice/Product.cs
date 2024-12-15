using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CallApiPractice
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public List<Badge> badges { get; set; } = new List<Badge> { };
    }

    public class Badge { 
        public string code { get; set; }
        public string text { get; set; }
    }
}
