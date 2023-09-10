using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.DTOs.Response
{
    public class TokenResponse
    {
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
