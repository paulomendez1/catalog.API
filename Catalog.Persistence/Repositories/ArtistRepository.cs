using Catalog.Domain.Contracts.Persistence;
using Catalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Persistence.Repositories
{
    public class ArtistRepository : BaseRepository<Artist>, IArtistRepository
    {
        public IUnitOfWork UnitOfWork;
        public ArtistRepository(CatalogContext context) : base(context)
        {
            UnitOfWork = context;
        }
    }
}
