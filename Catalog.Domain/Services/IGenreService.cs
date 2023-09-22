using Catalog.Domain.Configurations;
using Catalog.Domain.DTOs.Request.Genre;
using Catalog.Domain.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Services
{
    public interface IGenreService
    {
        Task<PagedList<GenreResponse>> GetGenresAsync(GenericQueryFilter queryFilter, CancellationToken token);
        Task<GenreResponse> GetGenreAsync(GetGenreRequest request, CancellationToken token);
        Task<IEnumerable<ItemResponse>> GetItemByGenreIdAsync(GetGenreRequest request, CancellationToken token);
        Task<GenreResponse> AddGenreAsync(AddGenreRequest request, CancellationToken token);
    }
}
