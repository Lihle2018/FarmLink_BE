
using AutoFixture;
using FarmLink.Shared.Tests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ReviewService.Controllers;
using ReviewService.Models.RequestModels;
using ReviewService.Models.ResponseModels;
using ReviewService.Models;
using ReviewService.Repositories.Interfaces;
using System.Net;
using FluentAssertions;

namespace ReviewService.Tests.Controllers
{
    public class RatingsControllerTests : IClassFixture<LoggerFixture<RatingsController>>
    {
        private readonly IFixture _fixture;
        private readonly Mock<IRatingsRepository> _repositoryMock;
        private readonly RatingsController _controller;
        private readonly ILogger<RatingsController> _loggerMock;
        private readonly LoggerFixture<RatingsController> _loggerFixture;
        public RatingsControllerTests()
        {
            _fixture = new Fixture();
            _repositoryMock = _fixture.Freeze<Mock<IRatingsRepository>>();
            _loggerFixture = new LoggerFixture<RatingsController>();
            _loggerMock = _loggerFixture.GetRegisteredLogger();
            _controller = new RatingsController(_loggerMock, _repositoryMock.Object);
        }

        [Fact]
        public async Task AddRating_ShouldReturnOkResponse_WhenModelIsValid()
        {
            //Arrange
            var requestMock = _fixture.Create<RatingRequestModel>();
            var responseMock = new RatingResponseModel(_fixture.Create<Rating>());
            _repositoryMock.Setup(x => x.AddRatingAsync(requestMock)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.AddRating(requestMock).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task AddRating_ShouldReturnBadRequestResponse_WhenDataNotInserted()
        {
            //Arrange
            var requestMock = _fixture.Create<RatingRequestModel>();
            var responseMock = new RatingResponseModel(null);
            _repositoryMock.Setup(x => x.AddRatingAsync(requestMock)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.AddRating(requestMock).ConfigureAwait(false);

            //Assert
            var badReqeustResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.BadRequest, badReqeustResult.StatusCode);
        }

        [Fact]
        public async Task AddRating_ShouldInternalServerErrorResponse_WhenErrorOccured()
        {
            //Arrange
            var requestMock = _fixture.Create<RatingRequestModel>();
            var responseMock = new RatingResponseModel(null, "", true);
            _repositoryMock.Setup(x => x.AddRatingAsync(requestMock)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.AddRating(requestMock).ConfigureAwait(false);

            //Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
        }

        [Fact]
        public async Task UpdateRating_ShouldReturnOkResponse_WhenDataFound()
        {
            //Arrange
            var requestMock = _fixture.Create<RatingRequestModel>();
            var responseMock = new RatingResponseModel(_fixture.Create<Rating>());
            _repositoryMock.Setup(x => x.UpdateRatingAsync(requestMock)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.UpdateRating(requestMock).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task UpdateRating_ShouldReturnBadRequestResponse_WhenDataNotInserted()
        {
            //Arrange
            var requestMock = _fixture.Create<RatingRequestModel>();
            var responseMock = new RatingResponseModel(null);
            _repositoryMock.Setup(x => x.UpdateRatingAsync(requestMock)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.UpdateRating(requestMock).ConfigureAwait(false);

            //Assert
            var badReqeustResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.BadRequest, badReqeustResult.StatusCode);
        }

        [Fact]
        public async Task UpdateRating_ShouldInternalServerErrorResponse_WhenErrorOccured()
        {
            //Arrange
            var requestMock = _fixture.Create<RatingRequestModel>();
            var responseMock = new RatingResponseModel(null, "", true);
            _repositoryMock.Setup(x => x.UpdateRatingAsync(requestMock)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.UpdateRating(requestMock).ConfigureAwait(false);

            //Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
        }

        [Fact]
        public async Task GetRating_ShouldReturnOkresponse_WhenDataFound()
        {
            //Arrange
            var id = _fixture.Create<string>();
            var responseMock = new RatingResponseModel(_fixture.Create<Rating>());
            _repositoryMock.Setup(x => x.GetRatingByIdAsync(id)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetRating(id);

            //Assert
            result.Should().NotBeNull();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task GetRating_ShouldReturnNotFound_WhenDataNotFound()
        {
            //Arrange
            var id = _fixture.Create<string>();
            var responseMock = new RatingResponseModel(null);
            _repositoryMock.Setup(x => x.GetRatingByIdAsync(id)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetRating(id);

            //Assert
            result.Should().NotBeNull();
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task GetRating_ShouldReturnInternalServerError_WhenErrorOccured()
        {
            //Arrange
            var id = _fixture.Create<string>();
            var responseMock = new RatingResponseModel(null, "", true);
            _repositoryMock.Setup(x => x.GetRatingByIdAsync(id)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetRating(id);

            //Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
        }

        [Fact]
        public async Task GetRatings_ShouldReturnOkresponse_WhenDataFound()
        {
            //Arrange
            var responseMock = new[] { new RatingResponseModel(_fixture.Create<Rating>()) };
            _repositoryMock.Setup(x => x.GetRatingsAsync()).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetRatings();

            //Assert
            result.Should().NotBeNull();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task GetRatings_ShouldReturnNotFound_WhenDataNotFound()
        {
            //Arrange
            var responseMock = new[] { new RatingResponseModel(null) };
            _repositoryMock.Setup(x => x.GetRatingsAsync()).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetRatings();

            //Assert
            result.Should().NotBeNull();
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task GetRatings_ShouldReturnInternalServerError_WhenErrorOccured()
        {
            //Arrange
            var responseMock = new[] { new RatingResponseModel(null, "", true) };
            _repositoryMock.Setup(x => x.GetRatingsAsync()).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetRatings();

            //Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
        }

        [Fact]
        public async Task GetRatingsByUserId_ShouldReturnOkresponse_WhenDataFound()
        {
            //Arrange
            string id = _fixture.Create<string>();
            var responseMock = new[] { new RatingResponseModel(_fixture.Create<Rating>()) };
            _repositoryMock.Setup(x => x.GetRatingsByUserIdAsync(id)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetRatingsByUserId(id);

            //Assert
            result.Should().NotBeNull();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task GetRatingsByUserId_ShouldReturnNotFound_WhenDataNotFound()
        {
            //Arrange
            string id = _fixture.Create<string>();
            var responseMock = new[] { new RatingResponseModel(null) };
            _repositoryMock.Setup(x => x.GetRatingsByUserIdAsync(id)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetRatingsByUserId(id);

            //Assert
            result.Should().NotBeNull();
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task GetRatingsByUserId_ShouldReturnInternalServerError_WhenErrorOccured()
        {
            //Arrange
            string id = _fixture.Create<string>();
            var responseMock = new[] { new RatingResponseModel(null, "", true) };
            _repositoryMock.Setup(x => x.GetRatingsByUserIdAsync(id)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetRatingsByUserId(id);

            //Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
        }


        [Fact]
        public async Task GetRatingsByPostId_ShouldReturnOkresponse_WhenDataFound()
        {
            //Arrange
            string id = _fixture.Create<string>();
            var responseMock = new[] { new RatingResponseModel(_fixture.Create<Rating>()) };
            _repositoryMock.Setup(x => x.GetRatingsForPostAsync(id)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetRatingsByPostId(id);

            //Assert
            result.Should().NotBeNull();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task GetRatingsByPostId_ShouldReturnNotFound_WhenDataNotFound()
        {
            //Arrange
            string id = _fixture.Create<string>();
            var responseMock = new[] { new RatingResponseModel(null) };
            _repositoryMock.Setup(x => x.GetRatingsForPostAsync(id)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetRatingsByPostId(id);

            //Assert
            result.Should().NotBeNull();
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task GetRatingsByPostId_ShouldReturnInternalServerError_WhenErrorOccured()
        {
            //Arrange
            string id = _fixture.Create<string>();
            var responseMock = new[] { new RatingResponseModel(null, "", true) };
            _repositoryMock.Setup(x => x.GetRatingsForPostAsync(id)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetRatingsByPostId(id);

            //Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
        }

        [Fact]
        public async Task DeleteRating_ShouldReturnOkResponse_WhenDataDeleted()
        {
            //Arrange
            string id = _fixture.Create<string>();
            _repositoryMock.Setup(x => x.DeleteRatingAsync(id)).ReturnsAsync(1);

            //Act
            var result = await _controller.DeleteRating(id).ConfigureAwait(false);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task DeleteRating_ShouldReturnNotFoundResponse_WhenDataNotDeleted()
        {
            //Arrange
            string id = _fixture.Create<string>();
            _repositoryMock.Setup(x => x.DeleteRatingAsync(id)).ReturnsAsync(0);

            //Act
            var result = await _controller.DeleteRating(id).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task DeleteRating_ShouldReturnInternalServerError_WhenMoreThanOneRatingDeleted()
        {
            //Arrange
            string id = _fixture.Create<string>();
            _repositoryMock.Setup(x => x.DeleteRatingAsync(id)).ReturnsAsync(2);

            //Act
            var result = await _controller.DeleteRating(id).ConfigureAwait(false);

            //Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
        }
    }
}
