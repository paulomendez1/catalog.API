using Catalog.API.ResponseModels;
using Catalog.Domain.DTOs.Request.Artist;
using Catalog.Domain.DTOs.Request.Genre;
using Catalog.Domain.DTOs.Request.Item;
using Catalog.Domain.DTOs.Response;
using Catalog.Domain.Entities;
using Catalog.Infrastructure.Tests.Fixtures;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.API.Tests
{
    public class GenreControllerTests : IClassFixture<InMemoryWebApplicationFactory>
    {
        private readonly InMemoryWebApplicationFactory _factory;
        public GenreControllerTests(InMemoryWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/api/genre/?pageSize=1&pageIndex=0", 1, 0)]
        [InlineData("/api/genre/?pageSize=2&pageIndex=0", 2, 0)]
        [InlineData("/api/genre/?pageSize=1&pageIndex=1", 1, 1)]

        public async Task Get_Success(string url, int pageSize, int pageIndex)
        {
            var client = _factory.CreateClient();
            string token = GenerateSecurityToken();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync(url);
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseEntity = JsonConvert.DeserializeObject<PaginatedResponseModel<GenreResponse>>(responseContent);
            Assert.Equal(pageSize, responseEntity.PageSize);
            Assert.Equal(pageIndex, responseEntity.PageIndex);
            Assert.Equal(pageSize, responseEntity.Data.Count());
        }
        [Fact]
        public async Task GetById_Success()
        {
            const string id = "c04f05c0-f6ad-44d1-a400-3375bfb5dfd6";
            var client = _factory.CreateClient();
            string token = GenerateSecurityToken();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync($"/api/genre/{id}");
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseEntity = JsonConvert.DeserializeObject<Genre>(responseContent);
            Assert.NotNull(responseEntity);
        }

        [Fact]
        public async Task GetItemsById_Success()
        {
            const string id = "c04f05c0-f6ad-44d1-a400-3375bfb5dfd6";
            var client = _factory.CreateClient();
            string token = GenerateSecurityToken();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync($"/api/genre/{id}/items");
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseEntity = JsonConvert.DeserializeObject<IEnumerable<ItemResponse>>(responseContent);
            Assert.NotNull(responseEntity);
        }

        [Fact]
        public async Task Add_Success()
        {
            var request = new AddGenreRequest
            {
                GenreDescription = "Test description",
            };
            var client = _factory.CreateClient();
            string token = GenerateSecurityToken();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var httpContent = new StringContent(JsonConvert.SerializeObject(request),
                Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"/api/genre", httpContent);
            response.EnsureSuccessStatusCode();
            Assert.NotNull(response.Headers.Location);
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
