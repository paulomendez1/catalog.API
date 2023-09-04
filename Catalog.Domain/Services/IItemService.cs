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
        Task<IEnumerable<ItemResponse>> GetItemsAsync();
        Task<ItemResponse> GetItemAsync(GetItemRequest request);
        Task<ItemResponse> AddItemAsync(AddItemRequest request);
        Task<ItemResponse> EditItemAsync(EditItemRequest request);
        Task<ItemResponse> DeleteItemAsync(DeleteItemRequest request);
    }
}
