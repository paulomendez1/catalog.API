using Catalog.Domain.Contracts.Persistence;
using Catalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Persistence.Repositories
{
    public class GenreRepository : BaseRepository<Genre>, IGenreRepository
    {
        public IUnitOfWork UnitOfWork;
        public GenreRepository(CatalogContext context) : base(context)
        {
            UnitOfWork = context;
        }
    }
}
