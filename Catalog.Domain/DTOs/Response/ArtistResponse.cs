using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.DTOs.Response
{
    public class ArtistResponse
    {
        public Guid ArtistId { get; set; }
        public string ArtistName { get; set; }
    }
}
