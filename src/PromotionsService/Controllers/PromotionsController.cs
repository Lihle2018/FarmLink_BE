using Microsoft.AspNetCore.Mvc;
using PromotionsService.Models.RequestModels;
using PromotionsService.Models.ResponseModels;
using PromotionsService.Repositories.Interfaces;
using System.Net;
using System.Reflection;

namespace PromotionsService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionsController : ControllerBase
    {
        private readonly ILogger<PromotionsController> _logger;
        private readonly IPromotionsRepository _repository;
        public PromotionsController(ILogger<PromotionsController> logger, IPromotionsRepository repository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpPost("AddPromotion")]
        [ProducesResponseType(typeof(PromotionResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<PromotionResponseModel>> AddPromotion(PromotionRequestModel Request)
        {
            var result = await ExecuteWithLogging(async () => await _repository.CreatePromotionAsync(Request));
            if (result.Data == null && !result.Error)
                return BadRequest(result);
            if (result.Error)
                return StatusCode(500, result);
            return Ok(result);
        }

        [HttpPut("UpdatePromotion")]
        [ProducesResponseType(typeof(PromotionResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<PromotionResponseModel>> UpdatePromotion(PromotionRequestModel Request)
        {
            var result = await ExecuteWithLogging(async () => await _repository.UpdatePromotionAsync(Request));
            if (result.Data == null && !result.Error)
                return BadRequest(result);
            if (result.Error)
                return StatusCode(500, result);
            return Ok(result);
        }

        [HttpDelete("DeletePromotion")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> DeletePromotion(string id)
        {
            return await ExecuteActionAsync(async () => await _repository.DeletePromotionAsync(id));
        }

        [HttpGet("GetPromotion")]
        [ProducesResponseType(typeof(PromotionResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<PromotionResponseModel>> GetPromotion(string Id)
        {
            var result = await ExecuteWithLogging(async () => await _repository.GetPromotionByIdAsync(Id));
            if (result.Data == null && !result.Error)
                return NotFound(result);
            if (result.Error)
                return StatusCode(500, result);
            return Ok(result);
        }

        [HttpGet("GetPromotions")]
        [ProducesResponseType(typeof(IEnumerable<PromotionResponseModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IEnumerable<PromotionResponseModel>>> GetPromotions()
        {
            var result = await ExecuteWithLogging(async () => await _repository.GetPromotionsAsync());
            var first = result.FirstOrDefault();
            if (first.Data == null && !first.Error)
                return NotFound(result);
            if (first.Error)
                return StatusCode(500, result);
            return Ok(result);
        }

        [HttpGet("GetPromotionsByTargetAudience")]
        [ProducesResponseType(typeof(IEnumerable<PromotionResponseModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IEnumerable<PromotionResponseModel>>> GetPromotionsByTargetAudience(PromotionsByTargetAudienceRequestModel Request)
        {
            var result = await ExecuteWithLogging(async () => await _repository.GetPromotionsByTargetAudienceAsync(Request.TargetAudience));
            var first = result.FirstOrDefault();
            if (first.Data == null && !first.Error)
                return NotFound(result);
            if (first.Error)
                return StatusCode(500, result);
            return Ok(result);
        }

        [HttpGet("GetPromotionsByType")]
        [ProducesResponseType(typeof(IEnumerable<PromotionResponseModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IEnumerable<PromotionResponseModel>>> GetPromotionsByType(PromotionsByTypeRequestModel Request)
        {
            var result = await ExecuteWithLogging(async () => await _repository.GetPromotionsByTypeAsync(Request.PromotionType));
            var first = result.FirstOrDefault();
            if (first.Data == null && !first.Error)
                return NotFound(result);
            if (first.Error)
                return StatusCode(500, result);
            return Ok(result);
        }

        #region Helpers
        private async Task<PromotionResponseModel> ExecuteWithLogging(Func<Task<PromotionResponseModel>> action)
        {
            var logName = MethodBase.GetCurrentMethod()?.Name;
            _logger.LogInformation("[BEGIN] " + logName);
            var result = await action.Invoke();
            _logger.LogInformation("[END] " + logName);
            return result;
        }
        private async Task<IEnumerable<PromotionResponseModel>> ExecuteWithLogging(Func<Task<IEnumerable<PromotionResponseModel>>> action)
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
