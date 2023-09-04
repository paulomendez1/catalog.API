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
        Task<IEnumerable<ArtistResponse>> GetArtistsAsync();
        Task<ArtistResponse> GetArtistAsync(GetArtistRequest request);
        Task<IEnumerable<ItemResponse>> GetItemByArtistIdAsync(GetArtistRequest request);
        Task<ArtistResponse> AddArtistAsync(AddArtistRequest request);
    }
}
