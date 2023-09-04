using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Persistence.SchemaDefinitions
{
    public class GenreEntitySchemaDefinition : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> builder)
        {
            builder.ToTable("Genres", CatalogContext.DEFAULT_SCHEMA);
            builder.HasKey(k => k.GenreId);
            builder.Property(p => p.GenreId);
            builder.Property(p => p.GenreDescription).IsRequired().HasMaxLength(1000);

        }
    }
}
