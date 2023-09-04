using AutoMapper;
using Catalog.Domain.DTOs.Request;
using Catalog.Domain.DTOs.Request.Artist;
using Catalog.Domain.DTOs.Request.Genre;
using Catalog.Domain.DTOs.Request.Item;
using Catalog.Domain.DTOs.Request.Validators;
using Catalog.Domain.DTOs.Response;
using Catalog.Domain.Entities;
using Catalog.Domain.Services;
using Catalog.Infrastructure.Tests.Fixtures;
using Catalog.Persistence.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace Catalog.Domain.Tests
{
    public class ItemServiceTests : IClassFixture<CatalogContextFactory>
    {
        private readonly ItemRepository _itemRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<AddItemRequest> _addItemRequest;
        private readonly IValidator<EditItemRequest> _editItemRequest;
        private readonly Mock<IArtistService> _artistServiceMock;
        private readonly Mock<IGenreService> _genreServiceMock;
        private readonly IItemService _itemService;
        private readonly IMemoryCache _memoryCache;
        public ItemServiceTests(CatalogContextFactory catalogContextFactory)
        {
            _itemRepository = new ItemRepository(catalogContextFactory._dbContext);
            _mapper = catalogContextFactory._mapper;
            _artistServiceMock = new Mock<IArtistService>();
            _artistServiceMock.Setup(x => x.GetArtistAsync(It.IsAny<GetArtistRequest>()))
                                           .ReturnsAsync(() => new ArtistResponse());
            _genreServiceMock = new Mock<IGenreService>();
            _genreServiceMock.Setup(x => x.GetGenreAsync(It.IsAny<GetGenreRequest>()))
                                            .ReturnsAsync(() => new GenreResponse());
            _addItemRequest = new AddItemRequestValidator(_artistServiceMock.Object, _genreServiceMock.Object);
            _editItemRequest = new EditItemRequestValidator(_artistServiceMock.Object, _genreServiceMock.Object);
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
            _itemService = new ItemService(_itemRepository, _mapper, _addItemRequest, _editItemRequest, _memoryCache);
        }
        [Fact]
        public async void GetItemsAsync_Success()
        {

            //Act
            var result = await _itemService.GetItemsAsync();

            //Result
            Assert.NotNull(result);
        }


        [Theory]
        [InlineData("B5B05534-9263-448C-A69E-0BBD8B3EB90E")]
        public async Task GetItemAsync_ReturnItem(string guid)
        {
            //Act
            var result = await _itemService.GetItemAsync(new GetItemRequest { Id = new Guid(guid) });
            
            //Result
            Assert.Equal(new Guid(guid), result.Id);
        }

        [Fact]
        public async Task GetItemAsync_ThrowException()
        {
            //Result
            await Assert.ThrowsAsync<ArgumentNullException>(() => _itemService.GetItemAsync(null));
        }

        [Fact]
        public async Task AddItemAsync_Success()
        {
            //Arrange
            var testItem = new AddItemRequest
            {

                Name = "Test album",
                Price = new Price
                {
                    Amount = 13,
                    Currency = "EUR"
                },
                GenreId = new Guid("c04f05c0-f6ad-44d1-a400-3375bfb5dfd6"),
                ArtistId = new Guid("f08a333d-30db-4dd1-b8ba-3b0473c7cdab"),
                Description = "Description",
                Format = "Format",
                LabelName = "Label name",
                PictureUri = "",
                ReleaseDate = DateTime.Now,
                AvailableStock = 3
            };

            //Act
            var result = await _itemService.AddItemAsync(testItem);

            Assert.Equal(testItem.Name, result.Name);
            Assert.Equal(testItem.Description, result.Description);
            Assert.Equal(testItem.GenreId, result.GenreId);
            Assert.Equal(testItem.ArtistId, result.ArtistId);
            Assert.Equal(testItem.Price.Amount, result.Price.Amount);
            Assert.Equal(testItem.Price.Currency, result.Price.Currency);
        }

        [Fact]
        public async Task EditItemAsync_Success()
        {
            //Arrange
            var testItem = new EditItemRequest
            {
                Id = new Guid("b5b05534-9263-448c-a69e-0bbd8b3eb90e"),
                Name = "Test album",
                Price = new Price
                {
                    Amount = 13,
                    Currency = "EUR"
                },
                GenreId = new Guid("c04f05c0-f6ad-44d1-a400-3375bfb5dfd6"),
                ArtistId = new Guid("f08a333d-30db-4dd1-b8ba-3b0473c7cdab"),
                Description = "Description",
                Format = "Format",
                LabelName = "Label name",
                PictureUri = "",
                ReleaseDate = DateTime.Now,
                AvailableStock = 3
            };

            //Act
            var result = await _itemService.EditItemAsync(testItem);

            Assert.Equal(testItem.Name, result.Name);
            Assert.Equal(testItem.Description, result.Description);
            Assert.Equal(testItem.GenreId, result.GenreId);
            Assert.Equal(testItem.ArtistId, result.ArtistId);
            Assert.Equal(testItem.Price.Amount, result.Price.Amount);
            Assert.Equal(testItem.Price.Currency, result.Price.Currency);
        }

    }
}