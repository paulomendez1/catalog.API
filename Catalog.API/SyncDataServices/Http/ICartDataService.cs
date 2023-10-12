using Catalog.Domain.DTOs.Request.Item;
using Catalog.Domain.DTOs.Response;

namespace Catalog.API.SyncDataServices.Http
{
    public interface ICartDataService
    {
        Task SendItemToCart(ItemResponse item);
    }
}
