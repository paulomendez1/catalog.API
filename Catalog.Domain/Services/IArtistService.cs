using Catalog.Domain.Configurations;
using Catalog.Domain.DTOs.Request.Artist;
using Catalog.Domain.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Services
{
    public interface IArtistService
    {
        Task<PagedList<ArtistResponse>> GetArtistsAsync(GenericQueryFilter queryFilter, CancellationToken token);
        Task<ArtistResponse> GetArtistAsync(GetArtistRequest request, CancellationToken token);
        Task<IEnumerable<ItemResponse>> GetItemByArtistIdAsync(GetArtistRequest request, CancellationToken token);
        Task<ArtistResponse> AddArtistAsync(AddArtistRequest request, CancellationToken token);
    }
}
