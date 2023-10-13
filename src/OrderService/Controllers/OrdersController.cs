using FarmLink.OrderService.Models;
using Microsoft.AspNetCore.Mvc;
using OrderService.Repositories.Interfaces;
using System.Net;
using System.Reflection;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _repository;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IOrderRepository repository, ILogger<OrdersController> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("AddOrder")]
        [ProducesResponseType(typeof(OrderResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<OrderResponseModel>> AddOrder(OrderRequestModel Request)
        {
            var result = await ExecuteWithLogging(()=> _repository.AddOrderAsync(Request));
            if (result.Data == null && !result.Error)
                return BadRequest(result);
            if (result.Error)
                return StatusCode(500, result);
            return Ok(result);
        }

        [HttpGet("GetOrder")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(OrderResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<OrderResponseModel>> GetOrder(string Id)
        {
            var result = await ExecuteWithLogging(() => _repository.GetOrderAsync(Id));
            if (result.Data == null && !result.Error)
                return NotFound(result);
            if (result.Error)
                return StatusCode(500, result);
            return Ok(result);
        }

        [HttpGet("GetOrders")]
        [ProducesResponseType(typeof(IEnumerable<OrderResponseModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IEnumerable<OrderResponseModel>>> GetOrders()
        {
            var result = await ExecuteWithLogging(() => _repository.GetOrdersAsync());
            var first = result.FirstOrDefault();
            if (first.Data == null && !first.Error)
                return NotFound(result);
            if (first.Error)
                return StatusCode(500, result);
            return Ok(result);
        }

        [HttpPut("UpdateOrder")]
        [ProducesResponseType(typeof(OrderResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<OrderResponseModel>> UpdateOrder(OrderRequestModel Request)
        {
            var result = await ExecuteWithLogging(() => _repository.UpdateOrderAsync(Request));
            if (result.Data == null && !result.Error)
                return NotFound(result);
            if (result.Error)
                return StatusCode(500, result);
            return Ok(result);
        }

        [HttpDelete("DeleteOrder")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> DeleteOrder(string id)
        {
            return await ExecuteActionAsync(() => _repository.DeleteOrderAsync(id));
        }

        [HttpDelete("SoftDeleteOrder")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> SoftDeleteOrder(string id)
        {
            return await ExecuteActionAsync(() => _repository.SoftDeleteOrderAsync(id));
        }

        #region Helpers
        private async Task<OrderResponseModel> ExecuteWithLogging(Func<Task<OrderResponseModel>> action)
        {
            var logName = MethodBase.GetCurrentMethod()?.Name;
            _logger.LogInformation("[BEGIN] " + logName);
            var result = await action.Invoke();
            _logger.LogInformation("[END] " + logName);
            return result;
        }
        private async Task<IEnumerable<OrderResponseModel>> ExecuteWithLogging(Func<Task<IEnumerable<OrderResponseModel>>> action)
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
                return StatusCode(500,$"Multiple actions were completed: {logName}, number of actions completed: {result}");
        }
        #endregion
    }
}
