using CallApiPractice;
using Microsoft.Extensions.Caching.Memory;
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
        static IMemoryCache memoryCache = new MemoryCache(new MemoryCacheOptions());

        static void ShowProduct(Product product)
        {
            Console.WriteLine($"Id: {product.id} Name: {product.name} Price: {product.price}");
            foreach (var b in product.badges) {
                Console.WriteLine($"Code: {b.code} Text: {b.text}");
            }

        }

        static async Task<Product> GetProductAsync(string path)
        {
            if (memoryCache.TryGetValue(path, out Product cachedProduct))
            {
                Console.WriteLine("Cache hit! Returning cached product.");
                return cachedProduct;
            }

            Console.WriteLine("Cache miss! Fetching product from API...");
            Product product = null;
            HttpResponseMessage response = await client.GetAsync(path);

            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                product = JsonSerializer.Deserialize<Product>(responseData);

                // Add the product to the cache
                if (product != null)
                {
                    memoryCache.Set(path, product, TimeSpan.FromSeconds(30));
                }
            }

            return product;
        }

        static async Task FetchAndDisplayProduct(string apiUrl)
        {
            var product = await GetProductAsync(apiUrl);
            ShowProduct(product);
        }
        static void Main()
        {
            RunAsync().GetAwaiter().GetResult();
        }

        static async Task RunAsync()
        {
            client.BaseAddress = new Uri("http://localhost:64195/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                string path = "https://tiki.vn/api/v2/products/7832997?platform=web&spid=60865236&version=3";

                Console.WriteLine("Press Enter to call the API.");
                while (true)
                {
                    Console.ReadLine(); // Simulate button press
                    await FetchAndDisplayProduct(path);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        //test branch.....
    }
}