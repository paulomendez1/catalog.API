using Catalog.Domain.Entities;
using Catalog.Infrastructure.Tests.Fixtures.Extensions;
using Catalog.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Tests.Fixtures
{
    public class TestCatalogContext : CatalogContext
    {
        public TestCatalogContext(DbContextOptions<CatalogContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Seed<Artist>("./Fixtures/Data/artist.json");
            modelBuilder.Seed<Genre>("./Fixtures/Data/genre.json");
            modelBuilder.Seed<Item>("./Fixtures/Data/item.json");
        }
    }

}

