using Microsoft.AspNetCore.Mvc;
using ReviewService.Models.RequestModels;
using ReviewService.Models.ResponseModels;
using ReviewService.Repositories.Interfaces;
using System.Net;
using System.Reflection;

namespace ReviewService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ILogger<CommentsController> _logger;
        private readonly ICommentsRepository _repository;
        public CommentsController(ILogger<CommentsController> logger, ICommentsRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(_logger));
        }

        [HttpPost("AddComment")]
        [ProducesResponseType(typeof(CommentResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<CommentResponseModel>> AddComment(CommentRequestModel Request)
        {
            var result = await ExecuteWithLogging(async () => await _repository.AddCommentAsync(Request));
            if (result.Data == null && !result.Error)
                return Unauthorized(result);
            if (result.Error)
                return StatusCode(500, result);
            return Ok(result);
        }

        [HttpPut("UpdateComment")]
        [ProducesResponseType(typeof(CommentResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<CommentResponseModel>> UpdateComment(CommentRequestModel Request)
        {
            var result = await ExecuteWithLogging(async () => await _repository.UpdateCommentAsync(Request));
            if (result.Data == null && !result.Error)
                return Unauthorized(result);
            if (result.Error)
                return StatusCode(500, result);
            return Ok(result);
        }

        [HttpGet("GetComment")]
        [ProducesResponseType(typeof(CommentResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<CommentResponseModel>> GetComment(string Id)
        {
            var result = await ExecuteWithLogging(async () => await _repository.GetCommentByIdAsync(Id));
            if (result.Data == null && !result.Error)
                return NotFound(result);
            if (result.Error)
                return StatusCode(500, result);
            return Ok(result);
        }

        [HttpGet("GetComments")]
        [ProducesResponseType(typeof(IEnumerable<CommentResponseModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IEnumerable<CommentResponseModel>>> GetComments()
        {
            var result = await ExecuteWithLogging(async () => await _repository.GetCommentsAsync());
            var first = result.FirstOrDefault();
            if (first.Data == null && !first.Error)
                return NotFound(result);
            if (first.Error)
                return StatusCode(500, result);
            return Ok(result);
        }

        [HttpGet("GetCommentsByUserId")]
        [ProducesResponseType(typeof(IEnumerable<CommentResponseModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IEnumerable<CommentResponseModel>>> GetCommentsByUserId(string userId)
        {
            var result = await ExecuteWithLogging(async () => await _repository.GetCommentsByUserIdAsync(userId));
            var first = result.FirstOrDefault();
            if (first.Data == null && !first.Error)
                return NotFound(result);
            if (first.Error)
                return StatusCode(500, result);
            return Ok(result);
        }

        [HttpGet("GetCommentsByPostId")]
        [ProducesResponseType(typeof(IEnumerable<CommentResponseModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IEnumerable<CommentResponseModel>>> GetComments(string postId)
        {
            var result = await ExecuteWithLogging(async () => await _repository.GetCommentsForPostAsync(postId));
            var first = result.FirstOrDefault();
            if (first.Data == null && !first.Error)
                return NotFound(result);
            if (first.Error)
                return StatusCode(500, result);
            return Ok(result);
        }

        [HttpDelete("DeleteComment")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> DeleteComment(string Id)
        {
            return await ExecuteActionAsync(async () => await _repository.DeleteCommentAsync(Id));
        }

        #region Helpers
        private async Task<CommentResponseModel> ExecuteWithLogging(Func<Task<CommentResponseModel>> action)
        {
            var logName = MethodBase.GetCurrentMethod()?.Name;
            _logger.LogInformation("[BEGIN] " + logName);
            var result = await action.Invoke();
            _logger.LogInformation("[END] " + logName);
            return result;
        }

        private async Task<IEnumerable<CommentResponseModel>> ExecuteWithLogging(Func<Task<IEnumerable<CommentResponseModel>>> action)
        {
            var logName = MethodBase.GetCurrentMethod()?.Name;

            _logger.LogInformation("[BEGIN] " + logName);
            var result = await action.Invoke();
            _logger.LogInformation("[END] " + logName);
            return result;
        }

        private async Task<ActionResult> ExecuteActionAsync(Func<Task<long>> action)
        {
            var logName = MethodBase.GetCurrentMethod()?.Name;
            _logger.LogInformation("[BEGIN] " + logName);

            var result = await action();

            _logger.LogInformation("[END] " + logName);

            if (result == 1)
                return Ok("Successfully completed the action");
            if (result == 0)
                return NotFound("Action was not completed");
            else
                return StatusCode(500, $"Multiple actions were completed: {logName}, number of actions completed: {result}");
        }
        #endregion
    }
}
