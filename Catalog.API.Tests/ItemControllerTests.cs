using Microsoft.AspNetCore.Hosting;
using Catalog.Infrastructure.Tests.Fixtures;
using Catalog.API;
using Catalog.Domain.Entities;
using Newtonsoft.Json;
using Catalog.Domain.DTOs.Request;
using System.Text;
using Catalog.API.ResponseModels;
using Moq;
using Catalog.Domain.DTOs.Response;
using Catalog.Domain.DTOs.Request.Item;
using System.Net;
using Catalog.Domain.Services;
using Catalog.Domain.DTOs.Request.User;
using Newtonsoft.Json.Linq;
using System.Dynamic;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Identity;
using System.Net.Http.Headers;
using Catalog.Domain.Configurations;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Catalog.API.Tests
{
    public class ItemControllerTests : IClassFixture<InMemoryWebApplicationFactory>
    {
        private readonly InMemoryWebApplicationFactory _factory;
        public ItemControllerTests(InMemoryWebApplicationFactory factory)
        {
            _factory = factory;
        }
        [Theory]
        [InlineData("/api/items/?pageSize=1&pageIndex=0",1,0)]
        [InlineData("/api/items/?pageSize=2&pageIndex=0",2,0)]
        [InlineData("/api/items/?pageSize=1&pageIndex=1",1,1)]

        public async Task Get_Success(string url, int pageSize, int pageIndex)
        {
            var client = _factory.CreateClient();

            string token = GenerateSecurityToken();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync(url);
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseEntity = JsonConvert.DeserializeObject<IEnumerable<ItemResponse>>(responseContent);
            Assert.Equal(pageSize, responseEntity.Count());
        }
        [Fact]
        public async Task GetById_Success()
        {
            const string id = "86bff4f7-05a7-46b6-ba73-d43e2c45840f";
            var client = _factory.CreateClient();

            string token = GenerateSecurityToken();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync($"/api/items/{id}");
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseEntity = JsonConvert.DeserializeObject<Item>(responseContent);
            Assert.NotNull(responseEntity);
        }

        [Fact]
        public async Task Add_Success()
        {
            var request = new AddItemRequest
            {
                Name = "Test album",
                Description = "Description",
                LabelName = "Label name",
                Price = new Price { Amount = 13, Currency = "EUR" },
                PictureUri = "https://mycdn.com/pictures/32423423",
                ReleaseDate = DateTimeOffset.Now,
                AvailableStock = 6,
                GenreId = new Guid("c04f05c0-f6ad-44d1-a400-3375bfb5dfd6"),
                ArtistId = new Guid("f08a333d-30db-4dd1-b8ba-3b0473c7cdab"),
                Format = ""
            };
            var client = _factory.CreateClient();
            string token = GenerateSecurityToken();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var httpContent = new StringContent(JsonConvert.SerializeObject(request),
                Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"/api/items", httpContent);
            response.EnsureSuccessStatusCode();
            Assert.NotNull(response.Headers.Location);
        }

        [Fact]
        public async Task Update_Success()
        {
            var client = _factory.CreateClient();
            string token = GenerateSecurityToken();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = new EditItemRequest
            {
                Id = new Guid("b5b05534-9263-448c-a69e-0bbd8b3eb90e"),
                Name = "Test album",
                Description = "Description updated",
                LabelName = "Label name",
                Price = new Price { Amount = 50, Currency = "EUR" },
                PictureUri = "https://mycdn.com/pictures/32423423",
                ReleaseDate = DateTimeOffset.Now,
                AvailableStock = 6,
                GenreId = new Guid("c04f05c0-f6ad-44d1-a400-3375bfb5dfd6"),
                ArtistId = new Guid("f08a333d-30db-4dd1-b8ba-3b0473c7cdab"),
                Format = ""
            };
            var httpContent = new StringContent(JsonConvert.SerializeObject(request), 
                Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"/api/items/{request.Id}", httpContent);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseEntity = JsonConvert.DeserializeObject<Item>(responseContent);
            Assert.Equal(request.Name, responseEntity.Name);
            Assert.Equal(request.Description, responseEntity.Description);
            Assert.Equal(request.GenreId, responseEntity.GenreId);
            Assert.Equal(request.ArtistId, responseEntity.ArtistId);
        }

        [Fact]
        public async Task Delete_Success()
        {
            var request = new DeleteItemRequest()
            {
                Id = new Guid("b5b05534-9263-448c-a69e-0bbd8b3eb90e")
            };
            var client = _factory.CreateClient();
            string token = GenerateSecurityToken();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.DeleteAsync($"/api/items/{request.Id}");
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }


        private string GenerateSecurityToken()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("My Super long secret");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, "paulooo_rc@example.com") }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                                                                SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}