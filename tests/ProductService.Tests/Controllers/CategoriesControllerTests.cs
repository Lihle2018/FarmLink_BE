using AutoFixture;
using FarmLink.Shared.Tests;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ProductService.Controllers;
using ProductService.Models;
using ProductService.Models.RequestModels;
using ProductService.Models.ResponseModels;
using ProductService.Repositories.Interfaces;
using System.Net;

namespace ProductService.Tests.Controllers
{
    public class CategoriesControllerTests : IClassFixture<LoggerFixture<CategoriesController>>
    {

        private readonly IFixture _fixture;
        private readonly Mock<ICategoryRepository> _repositoryMock;
        private readonly CategoriesController _controller;
        private readonly ILogger<CategoriesController> _loggerMock;
        private readonly LoggerFixture<CategoriesController> _loggerFixture;
        public CategoriesControllerTests()
        {
            _fixture = new Fixture();
            _repositoryMock = _fixture.Freeze<Mock<ICategoryRepository>>();
            _loggerFixture = new LoggerFixture<CategoriesController>();
            _loggerMock = _loggerFixture.GetRegisteredLogger();
            _controller = new CategoriesController(_loggerMock, _repositoryMock.Object);
        }

        [Fact]
        public async Task AddCategory_ShouldReturnOkResponse_WhenValidModel()
        {
            //Arrange
            var requestMock =_fixture.Create<CategoryRequestModel>();
            var responseMock = new CategoryResponseModel(_fixture.Create<Category>());
            _repositoryMock.Setup(x=>x.CreateCategoryAsync(requestMock)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.AddCategory(requestMock).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task AddCategory_ShouldReturnBadRequestResponse_WhenInvalidModel()
        {
            //Arrange
            var requestMock = _fixture.Create<CategoryRequestModel>();
            var responseMock = new CategoryResponseModel(null);
            _repositoryMock.Setup(x => x.CreateCategoryAsync(requestMock)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.AddCategory(requestMock).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var badReqeustResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.BadRequest, badReqeustResult.StatusCode);
        }

        [Fact]
        public async Task AddCategory_ShouldReturnInternalServerErrorResponse_WhenErrorOccured()
        {
            //Arrange
            var requestMock = _fixture.Create<CategoryRequestModel>();
            var responseMock = new CategoryResponseModel(null,"",true);
            _repositoryMock.Setup(x => x.CreateCategoryAsync(requestMock)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.AddCategory(requestMock).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
        }

        [Fact]
        public async Task UpdateCategory_ShouldReturnOkResponse_WhenDataFound()
        {
            //Arrange
            var requestMock = _fixture.Create<CategoryRequestModel>();
            var responseMock = new CategoryResponseModel(_fixture.Create<Category>());
            _repositoryMock.Setup(x => x.UpdateCategoryAsync(requestMock)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.UpdateCategory(requestMock).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task UpdateCategory_ShouldReturnNotFoundResponse_WhenDataNotFound()
        {
            //Arrange
            var requestMock = _fixture.Create<CategoryRequestModel>();
            var responseMock = new CategoryResponseModel(null);
            _repositoryMock.Setup(x => x.UpdateCategoryAsync(requestMock)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.UpdateCategory(requestMock).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task UpdateCategory_ShouldReturnInternalServerErrorResponse_WhenErrorOccured()
        {
            //Arrange
            var requestMock = _fixture.Create<CategoryRequestModel>();
            var responseMock = new CategoryResponseModel(null, "", true);
            _repositoryMock.Setup(x => x.UpdateCategoryAsync(requestMock)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.UpdateCategory(requestMock).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
        }

        [Fact]
        public async Task DeleteCategory_ShouldReturnOkResponse_WhenDataDeleted()
        {
            //Arrange
            string id = _fixture.Create<string>();
            _repositoryMock.Setup(x => x.DeleteCategoryAsync(id)).ReturnsAsync(1);

            //Act
            var result =await _controller.DeleteCategory(id).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task DeleteCategory_ShouldReturnNotFoundResponse_WhenDataNotDeleted()
        {
            //Arrange
            string id = _fixture.Create<string>();
            _repositoryMock.Setup(x => x.DeleteCategoryAsync(id)).ReturnsAsync(0);

            //Act
            var result = await _controller.DeleteCategory(id).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }


        [Fact]
        public async Task DeleteCategory_ShouldReturnInternalServerErrorResponse_WhenDataNotDeleted()
        {
            //Arrange
            string id = _fixture.Create<string>();
            _repositoryMock.Setup(x => x.DeleteCategoryAsync(id)).ReturnsAsync(2);

            //Act
            var result = await _controller.DeleteCategory(id).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
        }

        [Fact]
        public async Task GetCategory_ShouldReturnOkResponse_WhenDataFound()
        {
            //Arrange
            string id = _fixture.Create<string>();
            var responseMock = new CategoryResponseModel(_fixture.Create<Category>());
            _repositoryMock.Setup(x => x.GetCategoryAsync(id)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetCategory(id).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task GetCategory_ShouldReturnNotFoundResponse_WhenNoDataFound()
        {
            //Arrange
            string id = _fixture.Create<string>();
            var responseMock = new CategoryResponseModel(null);
            _repositoryMock.Setup(x => x.GetCategoryAsync(id)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetCategory(id).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task GetCategory_ShouldReturnInternalserverErrorResponse_WhenErrorOccured()
        {
            //Arrange
            string id = _fixture.Create<string>();
            var responseMock = new CategoryResponseModel(null,"",true);
            _repositoryMock.Setup(x => x.GetCategoryAsync(id)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetCategory(id).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
        }

        [Fact]
        public async Task GetCategories_ShouldReturnOkResponse_WhenDataFound()
        {
            //Arrange
            var responseMock = new[] { new CategoryResponseModel(_fixture.Create<Category>()) };
            _repositoryMock.Setup(x => x.GetCategoriesAsync()).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetCategories().ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task GetCategories_ShouldReturnNotFoundResponse_WhenDataNotFound()
        {
            //Arrange
            var responseMock = new[] { new CategoryResponseModel(null) };
            _repositoryMock.Setup(x => x.GetCategoriesAsync()).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetCategories().ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task GetCategories_ShouldReturnInternalServerResponse_WhenDataNotFound()
        {
            //Arrange
            var responseMock = new[] { new CategoryResponseModel(null,"",true) };
            _repositoryMock.Setup(x => x.GetCategoriesAsync()).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetCategories().ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
        }
    }
}
