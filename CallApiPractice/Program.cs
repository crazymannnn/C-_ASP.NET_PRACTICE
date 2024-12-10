using CallApiPractice;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace HttpClientSample
{
    class Program
    {
        static HttpClient client = new HttpClient();

        static void ShowProduct(Product product)
        {
            Console.WriteLine($"Id: {product.id} Name: {product.name} Price: {product.price}");
        }

        static async Task<Product> GetProductAsync(string path)
        {
            Product product = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                //product = await response.Content.ReadAsAsync<Product>();
                string responseData = await response.Content.ReadAsStringAsync();
                product = JsonSerializer.Deserialize<Product>(responseData);
            }
            return product;
        }
        static void Main()
        {
            RunAsync().GetAwaiter().GetResult();
        }

        static async Task RunAsync()
        {
            // Update port # in the following line.
            client.BaseAddress = new Uri("http://localhost:64195/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                string path = "https://tiki.vn/api/v2/products/7832997?platform=web&spid=60865236&version=3";
                // Get the product
                var product = await GetProductAsync(path);
                ShowProduct(product);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}