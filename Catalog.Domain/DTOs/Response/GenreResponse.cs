using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.DTOs.Response
{
    public class GenreResponse
    {
        public Guid GenreId { get; set; }
        public string GenreDescription { get; set; }
    }
}
