using Catalog.Domain.Entities;
using Catalog.Infrastructure.Tests.Fixtures;
using Catalog.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Tests
{
    public class ArtistRepositoryTests : IClassFixture<CatalogContextFactory>
    {
        private readonly TestCatalogContext _dbContext;
        private readonly ArtistRepository _artistRepository;

        public ArtistRepositoryTests(CatalogContextFactory catalogContextFactory)
        {
            _dbContext = catalogContextFactory._dbContext;
            _artistRepository = new ArtistRepository(_dbContext);
        }

        [Fact]
        public async void GetAllAsync_Success()
        {
            // Act
            var result = await _artistRepository.GetAllAsync(default(CancellationToken));

            // Result
            Assert.IsAssignableFrom<IEnumerable<Artist>>(result);
            Assert.NotNull(result);
        }

        [Fact]
        public async void GetByIdAsync_ReturnsNull()
        {
            // Act
            var result = await _artistRepository.GetByIdAsync(new Guid(), default(CancellationToken));

            // Result
            Assert.Null(result);
        }

        [Theory]
        [InlineData("3eb00b42-a9f0-4012-841d-70ebf3ab7474")]
        public async Task GetByIdAsync_ReturnItem(Guid guid)
        {
            // Act
            var result = await _artistRepository.GetByIdAsync(guid, default(CancellationToken));

            // Result
            Assert.Equal(result.ArtistId, guid);
        }

        [Fact]
        public async Task Add_Success()
        {
            //Arrange
            var testArtist = new Artist
            {
                ArtistName = "Test artist",
                Items = null
            };

            // Act
            _artistRepository.Add(testArtist, default(CancellationToken));
            await _dbContext.SaveEntitiesAsync();

            // Result
            var artistAdded = _dbContext.Artists.FirstOrDefault(_ => _.ArtistId == testArtist.ArtistId);
            Assert.NotNull(artistAdded);
        }
    }
}
