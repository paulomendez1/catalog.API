using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Contracts.Persistence
{
    public interface IRepository<T> where T : class
    {
        IUnitOfWork UnitOfWork { get; }
        Task<IEnumerable<T>> GetAllAsync(CancellationToken token);
        Task<T> GetByIdAsync(Guid id, CancellationToken token);
        T Add(T entity, CancellationToken token);
        T Update(T entity, CancellationToken token);

    }
}
