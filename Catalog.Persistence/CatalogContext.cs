using Catalog.Domain.Contracts.Persistence;
using Catalog.Domain.Entities;
using Catalog.Persistence.SchemaDefinitions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Persistence
{
    public class CatalogContext : IdentityDbContext<User>, IUnitOfWork
    {
        public const string DEFAULT_SCHEMA = "catalog";

        public DbSet<Item> Items { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Artist> Artists { get; set; }

        public CatalogContext(DbContextOptions<CatalogContext> options) : base(options)
        {
                
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ItemEntitySchemaDefinition());
            modelBuilder.ApplyConfiguration(new GenreEntitySchemaDefinition());
            modelBuilder.ApplyConfiguration(new ArtistEntitySchemaDefinition());
            base.OnModelCreating(modelBuilder);
        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken token = default)
        {
            await SaveChangesAsync();
            return true;
        }
    }
}
