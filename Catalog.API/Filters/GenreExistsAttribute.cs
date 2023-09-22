using Catalog.Domain.DTOs.Request;
using Catalog.Domain.DTOs.Request.Genre;
using Catalog.Domain.DTOs.Request.Item;
using Catalog.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Catalog.API.Filters
{
    public class GenreExistsAttribute : TypeFilterAttribute
    {
        public GenreExistsAttribute() : base(typeof(GenreExistsFilterImpl)) { }
        public class GenreExistsFilterImpl : IAsyncActionFilter
        {
            private readonly IGenreService _genreService;
            private readonly ILogger<GenreExistsAttribute> _logger;
            public GenreExistsFilterImpl(IGenreService genreService, ILogger<GenreExistsAttribute> logger)
            {
                _genreService = genreService;
                _logger = logger;
            }
            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                _logger.LogInformation($"Checking if there is a Genre with the ID provided");
                if (!(context.ActionArguments["id"] is Guid id))
                {
                    _logger.LogError("No ID or incorrect ID provided for genre");
                    context.Result = new BadRequestResult();
                    return;
                }
                var result = await _genreService.GetGenreAsync(new GetGenreRequest { GenreId = id }, default(CancellationToken));
                if (result == null)
                {
                    _logger.LogError($"Genre with ID: {id} not exist.");
                    context.Result = new NotFoundObjectResult($"Genre with id {id} not exist.");
                    return;
                }
                await next();
            }
        }
    }

}
