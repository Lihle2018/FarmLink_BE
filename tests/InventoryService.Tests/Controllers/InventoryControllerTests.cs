

using AutoFixture;
using FarmLink.Shared.Tests;
using FluentAssertions;
using InventoryService.Controllers;
using InventoryService.Models;
using InventoryService.Models.RequestModels;
using InventoryService.Models.ResponseModels;
using InventoryService.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

namespace InventoryService.Tests.Controllers
{
    public class InventoryControllerTests: IClassFixture<LoggerFixture<InventoryController>>
    {
        private readonly IFixture _fixture;
        private readonly Mock<IInventoryRepository> _repositoryMock;
        private readonly InventoryController _controller;
        private readonly ILogger<InventoryController> _loggerMock;
        private readonly LoggerFixture<InventoryController> _loggerFixture;
        public InventoryControllerTests()
        {
            _fixture = new Fixture();
            _repositoryMock = _fixture.Freeze<Mock<IInventoryRepository>>();
            _loggerFixture = new LoggerFixture<InventoryController>();
            _loggerMock = _loggerFixture.GetRegisteredLogger();
            _controller = new InventoryController(_loggerMock, _repositoryMock.Object);
        }

        [Fact]
        public async Task AddItem_ShouldReturnOkResponse_WhenValidModel()
        {
            //Arrange
            var requestMock = _fixture.Create<InventoryItemRequestModel>();
            var responseMock = new InventoryItemResponseModel(_fixture.Create<InventoryItem>());
            _repositoryMock.Setup(x => x.AddInventoryItemAsync(requestMock)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.AddItem(requestMock).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task AddItem_ShouldReturnBadRequestResponse_WhenInvalidModel()
        {
            //Arrange
            var requestMock = _fixture.Create<InventoryItemRequestModel>();
            var responseMock = new InventoryItemResponseModel(null);
            _repositoryMock.Setup(x => x.AddInventoryItemAsync(requestMock)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.AddItem(requestMock).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var badReqeustResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.BadRequest, badReqeustResult.StatusCode);
        }

        [Fact]
        public async Task AddItem_ShouldReturnInternalServerErrorResponse_WhenErrorOccured()
        {
            //Arrange
            var requestMock = _fixture.Create<InventoryItemRequestModel>();
            var responseMock = new InventoryItemResponseModel(null,error:true);
            _repositoryMock.Setup(x => x.AddInventoryItemAsync(requestMock)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.AddItem(requestMock).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
        }

        [Fact]
        public async Task UpdateItem_ShouldReturnOkResponse_WhenDataFound()
        {
            //Arrange
            var requestMock = _fixture.Create<InventoryItemRequestModel>();
            var responseMock = new InventoryItemResponseModel(_fixture.Create<InventoryItem>());
            _repositoryMock.Setup(x => x.UpdateInventoryItemAsync(requestMock)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.UpdateItem(requestMock).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task UpdateItem_ShouldReturnNotFoundResponse_WhenNoDataFound()
        {
            //Arrange
            var requestMock = _fixture.Create<InventoryItemRequestModel>();
            var responseMock = new InventoryItemResponseModel(null);
            _repositoryMock.Setup(x => x.UpdateInventoryItemAsync(requestMock)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.UpdateItem(requestMock).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task UpdateItem_ShouldReturnInternalServerErrorResponse_WhenErrorOccured()
        {
            //Arrange
            var requestMock = _fixture.Create<InventoryItemRequestModel>();
            var responseMock = new InventoryItemResponseModel(null, error: true);
            _repositoryMock.Setup(x => x.UpdateInventoryItemAsync(requestMock)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.UpdateItem(requestMock).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
        }

        [Fact]
        public async Task DeleteItem_ShouldReturnOkResponse_WhenDataDeleted()
        {
            //Arrange
            string id =_fixture.Create<string>();
            _repositoryMock.Setup(x => x.DeleteInventoryItemAsync(id)).ReturnsAsync(1);

            //Act
            var result =await _controller.DeleteItem(id).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task DeleteItem_ShoouldReturnNotFoundResponse_WhenNoDeleted()
        {
            //Arrange
            string id = _fixture.Create<string>();
            _repositoryMock.Setup(x => x.DeleteInventoryItemAsync(id)).ReturnsAsync(0);

            //Act
            var result = await _controller.DeleteItem(id).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task DeleteItem_ShouldReturnInternalServerErrorResponse_WhenErrorOccured()
        {
            //Arrange
            string id = _fixture.Create<string>();
            _repositoryMock.Setup(x => x.DeleteInventoryItemAsync(id)).ReturnsAsync(2);

            //Act
            var result = await _controller.DeleteItem(id).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
        }

        [Fact]
        public async Task GetItem_ShouldReturnOkResponse_WhenDataFound()
        {
            //Arrange
            string id = _fixture.Create<string>();
            var responseMock =new InventoryItemResponseModel(_fixture.Create<InventoryItem>());
            _repositoryMock.Setup(x=>x.GetInventoryItemByIdAsync(id)).ReturnsAsync(responseMock);

            //Act
            var result =await _controller.GetItem(id).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task GetItem_ShouldReturnNotFoundResponse_WhenNoDataFound()
        {
            //Arrange
            string id = _fixture.Create<string>();
            var responseMock = new InventoryItemResponseModel(null);
            _repositoryMock.Setup(x => x.GetInventoryItemByIdAsync(id)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetItem(id).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task GetItem_ShouldReturnInternalServerErrorResponse_WhenErrorOccured()
        {
            //Arrange
            string id = _fixture.Create<string>();
            var responseMock = new InventoryItemResponseModel(null,error:true);
            _repositoryMock.Setup(x => x.GetInventoryItemByIdAsync(id)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetItem(id).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
        }

        [Fact]
        public async Task GetItems_ShouldReturnOkResponse_WhenDataFound()
        {
            //Arrange
            var responseMock = new[] { new InventoryItemResponseModel(_fixture.Create<InventoryItem>()) };
            _repositoryMock.Setup(x => x.GetInventoryItemsAsync()).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetItems().ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task GetItems_ShouldReturnNotFoundResponse_WhenNoDataFound()
        {
            //Arrange
            var responseMock = new[] { new InventoryItemResponseModel(null) };
            _repositoryMock.Setup(x => x.GetInventoryItemsAsync()).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetItems().ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task GetItems_ShouldReturnInternalServerErrorResponse_WhenErrorOccured()
        {
            //Arrange
            var responseMock = new[] { new InventoryItemResponseModel(null,error:true) };
            _repositoryMock.Setup(x => x.GetInventoryItemsAsync()).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetItems().ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
        }

        [Fact]
        public async Task GetItemsByProduct_ShouldReturnOkResponse_WhenDataFound()
        {
            //Arrange
            var productId = _fixture.Create<string>();
            var responseMock = new[] { new InventoryItemResponseModel(_fixture.Create<InventoryItem>()) };
            _repositoryMock.Setup(x => x.GetInventoryItemsByProductIdAsync(productId)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetItemsByProduct(productId).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task GetItemsByProduct_ShouldReturnNotFoundResponse_WhenNoDataFound()
        {
            //Arrange
            var productId = _fixture.Create<string>();
            var responseMock = new[] { new InventoryItemResponseModel(null) };
            _repositoryMock.Setup(x => x.GetInventoryItemsByProductIdAsync(productId)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetItemsByProduct(productId).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task GetItemsByProduct_ShouldReturnInternalServerErrorResponse_WhenErrorOccured()
        {
            //Arrange
            var productId = _fixture.Create<string>();
            var responseMock = new[] { new InventoryItemResponseModel(null,error:true) };
            _repositoryMock.Setup(x => x.GetInventoryItemsByProductIdAsync(productId)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetItemsByProduct(productId).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
        }
    }
}
