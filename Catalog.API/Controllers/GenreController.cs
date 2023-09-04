using Catalog.API.Filters;
using Catalog.API.ResponseModels;
using Catalog.Domain.Contracts.Persistence;
using Catalog.Domain.DTOs.Request.Artist;
using Catalog.Domain.DTOs.Request.Genre;
using Catalog.Domain.DTOs.Response;
using Catalog.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/genre")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly IGenreService _genreService;
        private readonly ILogger<GenreController> _logger;
        public GenreController(IGenreService genreService, ILogger<GenreController> logger)
        {
            _genreService = genreService;
            _logger = logger;

        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
        {
            _logger.LogInformation("Getting genres...");
            var result = await _genreService.GetGenresAsync();
            var totalItems = result.ToList().Count;
            var itemsOnPage = result.OrderBy(c => c.GenreDescription)
                                    .Skip(pageSize * pageIndex)
                                    .Take(pageSize);
            var model = new PaginatedResponseModel<GenreResponse>(
            pageIndex, pageSize, totalItems, itemsOnPage);
            return Ok(model);
        }
        [HttpGet("{id:guid}")]
        [GenreExists]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogInformation($"Getting genre with ID: {id}...");
            var result = await _genreService.GetGenreAsync(new GetGenreRequest
           {
                GenreId = id
           });
           return Ok(result);
        }

        [HttpGet("{id:guid}/items")]
        [GenreExists]
        public async Task<IActionResult> GetItemsById(Guid id)
        {
            _logger.LogInformation($"Getting items with genre ID: {id}...");
            var result = await _genreService.GetItemByGenreIdAsync(new GetGenreRequest
            {
                GenreId = id
            });
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Post(AddGenreRequest request)
        {
            _logger.LogInformation("Creating a genre...");
            var result = await _genreService.AddGenreAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.GenreId }, null);
        }
    }

}

