using Catalog.API.Filters;
using Catalog.API.ResponseModels;
using Catalog.Domain.DTOs.Request.Artist;
using Catalog.Domain.DTOs.Response;
using Catalog.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/artist")]
    [ApiController]
    public class ArtistController : ControllerBase
    {
        private readonly IArtistService _artistService;
        private readonly ILogger<ArtistController> _logger;
        public ArtistController(IArtistService artistService, ILogger<ArtistController> logger)
        {
            _artistService = artistService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
        {
            _logger.LogInformation("Getting artists...");
            var result = await _artistService.GetArtistsAsync();
            var totalItems = result.ToList().Count;
            var itemsOnPage = result.OrderBy(c => c.ArtistName)
                                    .Skip(pageSize * pageIndex)
                                    .Take(pageSize);
            var model = new PaginatedResponseModel<ArtistResponse>
                (pageIndex, pageSize, totalItems, itemsOnPage);
            return Ok(model);
        }
        [HttpGet("{id:guid}")]
        [ArtistExists]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogInformation($"Getting artist with ID: {id}...");

            var result = await _artistService.GetArtistAsync(new GetArtistRequest
           {
               ArtistId = id
           });
           return Ok(result);
        }

        [HttpGet("{id:guid}/items")]
        [ArtistExists]
        public async Task<IActionResult> GetItemsById(Guid id)
        {
            _logger.LogInformation($"Getting items with artist ID {id}...");
            var result = await _artistService.GetItemByArtistIdAsync(new GetArtistRequest
           {
               ArtistId = id
           });
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Post(AddArtistRequest request)
        {
            _logger.LogInformation("Creating an artist...");
            var result = await _artistService.AddArtistAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.ArtistId }, null);
        }
    }

}

