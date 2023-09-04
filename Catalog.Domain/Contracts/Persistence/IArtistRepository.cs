using Catalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Contracts.Persistence
{
    public interface IArtistRepository : IRepository<Artist>
    {
    }
}
