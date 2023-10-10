using AutoFixture;
using FarmLink.Shared.Tests;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PromotionsService.Controllers;
using PromotionsService.Models;
using PromotionsService.Models.RequestModels;
using PromotionsService.Models.ResponseModels;
using PromotionsService.Repositories.Interfaces;
using System.Net;

namespace PromotionService.Tests.Controllers
{
    public class PromotionsControllerTests : IClassFixture<LoggerFixture<PromotionsController>>
    {
        private readonly IFixture _fixture;
        private readonly Mock<IPromotionsRepository> _repositoryMock;
        private readonly PromotionsController _controller;
        private readonly ILogger<PromotionsController> _loggerMock;
        private readonly LoggerFixture<PromotionsController> _loggerFixture;
        public PromotionsControllerTests()
        {
            _fixture = new Fixture();
            _repositoryMock = _fixture.Freeze<Mock<IPromotionsRepository>>();
            _loggerFixture = new LoggerFixture<PromotionsController>();
            _loggerMock = _loggerFixture.GetRegisteredLogger();
            _controller = new PromotionsController(_loggerMock, _repositoryMock.Object);
        }

        [Fact]
        public async Task AddPromotion_ShouldReturnOkResponse_WhenModelValid()
        {
            //Arrange
            var requestMock = _fixture.Create<PromotionRequestModel>();
            var responseMock = new PromotionResponseModel(_fixture.Create<Promotion>());
            _repositoryMock.Setup(x => x.CreatePromotionAsync(requestMock)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.AddPromotion(requestMock).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task AddPromotion_ShouldReturnBadRequestResponse_WhenModelInvalid()
        {
            //Arrange
            var requestMock = _fixture.Create<PromotionRequestModel>();
            var responseMock = new PromotionResponseModel(null);
            _repositoryMock.Setup(x => x.CreatePromotionAsync(requestMock)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.AddPromotion(requestMock).ConfigureAwait(false);

            //Assert
            var badReqeustResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.BadRequest, badReqeustResult.StatusCode);
        }

        [Fact]
        public async Task AddPromotion_ShouldreturnInternalServerError_WhenErrorOccured()
        {

            //Arrange
            var requestMock = _fixture.Create<PromotionRequestModel>();
            var responseMock = new PromotionResponseModel(null,"",true);
            _repositoryMock.Setup(x => x.CreatePromotionAsync(requestMock)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.AddPromotion(requestMock).ConfigureAwait(false);

            //Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
        }

        [Fact]
        public async Task UpdatePromotion_ShouldReturnOkResponse_WhenModelValid()
        {
            //Arrange
            var requestMock = _fixture.Create<PromotionRequestModel>();
            var responseMock = new PromotionResponseModel(_fixture.Create<Promotion>());
            _repositoryMock.Setup(x => x.UpdatePromotionAsync(requestMock)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.UpdatePromotion(requestMock).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task UpdatePromotion_ShouldReturnBadRequestResponse_WhenModelInvalid()
        {
            //Arrange
            var requestMock = _fixture.Create<PromotionRequestModel>();
            var responseMock = new PromotionResponseModel(null);
            _repositoryMock.Setup(x => x.UpdatePromotionAsync(requestMock)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.UpdatePromotion(requestMock).ConfigureAwait(false);

            //Assert
            var badReqeustResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.BadRequest, badReqeustResult.StatusCode);
        }

        [Fact]
        public async Task UpdatePromotion_ShouldreturnInternalServerError_WhenErrorOccured()
        {

            //Arrange
            var requestMock = _fixture.Create<PromotionRequestModel>();
            var responseMock = new PromotionResponseModel(null, "", true);
            _repositoryMock.Setup(x => x.UpdatePromotionAsync(requestMock)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.UpdatePromotion(requestMock).ConfigureAwait(false);

            //Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
        }

        [Fact]
        public async Task DeletePromotion_ShouldReturnOkResponse_WhenDataDeleted()
        {
            //Arrange
            string id = _fixture.Create<string>();
            _repositoryMock.Setup(x => x.DeletePromotionAsync(id)).ReturnsAsync(1);

            //Act
            var result = await _controller.DeletePromotion(id).ConfigureAwait(false);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task DeletePromotion_ShouldReturnNotFoundResponse_WhenDataNotDeleted()
        {
            //Arrange
            string id = _fixture.Create<string>();
            _repositoryMock.Setup(x => x.DeletePromotionAsync(id)).ReturnsAsync(0);

            //Act
            var result = await _controller.DeletePromotion(id).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }


        [Fact]
        public async Task DeletePromotion_ShouldReturnInternalServerErrorResponse_WhenMoreThanOneRecordDeleted()
        {
            //Arrange
            string id = _fixture.Create<string>();
            _repositoryMock.Setup(x => x.DeletePromotionAsync(id)).ReturnsAsync(2);

            //Act
            var result = await _controller.DeletePromotion(id).ConfigureAwait(false);

            //Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
        }

        [Fact]
        public async Task GetPromotion_ShouldReturnOkResponse_WhenDataFound()
        {
            //Arrange
            string id =_fixture.Create<string>();
            var responseMock = new PromotionResponseModel(_fixture.Create<Promotion>());
            _repositoryMock.Setup(x=>x.GetPromotionByIdAsync(id)).ReturnsAsync(responseMock);

            //Act
            var result =await _controller.GetPromotion(id).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task GetPromotion_ShouldReturnNotFoundResponse_WhenNoDataFound()
        {
            //Arrange
            string id = _fixture.Create<string>();
            var responseMock = new PromotionResponseModel(null);
            _repositoryMock.Setup(x => x.GetPromotionByIdAsync(id)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetPromotion(id).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task GetPromotion_ShouldReturnInternalServerErrorResponse_WhenErrorFound()
        {
            //Arrange
            string id = _fixture.Create<string>();
            var responseMock = new PromotionResponseModel(null,"",true);
            _repositoryMock.Setup(x => x.GetPromotionByIdAsync(id)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetPromotion(id).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
        }

        [Fact]
        public async Task GetPromotions_shouldReturnOkResponse_WhenDataFound()
        {
            //Arrange
            var responseMock = new[] { new PromotionResponseModel(_fixture.Create<Promotion>()) };
            _repositoryMock.Setup(x => x.GetPromotionsAsync()).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetPromotions().ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task GetPromotions_shouldReturnNotFoundResponse_WhenDataNotFound()
        {
            //Arrange
            var responseMock = new[] { new PromotionResponseModel(null) };
            _repositoryMock.Setup(x => x.GetPromotionsAsync()).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetPromotions().ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task GetPromotions_shouldReturnInternalServerErrorResponse_WhenEnrrorOccured()
        {
            //Arrange
            var responseMock = new[] { new PromotionResponseModel(null,"",true) };
            _repositoryMock.Setup(x => x.GetPromotionsAsync()).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetPromotions().ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
        }

        [Fact]
        public async Task GetPromotionsByTargetAudience_shouldReturnOkResponse_WhenDataFound()
        {
            //Arrange
            var requestMock = _fixture.Create<PromotionsByTargetAudienceRequestModel>();
            var responseMock = new[] { new PromotionResponseModel(_fixture.Create<Promotion>()) };
            _repositoryMock.Setup(x => x.GetPromotionsByTargetAudienceAsync(requestMock.TargetAudience)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetPromotionsByTargetAudience(requestMock).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task GetPromotionsByTargetAudience_shouldReturnNotFoundResponse_WhenDataNotFound()
        {
            //Arrange
            var requestMock = _fixture.Create<PromotionsByTargetAudienceRequestModel>();
            var responseMock = new[] { new PromotionResponseModel(null) };
            _repositoryMock.Setup(x => x.GetPromotionsByTargetAudienceAsync(requestMock.TargetAudience)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetPromotionsByTargetAudience(requestMock).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task GetPromotionsByTargetAudience_shouldReturnInternalServerErrorResponse_WhenEnrrorOccured()
        {
            //Arrange
            var requestMock = _fixture.Create<PromotionsByTargetAudienceRequestModel>();
            var responseMock = new[] { new PromotionResponseModel(null,"",true) };
            _repositoryMock.Setup(x => x.GetPromotionsByTargetAudienceAsync(requestMock.TargetAudience)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetPromotionsByTargetAudience(requestMock).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
        }

        [Fact]
        public async Task GetPromotionsByType_shouldReturnOkResponse_WhenDataFound()
        {
            //Arrange
            var requestMock = _fixture.Create<PromotionsByTypeRequestModel>();
            var responseMock = new[] { new PromotionResponseModel(_fixture.Create<Promotion>()) };
            _repositoryMock.Setup(x => x.GetPromotionsByTypeAsync(requestMock.PromotionType)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetPromotionsByType(requestMock).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task GetPromotionsByType_shouldReturnNotFoundResponse_WhenDataNotFound()
        {
            //Arrange
            var requestMock = _fixture.Create<PromotionsByTypeRequestModel>();
            var responseMock = new[] { new PromotionResponseModel(null) };
            _repositoryMock.Setup(x => x.GetPromotionsByTypeAsync(requestMock.PromotionType)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetPromotionsByType(requestMock).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task GetPromotionsByType_shouldReturnInternalServerErrorResponse_WhenEnrrorOccured()
        {
            //Arrange
            var requestMock = _fixture.Create<PromotionsByTypeRequestModel>();
            var responseMock = new[] { new PromotionResponseModel(null,"",true) };
            _repositoryMock.Setup(x => x.GetPromotionsByTypeAsync(requestMock.PromotionType)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetPromotionsByType(requestMock).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
        }
    }
}
