using AutoFixture;
using FarmLink.OrderService.Models;
using FarmLink.Shared.Tests;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using OrderService.Controllers;
using OrderService.Repositories.Interfaces;
using System.Net;
using ZstdSharp.Unsafe;

namespace OrderService.Tests.Controllers
{
    public class OrdersControllerTests: IClassFixture<LoggerFixture<OrdersController>>
    {
        private readonly IFixture _fixture;
        private readonly Mock<IOrderRepository> _repositoryMock;
        private readonly OrdersController _controller;
        private readonly ILogger<OrdersController> _loggerMock;
        private readonly LoggerFixture<OrdersController> _loggerFixture;
        public OrdersControllerTests()
        {
            _fixture = new Fixture();
            _repositoryMock = _fixture.Freeze<Mock<IOrderRepository>>();
            _loggerFixture = new LoggerFixture<OrdersController>();
            _loggerMock = _loggerFixture.GetRegisteredLogger();
            _controller = new OrdersController( _repositoryMock.Object,_loggerMock);
        }

        [Fact]
        public async Task AddOrder_ShouldReturnOkResponse_WhenValidModel()
        {
            //Arrange
            var requestMock = _fixture.Create<OrderRequestModel>();
            var responseMock = new OrderResponseModel(_fixture.Create<Order>());
            _repositoryMock.Setup(x => x.AddOrderAsync(requestMock)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.AddOrder(requestMock).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }


        [Fact]
        public async Task AddOrder_ShouldReturnBadRequestResponse_WhenInvalidModel()
        {
            //Arrange
            var requestMock = _fixture.Create<OrderRequestModel>();
            var responseMock = new OrderResponseModel(null);
            _repositoryMock.Setup(x => x.AddOrderAsync(requestMock)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.AddOrder(requestMock).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var badReqeustResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.BadRequest, badReqeustResult.StatusCode);
        }

        [Fact]
        public async Task AddOrder_ShouldReturnInternalServerErrorResponse_WhenErrorOccured()
        {
            //Arrange
            var requestMock = _fixture.Create<OrderRequestModel>();
            var responseMock = new OrderResponseModel(null,error:true);
            _repositoryMock.Setup(x => x.AddOrderAsync(requestMock)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.AddOrder(requestMock).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
        }


        [Fact]

        public async Task GetOrder_ShouldReturnOkResponse_WhenDataFound()
        {
            //Arrange
            string id = _fixture.Create<string>();
            var responseMock = new OrderResponseModel(_fixture.Create<Order>());
            _repositoryMock.Setup(x=>x.GetOrderAsync(id)).ReturnsAsync(responseMock);

            //Act
            var result =await _controller.GetOrder(id).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }


        [Fact]

        public async Task GetOrder_ShouldReturnNotFoundResponse_WhenNoDataFound()
        {
            //Arrange
            string id = _fixture.Create<string>();
            var responseMock = new OrderResponseModel(null);
            _repositoryMock.Setup(x => x.GetOrderAsync(id)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetOrder(id).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Fact]

        public async Task GetOrder_ShouldReturnInternalServerErrorResponse_WhenErrorFound()
        {
            //Arrange
            string id = _fixture.Create<string>();
            var responseMock = new OrderResponseModel(null,error:true);
            _repositoryMock.Setup(x => x.GetOrderAsync(id)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetOrder(id).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
        }

        [Fact]
        public async Task GetOrders_ShouldReturnOkResponse_WhenDataFound()
        {
            //Arrange
            var responseMock = new[] { new OrderResponseModel(_fixture.Create<Order>()) };
            _repositoryMock.Setup(x=>x.GetOrdersAsync()).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetOrders().ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task GetOrders_ShouldReturnNotFoundResponse_WhenNoDataFound()
        {
            //Arrange
            var responseMock = new[] { new OrderResponseModel(null) };
            _repositoryMock.Setup(x => x.GetOrdersAsync()).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetOrders().ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task GetOrders_ShouldReturnInternalServerErrorResponse_WhenErrorFound()
        {
            //Arrange
            var responseMock = new[] { new OrderResponseModel(null,error:true) };
            _repositoryMock.Setup(x => x.GetOrdersAsync()).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetOrders().ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
        }

        [Fact]
        public async Task UpdateOrder_ShouldReturnOkResponse_WhenValidModel()
        {
            //Arrange
            var requestMock = _fixture.Create<OrderRequestModel>();
            var responseMock = new OrderResponseModel(_fixture.Create<Order>());
            _repositoryMock.Setup(x => x.UpdateOrderAsync(requestMock)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.UpdateOrder(requestMock).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }


        [Fact]
        public async Task UpdateOrder_ShouldReturnNotFoundResponse_WhenNoDataFound()
        {
            //Arrange
            var requestMock = _fixture.Create<OrderRequestModel>();
            var responseMock = new OrderResponseModel(null);
            _repositoryMock.Setup(x => x.UpdateOrderAsync(requestMock)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.UpdateOrder(requestMock).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task UpdateOrder_ShouldReturnInternalServerErrorResponse_WhenErrorOccured()
        {
            //Arrange
            var requestMock = _fixture.Create<OrderRequestModel>();
            var responseMock = new OrderResponseModel(null, error: true);
            _repositoryMock.Setup(x => x.UpdateOrderAsync(requestMock)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.UpdateOrder(requestMock).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
        }

        [Fact]
        public async Task DeleteOrder_ShouldReturnOkResponse_WhenDataDeleted()
        {
            //Arrange
            string id = _fixture.Create<string>();
            _repositoryMock.Setup(x => x.DeleteOrderAsync(id)).ReturnsAsync(1);

            //Act
            var result =await _controller.DeleteOrder(id).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task DeleteOrder_ShouldReturnNotFoundResponse_WhenDataNotFound()
        {
            //Arrange
            string id = _fixture.Create<string>();
            _repositoryMock.Setup(x => x.DeleteOrderAsync(id)).ReturnsAsync(0);

            //Act
            var result = await _controller.DeleteOrder(id).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task DeleteOrder_ShouldReturnInternalServerErrorResponse_WhenEnrrorOccured()
        {
            //Arrange
            string id = _fixture.Create<string>();
            _repositoryMock.Setup(x => x.DeleteOrderAsync(id)).ReturnsAsync(3);

            //Act
            var result = await _controller.DeleteOrder(id).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
        }


        [Fact]
        public async Task SoftDeleteOrder_ShouldReturnOkResponse_WhenDataDeleted()
        {
            //Arrange
            string id = _fixture.Create<string>();
            _repositoryMock.Setup(x => x.SoftDeleteOrderAsync(id)).ReturnsAsync(1);

            //Act
            var result = await _controller.SoftDeleteOrder(id).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task SoftDeleteOrder_ShouldReturnNotFoundResponse_WhenDataNotFound()
        {
            //Arrange
            string id = _fixture.Create<string>();
            _repositoryMock.Setup(x => x.SoftDeleteOrderAsync(id)).ReturnsAsync(0);

            //Act
            var result = await _controller.SoftDeleteOrder(id).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task SoftDeleteOrder_ShouldReturnInternalServerErrorResponse_WhenEnrrorOccured()
        {
            //Arrange
            string id = _fixture.Create<string>();
            _repositoryMock.Setup(x => x.SoftDeleteOrderAsync(id)).ReturnsAsync(3);

            //Act
            var result = await _controller.SoftDeleteOrder(id).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
        }
    }
}
