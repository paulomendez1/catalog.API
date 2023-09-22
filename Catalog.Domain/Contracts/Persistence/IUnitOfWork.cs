using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Contracts.Persistence
{
    public interface IUnitOfWork : IDisposable 
    {
        Task<int> SaveChangesAsync(CancellationToken token = default(CancellationToken));
        Task<bool> SaveEntitiesAsync(CancellationToken token = default(CancellationToken));
    }
}
