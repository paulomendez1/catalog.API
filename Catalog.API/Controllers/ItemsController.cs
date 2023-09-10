using Catalog.API.Filters;
using Catalog.API.ResponseModels;
using Catalog.Domain.Configurations;
using Catalog.Domain.Contracts.Persistence;
using Catalog.Domain.DTOs.Request;
using Catalog.Domain.DTOs.Request.Item;
using Catalog.Domain.DTOs.Response;
using Catalog.Domain.Entities;
using Catalog.Domain.Services;
using Catalog.Persistence.Extensions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RiskFirst.Hateoas;

namespace Catalog.API.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/items")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _itemService;
        private readonly ILogger<ItemsController> _logger;
        private readonly ILinksService _linksService;
        public ItemsController(IItemService itemService, ILogger<ItemsController> logger,
                               ILinksService linksService)
        {
            _itemService = itemService;
            _logger = logger;
            _linksService = linksService;
        }

        [HttpGet]

        public async Task<IActionResult> Get([FromQuery] GenericQueryFilter queryFilter)
        {
            _logger.LogInformation("Getting items...");

            var items = await _itemService.GetItemsAsync(queryFilter);

            var paginationMetadata = new
            {
                totalCount = items.TotalCount,
                pageSize = items.PageSize,
                currentPage = items.CurrentPage,
                totalPages = items.TotalPages
            };

            HttpContext.Response.Headers.Append("x-pagination", JsonConvert.SerializeObject(paginationMetadata));

            return Ok(items);
        }

        [HttpGet("{id:guid}")]
        [ItemExists]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogInformation($"Getting item with ID:{id}...");
            var result = await _itemService.GetItemAsync(new GetItemRequest { Id = id });
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(AddItemRequest request)
        {
            _logger.LogInformation("Creating item...");
            var result = await _itemService.AddItemAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, null);
        }

        [HttpPut("{id:guid}")]
        [ItemExists]
        public async Task<IActionResult> Put(Guid id, EditItemRequest request)
        {
            _logger.LogInformation($"Updating item with ID: {id}");
            request.Id = id;
            var result = await _itemService.EditItemAsync(request);
            return Ok(result);
        }        [HttpDelete("{id:guid}")]
        [ItemExists]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation($"Deleting item with ID: {id}");
            if (id == Guid.Empty)
            { 
                return BadRequest();
            }
            var request = new DeleteItemRequest { Id = id };
            var item = await _itemService.DeleteItemAsync(request);
            return NoContent();
        }
    }
}
