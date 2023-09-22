using AutoMapper;
using Catalog.Domain.Configurations;
using Catalog.Domain.Contracts.Persistence;
using Catalog.Domain.DTOs.Request.Artist;
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
    public class ArtistService : IArtistService
    {
        private readonly IArtistRepository _artistRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private DateTimeOffset EXPIRATION_DATE = DateTimeOffset.Now.AddSeconds(60);
        public ArtistService(IArtistRepository artistRepository, IMapper mapper, 
                            IItemRepository itemRepository, IMemoryCache memoryCache)
        {
            _artistRepository = artistRepository;
            _mapper = mapper;
            _itemRepository = itemRepository;
            _memoryCache = memoryCache;
        }
        public async Task<ArtistResponse> AddArtistAsync(AddArtistRequest request, CancellationToken token)
        {
            var artist = new Artist
            {
                ArtistName = request.ArtistName
            };
            var result = _artistRepository.Add(artist, token);
            await _artistRepository.UnitOfWork.SaveChangesAsync();
            _memoryCache.Remove(CacheKeyEnum.Artists);
            return _mapper.Map<ArtistResponse>(result);
        }

        public async Task<ArtistResponse> GetArtistAsync(GetArtistRequest request, CancellationToken token)
        {
            var cacheData = _memoryCache.Get<ArtistResponse>(CacheKeyEnum.Artist);

            if (cacheData != null)
            {
                return cacheData;
            }

            if (request?.ArtistId == null) throw new ArgumentNullException();
            var artist = await _artistRepository.GetByIdAsync(request.ArtistId, token);
            var mappedArtist = artist == null ? null : _mapper.Map<ArtistResponse>(artist);
            _memoryCache.Set<ArtistResponse>(CacheKeyEnum.Artist, mappedArtist, EXPIRATION_DATE);
            return mappedArtist;
        }

        public async Task<PagedList<ArtistResponse>> GetArtistsAsync(GenericQueryFilter queryFilter, CancellationToken token)
        {
            var cacheData = _memoryCache.Get<PagedList<ArtistResponse>>(CacheKeyEnum.Artists);
            
            if (cacheData != null && cacheData.Count() > 0
                       && cacheData.CurrentPage == queryFilter.PageNumber && cacheData.PageSize == queryFilter.PageSize)
            {
                return cacheData;
            }

            var result = await _artistRepository.GetAllAsync(token);

            if (!string.IsNullOrWhiteSpace(queryFilter.SearchQuery))
            {
                queryFilter.SearchQuery = queryFilter.SearchQuery.Trim();
                result = result.Where(x => x.ArtistName.Contains(queryFilter.SearchQuery));
            }
            result = result.OrderByDescending(x => x.ArtistName);

            var mappedResult = _mapper.Map<List<ArtistResponse>>(result);
            var paginatedResult = PagedList<ArtistResponse>.Create(mappedResult, queryFilter.PageNumber, queryFilter.PageSize);
            _memoryCache.Set<IEnumerable<ArtistResponse>>(CacheKeyEnum.Artists, paginatedResult, EXPIRATION_DATE);
            return paginatedResult;
        }

        public async Task<IEnumerable<ItemResponse>> GetItemByArtistIdAsync(GetArtistRequest request, CancellationToken token)
        {
            var cacheData = _memoryCache.Get<IEnumerable<ItemResponse>>(CacheKeyEnum.Items);

            if (cacheData != null && cacheData.Count() > 0)
            {
                return cacheData;
            }

            if (request?.ArtistId == null) throw new ArgumentNullException();
            var items = await _itemRepository.GetItemByArtistIdAsync(request.ArtistId, token);
            var mappedItems = _mapper.Map<IEnumerable<ItemResponse>>(items);
            _memoryCache.Set<IEnumerable<ItemResponse>>(CacheKeyEnum.Items, mappedItems, EXPIRATION_DATE);
            return mappedItems;
        }
    }
}
