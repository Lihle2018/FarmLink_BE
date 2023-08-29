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
        public async Task<ActionResult<InventoryItemResponseModel>> AddItem(InventoryItemRequestModel Request)
        {
            var result =await ExecuteWithLogging(()=>_repository.AddInventoryItemAsync(Request));
            return Ok(result);
        }

        [HttpPut("UpdateItem")]
        [ProducesResponseType(typeof(InventoryItemResponseModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<InventoryItemResponseModel>> UpdateItem(InventoryItemRequestModel Request)
        {
            var result = await ExecuteWithLogging(() => _repository.UpdateInventoryItemAsync(Request));
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
        [ProducesResponseType(typeof(InventoryItemResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<InventoryItemResponseModel>> GetItem(string Id)
        {
            var result =await ExecuteWithLogging(()=>_repository.GetInventoryItemByIdAsync(Id));
            if(result == null)
            {
                _logger.LogError($"No item found with Id: {Id}");
                return NotFound();
            }
            else
            {
                return Ok(result);
            }
        }

        [HttpGet("GetItems")]
        [ProducesResponseType(typeof(IEnumerable<InventoryItemResponseModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<IEnumerable<InventoryItemResponseModel>>> GetItems()
        {
            var result = await ExecuteWithLogging(() => _repository.GetInventoryItemsAsync());
            if (result == null)
            {
                _logger.LogError("No items found");
                return NotFound();
            }
            else
            {
                return Ok(result);
            }
        }

        #region Helpers
        private async Task<InventoryItemResponseModel> ExecuteWithLogging(Func<Task<InventoryItem>> action)
        {
            var logName = MethodBase.GetCurrentMethod()?.Name;
            _logger.LogInformation("[BEGIN] " + logName);
            var result = await action.Invoke();
            _logger.LogInformation("[END] " + logName);
            return new InventoryItemResponseModel(result);
        }

        private async Task<IEnumerable<InventoryItemResponseModel>> ExecuteWithLogging(Func<Task<IEnumerable<InventoryItem>>> action)
        {
            var logName = MethodBase.GetCurrentMethod()?.Name;
            _logger.LogInformation("[BEGIN] " + logName);
            var result = await action.Invoke();
            _logger.LogInformation("[END] " + logName);
            return result.Select(x => new InventoryItemResponseModel(x));
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
