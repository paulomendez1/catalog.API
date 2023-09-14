using Catalog.API.Filters;
using Catalog.API.ResponseModels;
using Catalog.Domain.Configurations;
using Catalog.Domain.Contracts.Persistence;
using Catalog.Domain.DTOs.Request.Artist;
using Catalog.Domain.DTOs.Request.Genre;
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
        public async Task<IActionResult> Get([FromQuery] GenericQueryFilter queryFilter)
        {
            _logger.LogInformation("Getting genres...");
            var genres = await _genreService.GetGenresAsync(queryFilter);

            var paginationMetadata = new
            {
                totalCount = genres.TotalCount,
                pageSize = genres.PageSize,
                currentPage = genres.CurrentPage,
                totalPages = genres.TotalPages
            };

            HttpContext.Response.Headers.Append("x-pagination", JsonConvert.SerializeObject(paginationMetadata));

            return Ok(genres);
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

