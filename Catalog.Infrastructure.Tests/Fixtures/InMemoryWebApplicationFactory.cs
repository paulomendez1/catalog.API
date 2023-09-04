using Catalog.API;
using Catalog.Persistence;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebMotions.Fake.Authentication.JwtBearer;

namespace Catalog.Infrastructure.Tests.Fixtures
{
    public class InMemoryWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder
                .UseEnvironment("Testing")
                .ConfigureTestServices(services =>
            {
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = FakeJwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = FakeJwtBearerDefaults.AuthenticationScheme;
                }).AddFakeJwtBearer();
            var options = new DbContextOptionsBuilder<CatalogContext>()
                                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                                .Options;
                services.AddScoped<CatalogContext> (serviceProvider => new TestCatalogContext(options));
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService <CatalogContext>();
                db.Database.EnsureCreated();
                services.Replace(ServiceDescriptor.Scoped(_ => new UserContextFactory().InMemoryUserManager));
            });
        }
    }
}
