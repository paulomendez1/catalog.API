using Catalog.Domain.DTOs.Request.Artist;
using Catalog.Domain.DTOs.Request.Genre;
using Catalog.Domain.DTOs.Request.Item;
using Catalog.Domain.Services;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.DTOs.Request.Validators
{
    public class EditItemRequestValidator : AbstractValidator<EditItemRequest>
    {
        private readonly IArtistService _artistService;
        private readonly IGenreService _genreService;
        public EditItemRequestValidator(IArtistService artistService, IGenreService genreService)
        {
            _artistService = artistService;
            _genreService = genreService;
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.GenreId).NotEmpty().MustAsync(GenreExists).WithMessage("Genre must exists");
            RuleFor(x => x.ArtistId).NotEmpty().MustAsync(ArtistExists).WithMessage("Artist must exists");
            RuleFor(x => x.Price).NotEmpty();
            RuleFor(x => x.Price).Must(x => x?.Amount > 0);
            RuleFor(x => x.ReleaseDate).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
        }

        private async Task<bool> ArtistExists(Guid artistId, CancellationToken token)
        {
            if (string.IsNullOrEmpty(artistId.ToString())) return false;
            var artist = await _artistService.GetArtistAsync(new GetArtistRequest
            {
                ArtistId = artistId
            }, token);
            return artist != null;
        }
        private async Task<bool> GenreExists(Guid genreId, CancellationToken token)
        {
            if (string.IsNullOrEmpty(genreId.ToString())) return false;
            var genre = await _genreService.GetGenreAsync(new GetGenreRequest
            {
                GenreId = genreId
            }, token);
            return genre != null;
        }
    }
}
