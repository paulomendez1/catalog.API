using Catalog.Domain.DTOs.Request.Item;
using Catalog.Domain.DTOs.Response;
using System.Text;
using System.Text.Json;

namespace Catalog.API.SyncDataServices.Http
{
    public class CartDataService : ICartDataService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public CartDataService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;  
            _configuration = configuration;
        }
        public async Task SendItemToCart(ItemResponse item)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(item),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync("http://localhost:5080/api/c/items/", httpContent);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Sync POST");
            }
            else
            {
                Console.WriteLine("Sync Post NOT OK!");
            }
        }
    }
}
