

using AutoFixture;
using FarmLink.Shared.Services;
using FarmLink.Shared.Tests;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Moq;
using ReviewService.Controllers;
using ReviewService.Models;
using ReviewService.Models.RequestModels;
using ReviewService.Models.ResponseModels;
using ReviewService.Repositories.Interfaces;
using System.Net;
using System.Net.WebSockets;

namespace ReviewService.Tests.Controllers
{
    public class CommentsControllerTests : IClassFixture<LoggerFixture<CommentsController>>
    {
        private readonly IFixture _fixture;
        private readonly Mock<ICommentsRepository> _repositoryMock;
        private readonly CommentsController _controller;
        private readonly ILogger<CommentsController> _loggerMock;
        private readonly LoggerFixture<CommentsController> _loggerFixture;
        public CommentsControllerTests()
        {
            _fixture = new Fixture();
            _repositoryMock = _fixture.Freeze<Mock<ICommentsRepository>>();
            _loggerFixture = new LoggerFixture<CommentsController>();
            _loggerMock = _loggerFixture.GetRegisteredLogger();
            _controller = new CommentsController(_loggerMock, _repositoryMock.Object);
        }

        [Fact]
        public async Task AddComment_ShouldReturnOkResponse_WhenModelIsValid()
        {
            //Arrange
            var requestMock =_fixture.Create<CommentRequestModel>();
            var responseMock = new CommentResponseModel(_fixture.Create<Comment>());
            _repositoryMock.Setup(x => x.AddCommentAsync(requestMock)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.AddComment(requestMock).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task AddComment_ShouldReturnBadRequestResponse_WhenDataNotInserted()
        {
            //Arrange
            var requestMock = _fixture.Create<CommentRequestModel>();
            var responseMock = new CommentResponseModel(null);
            _repositoryMock.Setup(x => x.AddCommentAsync(requestMock)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.AddComment(requestMock).ConfigureAwait(false);

            //Assert
            var badReqeustResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.BadRequest, badReqeustResult.StatusCode);
        }

        [Fact]
        public async Task AddComment_ShouldInternalServerErrorResponse_WhenErrorOccured()
        {
            //Arrange
            var requestMock = _fixture.Create<CommentRequestModel>();
            var responseMock = new CommentResponseModel(null,"",true);
            _repositoryMock.Setup(x => x.AddCommentAsync(requestMock)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.AddComment(requestMock).ConfigureAwait(false);

            //Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
        }

        [Fact]
        public async Task UpdateComment_ShouldReturnOkResponse_WhenDataFound()
        {
            //Arrange
            var requestMock =_fixture.Create<CommentRequestModel>();
            var responseMock = new CommentResponseModel(_fixture.Create<Comment>());
            _repositoryMock.Setup(x => x.UpdateCommentAsync(requestMock)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.UpdateComment(requestMock).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task UpdateComment_ShouldReturnBadRequestResponse_WhenDataNotInserted()
        {
            //Arrange
            var requestMock = _fixture.Create<CommentRequestModel>();
            var responseMock = new CommentResponseModel(null);
            _repositoryMock.Setup(x => x.UpdateCommentAsync(requestMock)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.UpdateComment(requestMock).ConfigureAwait(false);

            //Assert
            var badReqeustResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.BadRequest, badReqeustResult.StatusCode);
        }

        [Fact]
        public async Task UpdateComment_ShouldInternalServerErrorResponse_WhenErrorOccured()
        {
            //Arrange
            var requestMock = _fixture.Create<CommentRequestModel>();
            var responseMock = new CommentResponseModel(null, "", true);
            _repositoryMock.Setup(x => x.UpdateCommentAsync(requestMock)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.UpdateComment(requestMock).ConfigureAwait(false);

            //Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
        }

        [Fact]
        public async Task GetComment_ShouldReturnOkresponse_WhenDataFound()
        {
            //Arrange
            var id = _fixture.Create<string>();
            var responseMock = new CommentResponseModel(_fixture.Create<Comment>());
            _repositoryMock.Setup(x=>x.GetCommentByIdAsync(id)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetComment(id);

            //Assert
            result.Should().NotBeNull();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task GetComment_ShouldReturnNotFound_WhenDataNotFound()
        {
            //Arrange
            var id = _fixture.Create<string>();
            var responseMock = new CommentResponseModel(null);
            _repositoryMock.Setup(x => x.GetCommentByIdAsync(id)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetComment(id);

            //Assert
            result.Should().NotBeNull();
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task GetComment_ShouldReturnInternalServerError_WhenErrorOccured()
        {
            //Arrange
            var id = _fixture.Create<string>();
            var responseMock = new CommentResponseModel(null,"",true);
            _repositoryMock.Setup(x => x.GetCommentByIdAsync(id)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetComment(id);

            //Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
        }

        [Fact]
        public async Task GetComments_ShouldReturnOkresponse_WhenDataFound()
        {
            //Arrange
            var responseMock = new[] { new CommentResponseModel(_fixture.Create<Comment>()) };
            _repositoryMock.Setup(x => x.GetCommentsAsync()).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetComments();

            //Assert
            result.Should().NotBeNull();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task GetComments_ShouldReturnNotFound_WhenDataNotFound()
        {
            //Arrange
            var responseMock = new[] { new CommentResponseModel(null) };
            _repositoryMock.Setup(x => x.GetCommentsAsync()).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetComments();

            //Assert
            result.Should().NotBeNull();
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task GetComments_ShouldReturnInternalServerError_WhenErrorOccured()
        {
            //Arrange
            var responseMock = new[] { new CommentResponseModel(null,"",true) };
            _repositoryMock.Setup(x => x.GetCommentsAsync()).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetComments();

            //Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
        }

        [Fact]
        public async Task GetCommentsByUserId_ShouldReturnOkresponse_WhenDataFound()
        {
            //Arrange
            string id = _fixture.Create<string>();
            var responseMock = new[] { new CommentResponseModel(_fixture.Create<Comment>()) };
            _repositoryMock.Setup(x => x.GetCommentsByUserIdAsync(id)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetCommentsByUserId(id);

            //Assert
            result.Should().NotBeNull();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task GetCommentsByUserId_ShouldReturnNotFound_WhenDataNotFound()
        {
            //Arrange
            string id =_fixture.Create<string>();
            var responseMock = new[] { new CommentResponseModel(null) };
            _repositoryMock.Setup(x => x.GetCommentsByUserIdAsync(id)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetCommentsByUserId(id);

            //Assert
            result.Should().NotBeNull();
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task GetCommentsByUserId_ShouldReturnInternalServerError_WhenErrorOccured()
        {
            //Arrange
            string id = _fixture.Create<string>();
            var responseMock = new[] { new CommentResponseModel(null, "", true) };
            _repositoryMock.Setup(x => x.GetCommentsByUserIdAsync(id)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetCommentsByUserId(id);

            //Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
        }


        [Fact]
        public async Task GetCommentsByPostId_ShouldReturnOkresponse_WhenDataFound()
        {
            //Arrange
            string id = _fixture.Create<string>();
            var responseMock = new[] { new CommentResponseModel(_fixture.Create<Comment>()) };
            _repositoryMock.Setup(x => x.GetCommentsForPostAsync(id)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetCommentsByPostId(id);

            //Assert
            result.Should().NotBeNull();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task GetCommentsByPostId_ShouldReturnNotFound_WhenDataNotFound()
        {
            //Arrange
            string id = _fixture.Create<string>();
            var responseMock = new[] { new CommentResponseModel(null) };
            _repositoryMock.Setup(x => x.GetCommentsForPostAsync(id)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetCommentsByPostId(id);

            //Assert
            result.Should().NotBeNull();
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task GetCommentsByPostId_ShouldReturnInternalServerError_WhenErrorOccured()
        {
            //Arrange
            string id = _fixture.Create<string>();
            var responseMock = new[] { new CommentResponseModel(null, "", true) };
            _repositoryMock.Setup(x => x.GetCommentsForPostAsync(id)).ReturnsAsync(responseMock);

            //Act
            var result = await _controller.GetCommentsByPostId(id);

            //Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
        }

        [Fact]
        public async Task DeleteComment_ShouldReturnOkResponse_WhenDataDeleted()
        {
            //Arrange
            string id =_fixture.Create<string>();
            _repositoryMock.Setup(x => x.DeleteCommentAsync(id)).ReturnsAsync(1);

            //Act
            var result = await _controller.DeleteComment(id).ConfigureAwait(false);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task DeleteComment_ShouldReturnNotFoundResponse_WhenDataNotDeleted()
        {
            //Arrange
            string id = _fixture.Create<string>();
            _repositoryMock.Setup(x => x.DeleteCommentAsync(id)).ReturnsAsync(0);

            //Act
            var result = await _controller.DeleteComment(id).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task DeleteComment_ShouldReturnInternalServerError_WhenMoreThanOneCommentDeleted()
        {
            //Arrange
            string id = _fixture.Create<string>();
            _repositoryMock.Setup(x => x.DeleteCommentAsync(id)).ReturnsAsync(2);

            //Act
            var result = await _controller.DeleteComment(id).ConfigureAwait(false);

            //Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
        }
    }
}
