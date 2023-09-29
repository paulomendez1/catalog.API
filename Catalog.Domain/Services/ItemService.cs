using AutoMapper;
using Catalog.Domain.Configurations;
using Catalog.Domain.Contracts.Persistence;
using Catalog.Domain.DTOs.Request.Item;
using Catalog.Domain.DTOs.Response;
using Catalog.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace Catalog.Domain.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<AddItemRequest> _addItemValidator;
        private readonly IValidator<EditItemRequest> _editItemValidator;
        private readonly IMemoryCache _memoryCache;
        private DateTimeOffset EXPIRATION_DATE = DateTimeOffset.Now.AddSeconds(60);
        public ItemService(IItemRepository itemRepository, IMapper mapper,
                           IValidator<AddItemRequest> addItemValidator,
                           IValidator<EditItemRequest> editItemValidator,
                           IMemoryCache memoryCache)
        {
            _itemRepository = itemRepository;
            _mapper = mapper;
            _addItemValidator = addItemValidator;
            _editItemValidator = editItemValidator;
            _memoryCache = memoryCache;
        }
        public async Task<ItemResponse> AddItemAsync(AddItemRequest request, CancellationToken token)
        {
            ValidationResult validationResult = await _addItemValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var validationMessage = string.Join(",", validationResult.Errors.Select(x=> x.ErrorMessage));
                throw new Exception(validationMessage);
            }
            var item = _mapper.Map<Item>(request);
            var result = _itemRepository.Add(item, token);
            await _itemRepository.UnitOfWork.SaveChangesAsync();
            _memoryCache.Remove(CacheKeyEnum.Items);
            return _mapper.Map<ItemResponse>(result);
        }

        public async Task<ItemResponse> DeleteItemAsync(DeleteItemRequest request, CancellationToken token)
        {
            if (request?.Id == null) throw new InvalidOperationException("");
            var result = await _itemRepository.GetByIdAsync(request.Id, token);
            if (result == null) return null;
            result.IsInactive = true;
            _itemRepository.Update(result, token);
            await _itemRepository.UnitOfWork.SaveChangesAsync();
            _memoryCache.Remove(CacheKeyEnum.Items);
            return _mapper.Map<ItemResponse>(result);
        }

        public async Task<ItemResponse> EditItemAsync(EditItemRequest request, CancellationToken token)
        {
            var existingRecord = await _itemRepository.GetByIdAsync(request.Id, token);
            if (existingRecord == null)
            {
                throw new ArgumentException($"Entity with { request.Id } is not present");
            }
            ValidationResult validationResult = await _editItemValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var validationMessage = string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new Exception(validationMessage);
            }
            var entity = _mapper.Map<Item>(request);
            var result = _itemRepository.Update(entity, token);
            await _itemRepository.UnitOfWork.SaveChangesAsync();
            _memoryCache.Remove(CacheKeyEnum.Items);
            _memoryCache.Remove(CacheKeyEnum.Item);
            return _mapper.Map<ItemResponse>(result);
        }

        public async Task<ItemResponse> GetItemAsync(GetItemRequest request, CancellationToken token)
        {
            var cacheData = _memoryCache.Get<ItemResponse>(CacheKeyEnum.Item);

            if (cacheData != null)
            {
                return cacheData;
            }

            if (request?.Id == null) throw new ArgumentNullException();

            var entity = await _itemRepository.GetByIdAsync(request.Id, token);
            var mappedEntity = _mapper.Map<ItemResponse>(entity);
            _memoryCache.Set<ItemResponse>(CacheKeyEnum.Item, mappedEntity, EXPIRATION_DATE);
            return mappedEntity;
        }

        public async Task<PagedList<ItemResponse>> GetItemsAsync(GenericQueryFilter queryFilter, CancellationToken token)
        {
            var cacheData = _memoryCache.Get<PagedList<ItemResponse>>(CacheKeyEnum.Items);
            if (cacheData != null && cacheData.Count() > 0
                && cacheData.CurrentPage == queryFilter.PageNumber && 
                cacheData.PageSize == queryFilter.PageSize
                && cacheData.SortOrder == queryFilter.SortOrder
                && cacheData.SortColumn == queryFilter.SortColumn)
            {
                return cacheData;
            }

            var result = await _itemRepository.GetActiveItemsAsync(token);

            if (!string.IsNullOrWhiteSpace(queryFilter.SearchQuery))
            {
                queryFilter.SearchQuery = queryFilter.SearchQuery.Trim();
                result = result.Where(x => x.Description.Contains(queryFilter.SearchQuery) || x.Name.Contains(queryFilter.SearchQuery) || x.LabelName.Contains(queryFilter.SearchQuery));
            }
            
            if (!string.IsNullOrEmpty(queryFilter.SortColumn))
            {
                queryFilter.SortOrder = !string.IsNullOrEmpty(queryFilter.SortOrder)
                        && queryFilter.SortOrder.ToUpper() == "ASC" ? "ASC" : "DESC";
                result = result.AsQueryable().OrderBy(
                string.Format("{0} {1}", queryFilter.SortColumn, queryFilter.SortOrder));
            }

            var mappedResult = _mapper.Map<List<ItemResponse>>(result);
            var paginatedResult = PagedList<ItemResponse>.Create(mappedResult, queryFilter.PageNumber, queryFilter.PageSize, queryFilter.SortColumn, queryFilter.SortOrder);
            _memoryCache.Set<PagedList<ItemResponse>>(CacheKeyEnum.Items, paginatedResult, EXPIRATION_DATE);
            return paginatedResult;
        }
    }
}
