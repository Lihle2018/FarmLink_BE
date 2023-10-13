
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
    public class ProductsControllerTests : IClassFixture<LoggerFixture<ProductsController>>
    {
        private readonly IFixture _fixture;
        private readonly Mock<IProductRepository> _repositoryMock;
        private readonly ProductsController _controller;
        private readonly ILogger<ProductsController> _loggerMock;
        private readonly LoggerFixture<ProductsController> _loggerFixture;
        public ProductsControllerTests()
        {
            _fixture = new Fixture();
            _repositoryMock = _fixture.Freeze<Mock<IProductRepository>>();
            _loggerFixture = new LoggerFixture<ProductsController>();
            _loggerMock = _loggerFixture.GetRegisteredLogger();
            _controller = new ProductsController(_loggerMock, _repositoryMock.Object);
        }

        [Fact]
        public async Task AddProduct_ShouldReturnOkResponse_WhenValidModel()
        {
            //Arrange
            var requestMock = _fixture.Create<ProductRequestModel>();
            var responseMock = new ProductResponseModel(_fixture.Create<Product>());
            _repositoryMock.Setup(x => x.CreateProductAsync(requestMock)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.CreateProduct(requestMock).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task AddProduct_ShouldReturnBadRequestResponse_WhenInvalidModel()
        {
            //Arrange
            var requestMock = _fixture.Create<ProductRequestModel>();
            var responseMock = new ProductResponseModel(null);
            _repositoryMock.Setup(x => x.CreateProductAsync(requestMock)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.CreateProduct(requestMock).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var badReqeustResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.BadRequest, badReqeustResult.StatusCode);
        }

        [Fact]
        public async Task AddProduct_ShouldReturnInternalServerErrorResponse_WhenErrorOccured()
        {
            //Arrange
            var requestMock = _fixture.Create<ProductRequestModel>();
            var responseMock = new ProductResponseModel(null, "", true);
            _repositoryMock.Setup(x => x.CreateProductAsync(requestMock)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.CreateProduct(requestMock).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
        }

        [Fact]
        public async Task UpdateProduct_ShouldReturnOkResponse_WhenDataFound()
        {
            //Arrange
            var requestMock = _fixture.Create<ProductRequestModel>();
            var responseMock = new ProductResponseModel(_fixture.Create<Product>());
            _repositoryMock.Setup(x => x.UpdateProductAsync(requestMock)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.UpdateProduct(requestMock).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task UpdateProduct_ShouldReturnNotFoundResponse_WhenDataNotFound()
        {
            //Arrange
            var requestMock = _fixture.Create<ProductRequestModel>();
            var responseMock = new ProductResponseModel(null);
            _repositoryMock.Setup(x => x.UpdateProductAsync(requestMock)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.UpdateProduct(requestMock).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task UpdateProduct_ShouldReturnInternalServerErrorResponse_WhenErrorOccured()
        {
            //Arrange
            var requestMock = _fixture.Create<ProductRequestModel>();
            var responseMock = new ProductResponseModel(null, "", true);
            _repositoryMock.Setup(x => x.UpdateProductAsync(requestMock)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.UpdateProduct(requestMock).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
        }

        [Fact]
        public async Task DeleteProduct_ShouldReturnOkResponse_WhenDataDeleted()
        {
            //Arrange
            string id = _fixture.Create<string>();
            _repositoryMock.Setup(x => x.DeleteProductAsync(id)).ReturnsAsync(1);

            //Act
            var result = await _controller.DeleteProduct(id).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task DeleteProduct_ShouldReturnNotFoundResponse_WhenDataNotDeleted()
        {
            //Arrange
            string id = _fixture.Create<string>();
            _repositoryMock.Setup(x => x.DeleteProductAsync(id)).ReturnsAsync(0);

            //Act
            var result = await _controller.DeleteProduct(id).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }


        [Fact]
        public async Task DeleteProduct_ShouldReturnInternalServerErrorResponse_WhenDataNotDeleted()
        {
            //Arrange
            string id = _fixture.Create<string>();
            _repositoryMock.Setup(x => x.DeleteProductAsync(id)).ReturnsAsync(2);

            //Act
            var result = await _controller.DeleteProduct(id).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
        }

        [Fact]
        public async Task GetProduct_ShouldReturnOkResponse_WhenDataFound()
        {
            //Arrange
            string id = _fixture.Create<string>();
            var responseMock = new ProductResponseModel(_fixture.Create<Product>());
            _repositoryMock.Setup(x => x.GetProductByIdAsync(id)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetProduct(id).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task GetProduct_ShouldReturnNotFoundResponse_WhenNoDataFound()
        {
            //Arrange
            string id = _fixture.Create<string>();
            var responseMock = new ProductResponseModel(null);
            _repositoryMock.Setup(x => x.GetProductByIdAsync(id)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetProduct(id).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task GetProduct_ShouldReturnInternalserverErrorResponse_WhenErrorOccured()
        {
            //Arrange
            string id = _fixture.Create<string>();
            var responseMock = new ProductResponseModel(null, "", true);
            _repositoryMock.Setup(x => x.GetProductByIdAsync(id)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetProduct(id).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
        }

        [Fact]
        public async Task GetProducts_ShouldReturnOkResponse_WhenDataFound()
        {
            //Arrange
            var responseMock = new[] { new ProductResponseModel(_fixture.Create<Product>()) };
            _repositoryMock.Setup(x => x.GetProductsAsync()).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetProducts().ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task GetProducts_ShouldReturnNotFoundResponse_WhenDataNotFound()
        {
            //Arrange
            var responseMock = new[] { new ProductResponseModel(null) };
            _repositoryMock.Setup(x => x.GetProductsAsync()).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetProducts().ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task GetProducts_ShouldReturnInternalServerResponse_WhenDataNotFound()
        {
            //Arrange
            var responseMock = new[] { new ProductResponseModel(null, "", true) };
            _repositoryMock.Setup(x => x.GetProductsAsync()).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetProducts().ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
        }
        [Fact]
        public async Task GetProductsByCategory_ShouldReturnOkResponse_WhenDataFound()
        {
            //Arrange
            string categoryId = _fixture.Create<string>();
            var responseMock = new[] { new ProductResponseModel(_fixture.Create<Product>()) };
            _repositoryMock.Setup(x => x.GetProductsByCategoryAsync(categoryId)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetProductsByCategory(categoryId).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }
        [Fact]
        public async Task GetProductsByCategory_ShouldReturnNotFoundResponse_WhenNoDataFound()
        {
            //Arrange
            string categoryId = _fixture.Create<string>();
            var responseMock = new[] { new ProductResponseModel(null) };
            _repositoryMock.Setup(x => x.GetProductsByCategoryAsync(categoryId)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetProductsByCategory(categoryId).ConfigureAwait(false);

            //Assert
            //Assert
            result.Should().NotBeNull();
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task GetProductsByCategory_ShouldReturnInternalServerErrorREesponse_WhenErrorOccured()
        {
            //Arrange
            string categoryId = _fixture.Create<string>();
            var responseMock = new[] { new ProductResponseModel(null,error:true) };
            _repositoryMock.Setup(x => x.GetProductsByCategoryAsync(categoryId)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetProductsByCategory(categoryId).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
        }

        [Fact]
        public async Task SearchProducts_ShouldReturnOkResponse_WhenDataFound()
        {
            //Arrange
            string searchString = _fixture.Create<string>();
            var responseMock = new[] { new ProductResponseModel(_fixture.Create<Product>()) };
            _repositoryMock.Setup(x=>x.SearchProductsAsync(searchString)).ReturnsAsync(responseMock);

            //Act
            var result =await _controller.SearchProducts(searchString).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task SearchProducts_ShouldReturnNotFoundResponse_WhenNoDataFound()
        {
            string searchString = _fixture.Create<string>();
            var responseMock = new[] { new ProductResponseModel(null) };
            _repositoryMock.Setup(x => x.SearchProductsAsync(searchString)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.SearchProducts(searchString).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }


        [Fact]
        public async Task SearchProducts_ShouldReturnInternalServerErrorResponse_WhenErrorOccured()
        {
            string searchString = _fixture.Create<string>();
            var responseMock = new[] { new ProductResponseModel(null,error:true) };
            _repositoryMock.Setup(x => x.SearchProductsAsync(searchString)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.SearchProducts(searchString).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
        }
    }
    
}
