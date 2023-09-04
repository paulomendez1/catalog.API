using Catalog.Domain.DTOs.Request;
using Catalog.Domain.DTOs.Request.Artist;
using Catalog.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Catalog.API.Filters
{
    public class ArtistExistsAttribute : TypeFilterAttribute
    {
        public ArtistExistsAttribute() : base(typeof(ArtistExistsFilterImpl)) { }
        public class ArtistExistsFilterImpl : IAsyncActionFilter
        {
            private readonly IArtistService _artistService;
            private readonly ILogger<ArtistExistsAttribute> _logger;
            public ArtistExistsFilterImpl(IArtistService ArtistService, ILogger<ArtistExistsAttribute> logger)
            {
                _artistService = ArtistService;
                _logger = logger;
            }
            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                _logger.LogInformation($"Checking if there is an Artist with the ID provided");
                if (!(context.ActionArguments["id"] is Guid id))
                {
                    _logger.LogError("No ID or incorrect ID provided for Artist");
                    context.Result = new BadRequestResult();
                    return;
                }
                var result = await _artistService.GetArtistAsync(new GetArtistRequest { ArtistId = id });
                if (result == null)
                {
                    _logger.LogError($"Artist with ID: {id} not exist.");
                    context.Result = new NotFoundObjectResult($"Artist with id {id} not exist.");
                    return;
                }
                await next();
            }
        }
    }

}
