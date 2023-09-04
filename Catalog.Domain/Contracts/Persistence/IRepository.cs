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
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(Guid id);
        T Add(T entity);
        T Update(T entity);

    }
}
