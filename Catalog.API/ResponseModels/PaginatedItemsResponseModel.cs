namespace Catalog.API.ResponseModels
{
    public class PaginatedResponseModel<T> where T : class
    {
        public PaginatedResponseModel(int pageIndex, int pageSize, long total, IEnumerable<T> data)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Total = total;
            Data = data;
        }
        public int PageIndex { get; }
        public int PageSize { get; }
        public long Total { get; }
        public IEnumerable<T> Data { get; }
    }
}
