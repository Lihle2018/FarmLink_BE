using Amazon.Runtime.Internal.Transform;
using InventoryService.Models;
using InventoryService.Models.RequestModels;
using InventoryService.Models.ResponseModels;
using InventoryService.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Reflection;

namespace InventoryService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly ILogger<InventoryController> _logger;
        private readonly IInventoryRepository _repository;
        public InventoryController(ILogger<InventoryController> logger, IInventoryRepository repository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpPost("AddItem")]
        [ProducesResponseType(typeof(InventoryItemResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<InventoryItemResponseModel>> AddItem(InventoryItemRequestModel Request)
        {
            var result =await ExecuteWithLogging(()=>_repository.AddInventoryItemAsync(Request));
            if (result.Data == null && !result.Error)
                return Unauthorized(result);
            if (result.Error)
                return StatusCode(500, result);
            return Ok(result);
        }

        [HttpPut("UpdateItem")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(InventoryItemResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]

        public async Task<ActionResult<InventoryItemResponseModel>> UpdateItem(InventoryItemRequestModel Request)
        {
            var result = await ExecuteWithLogging(() => _repository.UpdateInventoryItemAsync(Request));
            if (result.Data == null && !result.Error)
                return NotFound(result);
            if (result.Error)
                return StatusCode(500, result);
            return Ok(result);
        }

        [HttpDelete("DeleteItem")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> DeleteItem(string Id)
        {
           return await ExecuteActionAsync(() => _repository.DeleteInventoryItemAsync(Id));
        }

        [HttpGet("GetItem")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(InventoryItemResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<InventoryItemResponseModel>> GetItem(string Id)
        {
            var result =await ExecuteWithLogging(()=>_repository.GetInventoryItemByIdAsync(Id));
            if (result.Data == null && !result.Error)
                return NotFound(result);
            if (result.Error)
                return StatusCode(500, result);
            return Ok(result);
        }

        [HttpGet("GetItems")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<InventoryItemResponseModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IEnumerable<InventoryItemResponseModel>>> GetItems()
        {
            var result = await ExecuteWithLogging(() => _repository.GetInventoryItemsAsync());
            var first = result.FirstOrDefault();
            if (first.Data == null && !first.Error)
                return NotFound(result);
            if (first.Error)
                return StatusCode(500, result);
            return Ok(result);
        }

        #region Helpers
        private async Task<InventoryItemResponseModel> ExecuteWithLogging(Func<Task<InventoryItemResponseModel>> action)
        {
            var logName = MethodBase.GetCurrentMethod()?.Name;
            _logger.LogInformation("[BEGIN] " + logName);
            var result = await action.Invoke();
            _logger.LogInformation("[END] " + logName);
            return result;
        }

        private async Task<IEnumerable<InventoryItemResponseModel>> ExecuteWithLogging(Func<Task<IEnumerable<InventoryItemResponseModel>>> action)
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
