using Catalog.Domain.Contracts.Persistence;
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
    public class GenreRepositoryTests : IClassFixture<CatalogContextFactory>
    {
        private readonly TestCatalogContext _dbContext;
        private readonly GenreRepository _genreRepository;

        public GenreRepositoryTests(CatalogContextFactory catalogContextFactory)
        {
            _dbContext = catalogContextFactory._dbContext;
            _genreRepository = new GenreRepository(_dbContext);
        }

        [Fact]
        public async void GetAllAsync_Success()
        {
            // Act
            var result = await _genreRepository.GetAllAsync();

            // Result
            Assert.IsAssignableFrom<IEnumerable<Genre>>(result);
            Assert.NotNull(result);
        }

        [Fact]
        public async void GetByIdAsync_ReturnsNull()
        {
            // Act
            var result = await _genreRepository.GetByIdAsync(new Guid());

            // Result
            Assert.Null(result);
        }

        [Theory]
        [InlineData("c04f05c0-f6ad-44d1-a400-3375bfb5dfd6")]
        public async Task GetByIdAsync_ReturnItem(Guid guid)
        {
            // Act
            var result = await _genreRepository.GetByIdAsync(guid);

            // Result
            Assert.Equal(result.GenreId, guid);
        }

        [Fact]
        public async Task Add_Success()
        {
            //Arrange
            var testGenre = new Genre
            {
                GenreDescription = "Test Description"
            };

            // Act
            _genreRepository.Add(testGenre);
            await _dbContext.SaveEntitiesAsync();

            // Result
            var artistAdded = _dbContext.Genres.FirstOrDefault(_ => _.GenreId == testGenre.GenreId);
            Assert.NotNull(artistAdded);
        }
    }
}
