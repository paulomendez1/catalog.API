using Catalog.Domain.Contracts.Persistence;
using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Persistence.Repositories
{
    public class ItemRepository : BaseRepository<Item>, IItemRepository
    {
        public IUnitOfWork UnitOfWork;
        public ItemRepository(CatalogContext context) : base(context)
        {
            UnitOfWork = context;
        }

        public async Task<IEnumerable<Item>> GetActiveItemsAsync(CancellationToken token)
        {
            return await _context.Items.Where(item => !item.IsInactive)
                                            .ToListAsync(token);
        }

        public async Task<IEnumerable<Item>> GetItemByArtistIdAsync(Guid id, CancellationToken token)
        {
            var items = await _context.Items.Where(item => !item.IsInactive)
                                            .Where(item => item.ArtistId == id)
                                            .Include(x => x.Genre)
                                            .Include(x => x.Artist)
                                            .ToListAsync(token);
            return items;

        }

        public async Task<IEnumerable<Item>> GetItemByGenreIdAsync(Guid id, CancellationToken token)
        {
            var items = await _context.Items.Where(item => !item.IsInactive)
                                            .Where(item => item.GenreId == id)
                                            .Include(x => x.Genre)
                                            .Include(x => x.Artist)
                                            .ToListAsync(token);
            return items;
        }

        public async Task<Item> GetItemWithSub(Guid id, CancellationToken token)
        {
            var item = await _context.Items.AsNoTracking()
                                             .Where(x => x.Id == id)
                                             .Include(x => x.Genre)
                                             .Include(x =>
                                            x.Artist).FirstOrDefaultAsync(token);
            return item;

        }
    }
}
