using Catalog.Domain.DTOs.Request.Artist;
using Catalog.Domain.DTOs.Request.Genre;
using Catalog.Domain.DTOs.Request.Item;
using Catalog.Domain.DTOs.Response;
using Catalog.Domain.Entities;
using Catalog.Domain.Services;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.DTOs.Request.Validators
{
    public class AddItemRequestValidator : AbstractValidator<AddItemRequest>
    {
        private readonly IArtistService _artistService;
        private readonly IGenreService _genreService;
        public AddItemRequestValidator(IArtistService artistService, IGenreService genreService)
        {
            _artistService = artistService;
            _genreService = genreService;
            RuleFor(x => x.GenreId).NotEmpty().MustAsync(GenreExists).WithMessage("Genre must exists");
            RuleFor(x => x.ArtistId).NotEmpty().MustAsync(ArtistExists).WithMessage("Artist must exists");
            RuleFor(x => x.Price).NotEmpty();
            RuleFor(x => x.ReleaseDate).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Price).Must(x => x?.Amount > 0);
            RuleFor(x => x.AvailableStock).Must(x => x > 0);

        }

        private async Task<bool> ArtistExists(Guid artistId, CancellationToken token)
        {
            if (string.IsNullOrEmpty(artistId.ToString())) return false;
            var artist = await GetArtistAsync(artistId);
            return artist != null;
        }
        private async Task<bool> GenreExists(Guid genreId, CancellationToken token)
        {
            if (string.IsNullOrEmpty(genreId.ToString())) return false;
            var genre = await GetGenreAsync(genreId);
            return genre != null;
        }        private async Task<ArtistResponse> GetArtistAsync(Guid artistId, CancellationToken token = default)
        {
            return await _artistService.GetArtistAsync(new GetArtistRequest { ArtistId = artistId }, token);;
        }

        private async Task<GenreResponse> GetGenreAsync(Guid genreId, CancellationToken token = default)
        {
            return await _genreService.GetGenreAsync(new GetGenreRequest { GenreId = genreId }, token);
        }
    }
}
