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
    public class ArtistEntitySchemaDefinition : IEntityTypeConfiguration<Artist>
    {
        public void Configure(EntityTypeBuilder<Artist> builder)
        {
            builder.ToTable("Artists", CatalogContext.DEFAULT_SCHEMA);
            builder.HasKey(k => k.ArtistId);
            builder.Property(p => p.ArtistId);
            builder.Property(p => p.ArtistName).IsRequired().HasMaxLength(200);
        }
    }
}
