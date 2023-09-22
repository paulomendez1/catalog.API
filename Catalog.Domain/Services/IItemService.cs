using Catalog.Domain.Configurations;
using Catalog.Domain.DTOs.Request.Item;
using Catalog.Domain.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Services
{
    public interface IItemService
    {
        Task<PagedList<ItemResponse>> GetItemsAsync(GenericQueryFilter queryFilter, CancellationToken token);
        Task<ItemResponse> GetItemAsync(GetItemRequest request, CancellationToken token);
        Task<ItemResponse> AddItemAsync(AddItemRequest request, CancellationToken token);
        Task<ItemResponse> EditItemAsync(EditItemRequest request, CancellationToken token);
        Task<ItemResponse> DeleteItemAsync(DeleteItemRequest request, CancellationToken token);
    }
}
