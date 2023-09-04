using AutoMapper;
using Catalog.Domain.Entities;
using Catalog.Domain.Mapper;
using Catalog.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Tests.Fixtures
{
    public class CatalogContextFactory
    {
        public readonly TestCatalogContext _dbContext;
        public readonly IMapper _mapper;
        public CatalogContextFactory()
        {
            var contextOptions = new DbContextOptionsBuilder<CatalogContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging().Options;
            EnsureCreation(contextOptions);
            _dbContext = new TestCatalogContext(contextOptions);

            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CatalogProfile>();
            });
            _mapper = configurationProvider.CreateMapper();
        }

        private void EnsureCreation(DbContextOptions<CatalogContext> contextOptions)
        {
            using var context = new TestCatalogContext(contextOptions);
            context.Database.EnsureCreated();
        }
    }
}
