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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public async Task<ItemResponse> AddItemAsync(AddItemRequest request)
        {
            ValidationResult validationResult = await _addItemValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var validationMessage = string.Join(",", validationResult.Errors.Select(x=> x.ErrorMessage));
                throw new Exception(validationMessage);
            }
            var item = _mapper.Map<Item>(request);
            var result = _itemRepository.Add(item);
            await _itemRepository.UnitOfWork.SaveChangesAsync();
            return _mapper.Map<ItemResponse>(result);
        }

        public async Task<ItemResponse> DeleteItemAsync(DeleteItemRequest request)
        {
            if (request?.Id == null) throw new InvalidOperationException("");
            var result = await _itemRepository.GetByIdAsync(request.Id);
            if (result == null) return null;
            result.IsInactive = true;
            _itemRepository.Update(result);
            await _itemRepository.UnitOfWork.SaveChangesAsync();
            return _mapper.Map<ItemResponse>(result);
        }

        public async Task<ItemResponse> EditItemAsync(EditItemRequest request)
        {
            var existingRecord = await _itemRepository.GetByIdAsync(request.Id);
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
            var result = _itemRepository.Update(entity);
            await _itemRepository.UnitOfWork.SaveChangesAsync();
            return _mapper.Map<ItemResponse>(result);
        }

        public async Task<ItemResponse> GetItemAsync(GetItemRequest request)
        {
            var cacheData = _memoryCache.Get<ItemResponse>(CacheKeyEnum.Item);

            if (cacheData != null)
            {
                return cacheData;
            }

            if (request?.Id == null) throw new ArgumentNullException();

            var entity = await _itemRepository.GetByIdAsync(request.Id);
            var mappedEntity = _mapper.Map<ItemResponse>(entity);
            _memoryCache.Set<ItemResponse>(CacheKeyEnum.Item, mappedEntity, EXPIRATION_DATE);
            return mappedEntity;
        }

        public async Task<IEnumerable<ItemResponse>> GetItemsAsync()
        {
            var cacheData = _memoryCache.Get<IEnumerable<ItemResponse>>(CacheKeyEnum.Items);

            if(cacheData != null && cacheData.Count() > 0)
            {
                return cacheData;
            }

            var result = await _itemRepository.GetActiveItemsAsync();

            var mappedResult = _mapper.Map<List<ItemResponse>>(result);
            _memoryCache.Set<IEnumerable<ItemResponse>>(CacheKeyEnum.Items, mappedResult, EXPIRATION_DATE);
            return mappedResult;
        }
    }
}
