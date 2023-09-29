using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Configurations
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }

        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public string SortOrder { get; private set; }
        public string SortColumn { get; private set; }  

        public bool HasPrevious => (CurrentPage > 1);
        public bool HasNext => (CurrentPage < TotalPages);

        public PagedList(List<T> items, int count, int pageNumber, int pageSize, string sortColumn, string sortOrder)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            SortColumn = sortColumn;
            SortOrder = sortOrder;
            AddRange(items);
        }
        public static PagedList<T> Create(IEnumerable<T> source, int pageNumber, int pageSize, string sortColumn, string sortOrder)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PagedList<T>(items, count, pageNumber, pageSize, sortColumn, sortOrder);
        }
    }
}
