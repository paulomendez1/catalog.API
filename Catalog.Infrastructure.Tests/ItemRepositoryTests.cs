using AutoFixture;
using Catalog.Domain.Entities;
using Catalog.Infrastructure.Tests.Fixtures;
using Catalog.Persistence;
using Catalog.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Tests
{
    public class ItemRepositoryTests : IClassFixture<CatalogContextFactory>
    {
        private readonly TestCatalogContext _dbContext;
        private readonly ItemRepository _itemRepository;

        public ItemRepositoryTests(CatalogContextFactory catalogContextFactory)
        {
            _dbContext = catalogContextFactory._dbContext;
            _itemRepository = new ItemRepository(_dbContext);
        }
        [Fact]
        public async void GetAllAsync_Success()
        {
            // Act
            var result = await _itemRepository.GetAllAsync();

            // Result
            Assert.IsAssignableFrom<IEnumerable<Item>>(result);
            Assert.NotNull(result);
        }

        [Fact]
        public async void GetByIdAsync_ReturnsNull()
        {
            // Act
            var result = await _itemRepository.GetByIdAsync(new Guid());

            // Result
            Assert.Null(result);
        }

        [Theory]
        [InlineData("B5B05534-9263-448C-A69E-0BBD8B3EB90E")]
        public async Task GetByIdAsync_ReturnItem(Guid guid)
        {
            // Act
            var result = await _itemRepository.GetByIdAsync(guid);

            // Result
            Assert.Equal(result.Id, guid);
        }

        [Fact]
        public async Task Add_Success()
        {
            //Arrange
            var testItem = new Item
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
                PictureUri = ""
            };

            // Act
            _itemRepository.Add(testItem);
            await _dbContext.SaveEntitiesAsync();

            // Result
            var itemAdded = _dbContext.Items.FirstOrDefault(_ => _.Id == testItem.Id);
            Assert.NotNull(itemAdded);
        }

        [Theory]
        [InlineData("f5da5ce4-091e-492e-a70a-22b073d75a52")]
        public async Task GetActiveItemsAsync_DoesNotContainInactiveRecords(string id)
        {
            var result = await _itemRepository.GetActiveItemsAsync();
            Assert.False(result.Any(x => x.Id == new Guid(id)));
        }
    }
}
