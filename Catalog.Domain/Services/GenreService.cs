using AutoMapper;
using Catalog.Domain.Configurations;
using Catalog.Domain.Contracts.Persistence;
using Catalog.Domain.DTOs.Request.Genre;
using Catalog.Domain.DTOs.Response;
using Catalog.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Services
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _genreRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private DateTimeOffset EXPIRATION_DATE = DateTimeOffset.Now.AddSeconds(60);
        public GenreService(IGenreRepository genreRepository, IMapper mapper, 
                            IItemRepository itemRepository, IMemoryCache memoryCache)
        {
            _genreRepository = genreRepository;
            _mapper = mapper;
            _itemRepository = itemRepository;
            _memoryCache = memoryCache;
        }
        public async Task<GenreResponse> AddGenreAsync(AddGenreRequest request, CancellationToken token)
        {
            var Genre = new Genre
            {
                GenreDescription = request.GenreDescription
            };
            var result = _genreRepository.Add(Genre, token);
            await _genreRepository.UnitOfWork.SaveChangesAsync();
            _memoryCache.Remove(CacheKeyEnum.Genres);
            return _mapper.Map<GenreResponse>(result);
        }

        public async Task<PagedList<GenreResponse>> GetGenresAsync(GenericQueryFilter queryFilter, CancellationToken token)
        {
            var cacheData = _memoryCache.Get<PagedList<GenreResponse>>(CacheKeyEnum.Genres);

            if (cacheData != null && cacheData.Count() > 0
                && cacheData.CurrentPage == queryFilter.PageNumber && cacheData.PageSize == queryFilter.PageSize)
            {
                return cacheData;
            }

            var result = await _genreRepository.GetAllAsync(token);

            if (!string.IsNullOrWhiteSpace(queryFilter.SearchQuery))
            {
                queryFilter.SearchQuery = queryFilter.SearchQuery.Trim();
                result = result.Where(x => x.GenreDescription.Contains(queryFilter.SearchQuery));
            }
            result = result.OrderByDescending(x => x.GenreDescription);

            var mappedResult = _mapper.Map<List<GenreResponse>>(result);
            var paginatedResult = PagedList<GenreResponse>.Create(mappedResult, queryFilter.PageNumber, queryFilter.PageSize);
            _memoryCache.Set<PagedList<GenreResponse>>(CacheKeyEnum.Genres, paginatedResult, EXPIRATION_DATE);
            return paginatedResult;
        }

        public async Task<GenreResponse> GetGenreAsync(GetGenreRequest request, CancellationToken token)
        {
            var cacheData = _memoryCache.Get<GenreResponse>(CacheKeyEnum.Genre);

            if (cacheData != null)
            {
                return cacheData;
            }

            if (request?.GenreId == null) throw new ArgumentNullException();
            var genre = await _genreRepository.GetByIdAsync(request.GenreId, token);
            var mappedGenre = _mapper.Map<GenreResponse>(genre);

            _memoryCache.Set<GenreResponse>(CacheKeyEnum.Genre, mappedGenre, EXPIRATION_DATE);
            return mappedGenre;
        }

        public async Task<IEnumerable<ItemResponse>> GetItemByGenreIdAsync(GetGenreRequest request, CancellationToken token)
        {
            var cacheData = _memoryCache.Get<IEnumerable<ItemResponse>>(CacheKeyEnum.Items);

            if (cacheData != null && cacheData.Count() > 0)
            {
                return cacheData;
            }

            if (request?.GenreId == null) throw new ArgumentNullException();
            var items = await _itemRepository.GetItemByGenreIdAsync(request.GenreId, token);
            var mappedItems = _mapper.Map<IEnumerable<ItemResponse>>(items);

            _memoryCache.Set<IEnumerable<ItemResponse>>(CacheKeyEnum.Items, mappedItems, EXPIRATION_DATE);
            return mappedItems;
        }
    }
}
