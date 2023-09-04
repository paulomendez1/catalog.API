using Catalog.Domain.DTOs.Request.User;
using Catalog.Infrastructure.Tests.Fixtures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.API.Tests
{
    public class UserControllerTests : IClassFixture<InMemoryWebApplicationFactory>
    {
        private readonly InMemoryWebApplicationFactory _factory;
        public UserControllerTests(InMemoryWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/api/user/auth")]
        public async Task SignInWithCorrectUser_ReturnsToken(string url)
        {
            var client = _factory.CreateClient();
            var request = new SignInRequest
            {
                Email = "paulooo_rc@example.com",
                Password = "P@$$w0rd"
            };
            var httpContent = new StringContent(JsonConvert.SerializeObject(request),
                                                Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, httpContent);
            string responseContent = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotEmpty(responseContent);
        }

        [Theory]
        [InlineData("/api/user/auth")]
        public async Task SignInWithWrongPassword_ReturnsBadRequest(string url)
        {
            var client = _factory.CreateClient();
            var request = new SignInRequest
            {
                Email = "paulooo_rc@example.com",
                Password = "NotValidPWD"
            };
            var httpContent = new StringContent(JsonConvert.SerializeObject(request),
                                                 Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, httpContent);
            string responseContent = await response.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
