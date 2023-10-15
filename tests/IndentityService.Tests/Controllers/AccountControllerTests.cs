

using AutoFixture;
using FarmLink.IndentityService.Models;
using FarmLink.IndentityService.Models.RequestModels;
using FarmLink.Shared.Services;
using FarmLink.Shared.Tests;
using FluentAssertions;
using IdentityService.Controllers;
using IdentityService.Repositories.Interfaces;
using IdentityService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

namespace IndentityService.Tests.Controllers
{
    public class AccountControllerTests: IClassFixture<LoggerFixture<AccountController>>
    {
        private readonly IFixture _fixture;
        private readonly Mock<IUserRepository> _repositoryMock;
        private readonly AccountController _controller;
        private readonly ILogger<AccountController> _loggerMock;
        private readonly LoggerFixture<AccountController> _loggerFixture;
        private readonly Mock<IEmailService> _emailMock;
        private readonly Mock<IHashingService> _hashingServiceMock;
        public AccountControllerTests()
        {
            _fixture = new Fixture();
            _repositoryMock = _fixture.Freeze<Mock<IUserRepository>>();
            _loggerFixture = new LoggerFixture<AccountController>();
            _loggerMock = _loggerFixture.GetRegisteredLogger();
            _emailMock = new Mock<IEmailService>();
            _hashingServiceMock = new Mock<IHashingService>();
            _controller = new AccountController(_repositoryMock.Object,_loggerMock,_emailMock.Object,_hashingServiceMock.Object);
        }

        [Fact]
        public async Task AddUser_ShouldReturnOkResponse_WhenModelValid()
        {
            //Arrnage
            var requestMock = _fixture.Create<UserRequestModel>();
            var responseMock = new UserResponseModel(_fixture.Create<User>());
            _repositoryMock.Setup(x=>x.AddUserAsync(requestMock)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.AddUser(requestMock).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task AddUser_ShouldReturnBadRequestResponse_WhenModelInValid()
        {
            //Arrnage
            var requestMock = _fixture.Create<UserRequestModel>();
            var responseMock = new UserResponseModel(null);
            _repositoryMock.Setup(x => x.AddUserAsync(requestMock)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.AddUser(requestMock).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var badReqeustResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.BadRequest, badReqeustResult.StatusCode);
        }

        [Fact]
        public async Task AddUser_ShouldReturnInternalServerErrorResponse_WhenErrorOccured()
        {
            //Arrnage
            var requestMock = _fixture.Create<UserRequestModel>();
            var responseMock = new UserResponseModel(null,error:true);
            _repositoryMock.Setup(x => x.AddUserAsync(requestMock)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.AddUser(requestMock).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
        }

        [Fact]
        public async Task Login_ShouldReturnOkResponse_WhenDataFound()
        {
            //Arrange
            var requestMock =_fixture.Create<LoginRequestModel>();
            var responseMock = new UserResponseModel(_fixture.Create<User>());
            _repositoryMock.Setup(x => x.GetUserByEmailAsync(requestMock.Email)).ReturnsAsync(responseMock);
            _repositoryMock.Setup(x => x.LoginAsync(requestMock)).ReturnsAsync(responseMock);
            //Act
            var result =await _controller.Login(requestMock).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }
    }
}
