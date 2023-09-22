using Catalog.Domain.DTOs.Request.Validators;
using Catalog.Domain.DTOs.Request;
using Catalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using Catalog.Domain.DTOs.Request.Item;
using Catalog.Domain.Services;
using Moq;
using Catalog.Domain.DTOs.Request.Artist;
using Catalog.Domain.DTOs.Request.Genre;
using Catalog.Domain.DTOs.Response;

namespace Catalog.Domain.Tests.Requests.Items.Validators
{
    public class AddItemRequestValidatorTests
    {
        private readonly AddItemRequestValidator _validator;
        private readonly Mock<IArtistService> _artistServiceMock;
        private readonly Mock<IGenreService> _genreServiceMock;
        public AddItemRequestValidatorTests()
        {
            _artistServiceMock = new Mock<IArtistService>();
            _artistServiceMock.Setup(x => x.GetArtistAsync(It.IsAny<GetArtistRequest>(), default(CancellationToken)))
                                           .ReturnsAsync(() => new ArtistResponse());
            _genreServiceMock = new Mock<IGenreService>();
            _genreServiceMock.Setup(x => x.GetGenreAsync(It.IsAny<GetGenreRequest>(), default(CancellationToken)))
                                            .ReturnsAsync(() => new GenreResponse());
            _validator = new AddItemRequestValidator(_artistServiceMock.Object, _genreServiceMock.Object);
        }
        [Fact]
        public async void ArtistIdNull_ReturnsError()
        {
            var addItemRequest = new AddItemRequest
            {
                Price = new Price()
            };
            var validatorTest = await _validator.TestValidateAsync(addItemRequest);
            validatorTest.ShouldHaveValidationErrorFor(x => x.ArtistId);
        }
        [Fact]
        public async void GenreIdNull_ReturnsError()
        {
            var addItemRequest = new AddItemRequest
            {
                Price = new Price()
            };
            var validatorTest = await _validator.TestValidateAsync(addItemRequest);
            validatorTest.ShouldHaveValidationErrorFor(x => x.GenreId);
         
        }

        [Fact]
        public async void ArtistDoesNotExist_ReturnsError()
        {
            _artistServiceMock.Setup(x => x.GetArtistAsync(It.IsAny<GetArtistRequest>(), default(CancellationToken)))
                                            .ReturnsAsync(() => null);
            var addItemRequest = new AddItemRequest
            {
                Price = new Price(),
                ArtistId = Guid.NewGuid()
            };
            var validatorTest = await _validator.TestValidateAsync(addItemRequest);
            validatorTest.ShouldHaveValidationErrorFor(x => x.ArtistId);
        }
        [Fact]
        public async void GenreDoesNotExist_ReturnsError()
        {
            _genreServiceMock.Setup(x => x.GetGenreAsync(It.IsAny<GetGenreRequest>(), default(CancellationToken)))
                                          .ReturnsAsync(() => null);
            var addItemRequest = new AddItemRequest
            {
                Price = new Price(),
                GenreId = Guid.NewGuid()
            };
            var validatorTest = await _validator.TestValidateAsync(addItemRequest);
            validatorTest.ShouldHaveValidationErrorFor(x => x.GenreId);
        }

    }
}

