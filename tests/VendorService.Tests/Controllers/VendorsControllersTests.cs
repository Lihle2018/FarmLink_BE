using AutoFixture;
using FarmLink.Shared.Tests;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using System.Net.WebSockets;
using VendorService.Controllers;
using VendorService.Models;
using VendorService.Models.RequestModels;
using VendorService.Models.ResponseModels;
using VendorService.Repositories.Interfaces;

namespace VendorService.Tests.Controllers
{
    public class VendorsControllersTests : IClassFixture<LoggerFixture<VendorsController>>
    {
        private readonly IFixture _fixture;
        private readonly Mock<IVendorRepository> _repositoryMock;
        private readonly VendorsController _controller;
        private readonly ILogger<VendorsController> _loggerMock;
        private readonly LoggerFixture<VendorsController> _loggerFixture;
        public VendorsControllersTests()
        {
            _fixture = new Fixture();
            _repositoryMock = _fixture.Freeze<Mock<IVendorRepository>>();
            _loggerFixture = new LoggerFixture<VendorsController>();
            _loggerMock = _loggerFixture.GetRegisteredLogger();
            _controller = new VendorsController(_loggerMock, _repositoryMock.Object);
        }

        [Fact]
        public async Task AddVendor_ShouldReturnOkResponse_WhenValidModel()
        {
            //Arrange
            var responseMock = new VendorResponseModel(new Vendor());
            var requestMock = _fixture.Create<VendorRequestModel>();
            _repositoryMock.Setup(x => x.CreateVendorAsync(requestMock)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.AddVendor(requestMock).ConfigureAwait(false);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<VendorResponseModel>>();
            _repositoryMock.Verify(x => x.CreateVendorAsync(requestMock), Times.Once());
        }

        [Fact]
        public async Task AddVendor_ShouldReturnBadRequestResponse_WhenInvalidModel()
        {
            //Arrange
            VendorRequestModel requestMock = null;

            //Act
            var result = await _controller.AddVendor(requestMock);

            //Assert
            result.Result.Should().BeAssignableTo<BadRequestObjectResult>();
        }

        [Fact]
        public async Task AddVendor_ShouldReturnsInternalServerError_WhenErrorFound()
        {
            //Arrange
            var requestMock =_fixture.Create<VendorRequestModel>();
            var responseMock = _fixture.Create<VendorResponseModel>();
            _repositoryMock.Setup(x => x.CreateVendorAsync(requestMock)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.AddVendor(requestMock).ConfigureAwait(false);

            //Assert
            result.Result.Should().BeOfType<ObjectResult>();
            var objectResult = result.Result as ObjectResult;
            objectResult.StatusCode.Should().Be(500);
        }

        [Fact]
        public async Task UpdateVendor_ShouldReturnOkResponse_WhenDataFound()
        {

            //Arrange
            var responseMock = new VendorResponseModel(new Vendor());
            var requestMock = _fixture.Create<VendorRequestModel>();
            _repositoryMock.Setup(x => x.UpdateVendorAsync(requestMock)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.UpdateVendor(requestMock).ConfigureAwait(false);

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<VendorResponseModel>>();
            _repositoryMock.Verify(x => x.UpdateVendorAsync(requestMock), Times.Once());
        }

        [Fact]
        public async Task UpdateVendor_ShouldReturnNotFoundResponse_WhenNDataNotFound()
        {
            //Arrange
            var requestMock = _fixture.Create<VendorRequestModel>();
            var responseMock = new VendorResponseModel(null);
            _repositoryMock.Setup(x => x.UpdateVendorAsync(requestMock)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.UpdateVendor(requestMock).ConfigureAwait(false);

            //Assert
            result.Result.Should().BeAssignableTo<NotFoundObjectResult>();
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }


        [Fact]
        public async Task UpdateVendor_ShouldReturnsInternalServerError_WhenErrorFound()
        {
            //Arrange
            var requestMock = _fixture.Create<VendorRequestModel>();
            var responseMock = new VendorResponseModel(null,"Error found",true);
            _repositoryMock.Setup(x => x.UpdateVendorAsync(requestMock)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.UpdateVendor(requestMock).ConfigureAwait(false);

            // Assert
            result.Result.Should().BeOfType<ObjectResult>();
            var objectResult = result.Result as ObjectResult;
            objectResult.StatusCode.Should().Be(500);
        }

        [Fact]
        public async Task DeleteVendor_ShouldReturnOkResponse_WhenDataFound()
        {
            //Arrange
            var Id = _fixture.Create<string>();
            _repositoryMock.Setup(x => x.DeleteVendorAsync(Id)).ReturnsAsync(1);
            //Act
            var result =await _controller.DeleteVendor(Id).ConfigureAwait(false);

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            _repositoryMock.Verify(x => x.DeleteVendorAsync(Id), Times.Once());
        }

        [Fact]
        public async Task DeleteVendor_ShouldReturnNotFoundResponse_WhenNodataFound()
        {
            //Arrange
            var Id = _fixture.Create<string>();
            _repositoryMock.Setup(x => x.DeleteVendorAsync(Id)).ReturnsAsync(0);

            //Act
            var result = await _controller.DeleteVendor(Id);

            //Assert
            result.Should().BeOfType<NotFoundObjectResult>();

        }

        [Fact]
        public async Task DeleteVendor_ShouldReturnInternalServerError_WhenErrorFound()
        {
            //Arrange
            var Id = _fixture.Create<string>();
            _repositoryMock.Setup(x => x.DeleteVendorAsync(Id)).ReturnsAsync(2);

            //Act
            var result = await _controller.DeleteVendor(Id);

            //Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
        }

        [Fact]
        public async Task GetVendors_ShouldReturnOkResponse_WhenDataFound()
        {
            //Arrange
            var vendorsMock = new[] { new VendorResponseModel(_fixture.Create<Vendor>()) };
            _repositoryMock.Setup(x => x.GetVendorsAsync()).ReturnsAsync(vendorsMock);

            //Act
            var result = await _controller.GetVendors().ConfigureAwait(false);

            //Assert
            Assert.NotNull(result);
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<IEnumerable<VendorResponseModel>>>();
            result.Result.Should().BeAssignableTo<ObjectResult>();
            result.Result.As<OkObjectResult>().Value
                .Should()
                .NotBeNull()
                .And.BeOfType(vendorsMock.GetType());
            _repositoryMock.Verify(x => x.GetVendorsAsync(), Times.Once());
        }

        [Fact]
        public async Task GetVendors_ShouldReturnsNotFound_WhenDataNotFound()
        {
            //Arrange
            var vendorMock = new[] { new VendorResponseModel(null) };
            _repositoryMock.Setup(x => x.GetVendorsAsync()).ReturnsAsync(vendorMock);

            //Act
            var result = await _controller.GetVendors().ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            result.Result.Should().BeAssignableTo<NotFoundObjectResult>();
            _repositoryMock.Verify(x => x.GetVendorsAsync(), Times.Once());
        }

        [Fact]
        public async Task GetVendors_ShouldReturnInternalServerError_WhenErrorFound()
        {
            //Arrange
            var responseMock = new[] { new VendorResponseModel(null, "", true) };
            _repositoryMock.Setup(x => x.GetVendorsAsync()).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetVendors().ConfigureAwait(false);

            //Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
        }
        [Fact]
        public async Task GetVendorsByProduct_ShouldReturnOkResponse_WhenDataFound()
        {
            //Arrange
            var productMock = new[] { new VendorResponseModel(_fixture.Create<Vendor>()) };
            var Id = _fixture.Create<string>();
            _repositoryMock.Setup(x => x.GetVendorsByProductAsync(Id)).ReturnsAsync(productMock);

            //Act
            var result =await _controller.GetVendorsByProduct(Id).ConfigureAwait(false);

            //Assert
            Assert.NotNull(result);
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<IEnumerable<VendorResponseModel>>>();
            result.Result.Should().BeAssignableTo<ObjectResult>();
            result.Result.As<OkObjectResult>().Value
                .Should()
                .NotBeNull()
                .And.BeOfType(productMock.GetType());
            _repositoryMock.Verify(x => x.GetVendorsByProductAsync(Id), Times.Once());
        }

        [Fact]
        public async Task GetVendorsByProduct_ShouldReturnNotFoundResponse_WhenDataNotFound()
        {
            //Arrange
            var vendorMock = new[] { new VendorResponseModel(null) };
            string id=_fixture.Create<string>();
            _repositoryMock.Setup(x => x.GetVendorsByProductAsync(id)).ReturnsAsync(vendorMock);

            //Act
            var result = await _controller.GetVendorsByProduct(id).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            result.Result.Should().BeAssignableTo<NotFoundObjectResult>();
            _repositoryMock.Verify(x => x.GetVendorsByProductAsync(id), Times.Once());
        }

        [Fact]
        public async Task GetVendorsByProduct_ShouldReturnInternalServerError_WhenErrorFound()
        {
            //Arrange
            var vendorMock = new[] { new VendorResponseModel(null,"",true) };
            string id = _fixture.Create<string>();
            _repositoryMock.Setup(x => x.GetVendorsByProductAsync(id)).ReturnsAsync(vendorMock);

            //Act
            var result = await _controller.GetVendorsByProduct(id).ConfigureAwait(false);

            //Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
        }

        [Fact]
        public async Task GetVendorsByLocation_ShouldReturnOkResponse_WhenDataFound()
        {
            //Arrange
            var vendorMock = new[] { new VendorResponseModel(_fixture.Create<Vendor>()) };
            string id= _fixture.Create<string>();
            _repositoryMock.Setup(x => x.GetVendorsByLocationAsync(id)).ReturnsAsync(vendorMock);

            //Act
            var result = await _controller.GetVendorsByLocation(id).ConfigureAwait(false);

            //Assert
            Assert.NotNull(result);
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<IEnumerable<VendorResponseModel>>>();
            result.Result.Should().BeAssignableTo<ObjectResult>();
            result.Result.As<OkObjectResult>().Value
                .Should()
                .NotBeNull()
                .And.BeOfType(vendorMock.GetType());
            _repositoryMock.Verify(x => x.GetVendorsByLocationAsync(id), Times.Once());
        }

        [Fact]
        public async Task GetVendorsByLocation_ShouldReturnNotFoundResponse_WhenDataNotFound()
        {
            //Arrange
            var vendorMock = new[] { new VendorResponseModel(null) };
            string id = _fixture.Create<string>();
            _repositoryMock.Setup(x => x.GetVendorsByLocationAsync(id)).ReturnsAsync(vendorMock);

            //Act
            var result = await _controller.GetVendorsByLocation(id).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            result.Result.Should().BeAssignableTo<NotFoundObjectResult>();
            _repositoryMock.Verify(x => x.GetVendorsByLocationAsync(id), Times.Once());
        }

        [Fact]
        public async Task GetVendorsByLocation_ShouldReturnInternalServerErrorResponse_WhenErrorOccur()
        {
            //Arrange
            var vendorMock = new[] { new VendorResponseModel(null,"",true) };
            string id = _fixture.Create<string>();
            _repositoryMock.Setup(x => x.GetVendorsByLocationAsync(id)).ReturnsAsync(vendorMock);

            //Act
            var result = await _controller.GetVendorsByLocation(id).ConfigureAwait(false);

            //Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
        }
        [Fact]
        public async Task GetVendor_ShouldReturnOkResponse_WhenDataFound()
        {
            //Arrange
            string id =_fixture.Create<string>();
            var vendorMock=_fixture.Create<VendorResponseModel>();
            _repositoryMock.Setup(x=>x.GetVendorByIdAsync(id)).ReturnsAsync(vendorMock);

            //Act
            var result = await _controller.GetVendor(id).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<VendorResponseModel>>();
            _repositoryMock.Verify(x => x.GetVendorByIdAsync(id), Times.Once());
        }

        [Fact]
        public async Task GetVendor_ShouldReturnNotFoundResponse_WhenNoDataFound()
        {
            //Arrange
            string id =_fixture.Create<string> ();
            var responseMock = new VendorResponseModel(null);
            _repositoryMock.Setup(x => x.GetVendorByIdAsync(id)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetVendor(id).ConfigureAwait(false);

            //Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task GetVendor_ShouldReturnInternalServerError_WhenErrorFound()
        {
            //Arrange
            string id = _fixture.Create<string>();
            var responseMock=new VendorResponseModel(null,"",true);
            _repositoryMock.Setup(x => x.GetVendorByIdAsync(id)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetVendor(id).ConfigureAwait(false);

            //Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
        }


    }
}
