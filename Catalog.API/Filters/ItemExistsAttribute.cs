using Catalog.Domain.DTOs.Request;
using Catalog.Domain.DTOs.Request.Item;
using Catalog.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Catalog.API.Filters
{
    public class ItemExistsAttribute : TypeFilterAttribute
    {
        public ItemExistsAttribute() : base(typeof(ItemExistsFilterImpl)) { }
        public class ItemExistsFilterImpl : IAsyncActionFilter
        {
            private readonly IItemService _itemService;
            private readonly ILogger<ItemExistsAttribute> _logger;
            public ItemExistsFilterImpl(IItemService itemService, ILogger<ItemExistsAttribute> logger)
            {
                _itemService = itemService;
                _logger = logger;
            }
            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                _logger.LogInformation($"Checking if there is an Item with the ID provided");
                if (!(context.ActionArguments["id"] is Guid id))
                {
                    _logger.LogError("No ID or incorrect ID provided for item");
                    context.Result = new BadRequestResult();
                    return;
                }
                var result = await _itemService.GetItemAsync(new GetItemRequest { Id = id });
                if (result == null)
                {
                    _logger.LogError($"Item with ID: {id} not exist.");
                    context.Result = new NotFoundObjectResult($"Item with id {id} not exist.");
                    return;
                }
                await next();
            }
        }
    }

}
