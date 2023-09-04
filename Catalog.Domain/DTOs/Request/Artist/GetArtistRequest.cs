using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.DTOs.Request.Artist
{
    public class GetArtistRequest
    {
        public Guid ArtistId { get; set; }
    }
}
