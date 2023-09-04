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
        public async Task<ArtistResponse> AddArtistAsync(AddArtistRequest request)
        {
            var artist = new Artist
            {
                ArtistName = request.ArtistName
            };
            var result = _artistRepository.Add(artist);
            await _artistRepository.UnitOfWork.SaveChangesAsync();

            return _mapper.Map<ArtistResponse>(result);
        }

        public async Task<ArtistResponse> GetArtistAsync(GetArtistRequest request)
        {
            var cacheData = _memoryCache.Get<ArtistResponse>(CacheKeyEnum.Artist);

            if (cacheData != null)
            {
                return cacheData;
            }

            if (request?.ArtistId == null) throw new ArgumentNullException();
            var artist = await _artistRepository.GetByIdAsync(request.ArtistId);
            var mappedArtist = artist == null ? null : _mapper.Map<ArtistResponse>(artist);
            _memoryCache.Set<ArtistResponse>(CacheKeyEnum.Artist, mappedArtist, EXPIRATION_DATE);
            return mappedArtist;
        }

        public async Task<IEnumerable<ArtistResponse>> GetArtistsAsync()
        {
            var cacheData = _memoryCache.Get<IEnumerable<ArtistResponse>>(CacheKeyEnum.Artists);

            if (cacheData != null && cacheData.Count() > 0)
            {
                return cacheData;
            }

            var artists = await _artistRepository.GetAllAsync();
            var mappedArtists = _mapper.Map<IEnumerable<ArtistResponse>>(artists);
            _memoryCache.Set<IEnumerable<ArtistResponse>>(CacheKeyEnum.Artists, mappedArtists, EXPIRATION_DATE);
            return mappedArtists;
        }

        public async Task<IEnumerable<ItemResponse>> GetItemByArtistIdAsync(GetArtistRequest request)
        {
            var cacheData = _memoryCache.Get<IEnumerable<ItemResponse>>(CacheKeyEnum.Items);

            if (cacheData != null && cacheData.Count() > 0)
            {
                return cacheData;
            }

            if (request?.ArtistId == null) throw new ArgumentNullException();
            var items = await _itemRepository.GetItemByArtistIdAsync(request.ArtistId);
            var mappedItems = _mapper.Map<IEnumerable<ItemResponse>>(items);
            _memoryCache.Set<IEnumerable<ItemResponse>>(CacheKeyEnum.Items, mappedItems, EXPIRATION_DATE);
            return mappedItems;
        }
    }
}
