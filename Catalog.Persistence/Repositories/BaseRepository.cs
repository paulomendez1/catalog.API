using Catalog.Domain.Contracts.Persistence;
using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Persistence.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        protected readonly CatalogContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public BaseRepository(CatalogContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken token)
        {
            await Task.Delay(5000, token);
            return await _context.Set<T>().AsNoTracking().ToListAsync(token);
        }

        public async Task<T> GetByIdAsync(Guid id, CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                throw new OperationCanceledException("GetById operation was canceled.");
            }
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity != null) _context.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        public T Add(T entity, CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                throw new OperationCanceledException("Add operation was canceled.");
            }
            return _context.Set<T>().Add(entity).Entity;
        }

        public T Update(T entity, CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                throw new OperationCanceledException("Add operation was canceled.");
            }
            _context.Entry(entity).State = EntityState.Modified;
            return entity;
        }
    }
}
