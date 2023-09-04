using Catalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Contracts.Persistence
{
    public interface IItemRepository : IRepository<Item>
    {
        Task<Item> GetItemWithSub(Guid id);
        Task<IEnumerable<Item>> GetItemByArtistIdAsync(Guid id);
        Task<IEnumerable<Item>> GetItemByGenreIdAsync(Guid id);
        Task<IEnumerable<Item>> GetActiveItemsAsync();
    }
}
