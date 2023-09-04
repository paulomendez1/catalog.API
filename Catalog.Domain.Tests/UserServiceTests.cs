using Catalog.Domain.Configurations;
using Catalog.Domain.DTOs.Request.User;
using Catalog.Domain.Services;
using Catalog.Infrastructure.Tests.Fixtures;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Tests
{
    public class UserServiceTests : IClassFixture<UserContextFactory>
    {
        private readonly IUserService _userService;
        public UserServiceTests(UserContextFactory usersContextFactory)
        {
            _userService = new UserService(usersContextFactory.InMemoryUserManager, Options.Create(
                                            new AuthenticationSettings
                                            {
                                                Secret = "Very Secret key-word to match",
                                                ExpirationMinutes = 5
                                            }));
        }

        [Fact]
        public async Task SignInWithWrongUser_ReturnsNull()
        {
            var result = await _userService.SignInAsync(new SignInRequest
            {
                Email = "invalid.user",
                Password = "invalid_password"
            });
            Assert.Null(result);
        }

        [Fact]
        public async Task SignInWithCorrectUser_ReturnsToken()
        {
            var result = await _userService.SignInAsync(new SignInRequest
            {
                Email = "paulooo_rc@example.com",
                Password = "P@$$w0rd"
            });
            Assert.NotEmpty(result.Token);
        }

    }
}
