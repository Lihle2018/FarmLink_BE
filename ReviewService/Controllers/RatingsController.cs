using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReviewService.Models.ResponseModels;
using ReviewService.Models;
using ReviewService.Repositories.Interfaces;
using System.Reflection;
using ReviewService.Models.RequestModels;
using System.Net;

namespace ReviewService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingsController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IRatingsRepository _repository;
        public RatingsController(ILogger logger, IRatingsRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("AddRating")]
        [ProducesResponseType(typeof(RatingResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<RatingResponseModel>> AddRating(RatingRequestModel Request)
        {
            var result = await ExecuteWithLogging(async () => await _repository.AddRatingAsync(Request));
            if (result.Data == null && !result.Error)
                return Unauthorized(result);
            if (result.Error)
                return StatusCode(500, result);
            return Ok(result);
        }

        [HttpPut("UpdateRating")]
        [ProducesResponseType(typeof(RatingResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<RatingResponseModel>> UpdateRating(RatingRequestModel Request)
        {
            var result = await ExecuteWithLogging(async () => await _repository.UpdateRatingAsync(Request));
            if (result.Data == null && !result.Error)
                return Unauthorized(result);
            if (result.Error)
                return StatusCode(500, result);
            return Ok(result);
        }


        [HttpGet("GetRating")]
        [ProducesResponseType(typeof(RatingResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<RatingResponseModel>> GetRating(string Id)
        {
            var result = await ExecuteWithLogging(async () => await _repository.GetRatingByIdAsync(Id));
            if (result.Data == null && !result.Error)
                return NotFound(result);
            if (result.Error)
                return StatusCode(500, result);
            return Ok(result);
        }

        [HttpGet("GetRatings")]
        [ProducesResponseType(typeof(IEnumerable<RatingResponseModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IEnumerable<RatingResponseModel>>> GetRatings()
        {
            var result = await ExecuteWithLogging(async () => await _repository.GetRatingsAsync());
            var first = result.FirstOrDefault();
            if (first.Data == null && !first.Error)
                return NotFound(result);
            if (first.Error)
                return StatusCode(500, result);
            return Ok(result);
        }

        [HttpGet("GetRatingsByUserId")]
        [ProducesResponseType(typeof(IEnumerable<RatingResponseModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IEnumerable<RatingResponseModel>>> GetRatingsByUserId(string userId)
        {
            var result = await ExecuteWithLogging(async () => await _repository.GetRatingsByUserIdAsync(userId));
            var first = result.FirstOrDefault();
            if (first.Data == null && !first.Error)
                return NotFound(result);
            if (first.Error)
                return StatusCode(500, result);
            return Ok(result);
        }

        [HttpGet("GetRatingsByPostId")]
        [ProducesResponseType(typeof(IEnumerable<RatingResponseModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IEnumerable<RatingResponseModel>>> GetRatings(string postId)
        {
            var result = await ExecuteWithLogging(async () => await _repository.GetRatingsForPostAsync(postId));
            var first = result.FirstOrDefault();
            if (first.Data == null && !first.Error)
                return NotFound(result);
            if (first.Error)
                return StatusCode(500, result);
            return Ok(result);
        }

        [HttpDelete("DeleteRating")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> DeleteRating(string Id)
        {
            return await ExecuteActionAsync(async () => await _repository.DeleteRatingAsync(Id));
        }

        #region Helpers
        private async Task<RatingResponseModel> ExecuteWithLogging(Func<Task<RatingResponseModel>> action)
        {
            var logName = MethodBase.GetCurrentMethod()?.Name;
            _logger.LogInformation("[BEGIN] " + logName);
            var result = await action.Invoke();
            _logger.LogInformation("[END] " + logName);
            return result;

        }
        private async Task<IEnumerable<RatingResponseModel>> ExecuteWithLogging(Func<Task<IEnumerable<RatingResponseModel>>> action)
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
