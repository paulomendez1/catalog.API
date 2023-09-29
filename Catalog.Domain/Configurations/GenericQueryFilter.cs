using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Configurations
{
    public class GenericQueryFilter
    {
        const int maxPageSize = 50;
        public string? SearchQuery { get; set; }
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10;
        public string? SortColumn { get; set; }
        public string? SortOrder { get; set; }
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }

        public string? Fields { get; set; }
    }
}
