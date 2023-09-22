using Catalog.API.Filters;
using Catalog.API.ResponseModels;
using Catalog.Domain.Configurations;
using Catalog.Domain.DTOs.Request.Artist;
using Catalog.Domain.DTOs.Response;
using Catalog.Domain.Entities;
using Catalog.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        public async Task<IActionResult> Get([FromQuery] GenericQueryFilter queryFilter, CancellationToken token)
        {
            _logger.LogInformation("Getting artists...");
            var artists = await _artistService.GetArtistsAsync(queryFilter, token);

            var paginationMetadata = new
            {
                totalCount = artists.TotalCount,
                pageSize = artists.PageSize,
                currentPage = artists.CurrentPage,
                totalPages = artists.TotalPages
            };

            HttpContext.Response.Headers.Append("x-pagination", JsonConvert.SerializeObject(paginationMetadata));

            return Ok(artists);
        }
        [HttpGet("{id:guid}")]
        [ArtistExists]
        public async Task<IActionResult> GetById(Guid id, CancellationToken token)
        {
            _logger.LogInformation($"Getting artist with ID: {id}...");

            var result = await _artistService.GetArtistAsync(new GetArtistRequest
           {
               ArtistId = id
           }, token);
           return Ok(result);
        }

        [HttpGet("{id:guid}/items")]
        [ArtistExists]
        public async Task<IActionResult> GetItemsById(Guid id, CancellationToken token)
        {
            _logger.LogInformation($"Getting items with artist ID {id}...");
            var result = await _artistService.GetItemByArtistIdAsync(new GetArtistRequest
           {
               ArtistId = id
           }, token);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Post(AddArtistRequest request, CancellationToken token)
        {
            _logger.LogInformation("Creating an artist...");
            var result = await _artistService.AddArtistAsync(request, token);
            return CreatedAtAction(nameof(GetById), new { id = result.ArtistId }, null);
        }
    }

}

