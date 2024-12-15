using CallApiPractice;
using Polly.Extensions.Http;
using Polly;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Http;

namespace HttpClientSample
{
    class Program
    {
        static HttpClient client = new HttpClient();
        static string jwtToken = "initial-token";

        static void ShowProduct(Product product)
        {
            Console.WriteLine($"Id: {product.Id} Name: {product.Name}");
            foreach (var b in product.badges)
            {
                Console.WriteLine($"Code: {b.code} Text: {b.text}");
            }
        }

        static async Task<Product> GetProductAsync(string path)
        {
            Product product = null;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
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
            var retryPolicy = GetRetryPolicy();
            var handler = new PolicyHttpMessageHandler(retryPolicy)
            {
                InnerHandler = new HttpClientHandler()
            };
            client = new HttpClient(handler)
            {
                BaseAddress = new Uri("http://localhost:64195/")
            };

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                //string path = "https://tiki.vn/api/v2/products/7832997?platform=web&spid=60865236&version=3";
                //string path = "https://localhost:7080/Home/GetAllUser";
                string path = "https://localhost:7080/Product/GetAllProduct";
                // Get the product
                var product = await GetProductAsync(path);
                ShowProduct(product);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Request failed: {e.Message}");
            }

            Console.ReadLine();
        }

        //Retry with Polly
        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError() // Handles 5xx and 408 errors
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound) // Retry on 404
                .WaitAndRetryAsync(4, retryAttempt => TimeSpan.FromSeconds(2),
                onRetry: async (response, timespan, retryCount, context) =>
                {
                    if (response.Result?.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        Console.WriteLine($"Token expired. Attempting to refresh token (Retry {retryCount})...");
                        jwtToken = await RefreshJwtTokenAsync();
                    }
                    Console.WriteLine($"Retry {retryCount} due to {response.Result?.StatusCode}. Retrying in {timespan.TotalSeconds} seconds...");
                });
        }

        static async Task<string> RefreshJwtTokenAsync()
        {
            // Simulate API call to refresh token
            Console.WriteLine("Refreshing JWT token...");
            await Task.Delay(1000); // Simulate network delay
            return "new-refreshed-token"; // Replace with actual token fetching logic
        }
    }
}