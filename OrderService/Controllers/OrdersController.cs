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
            _repository = repository;
            _logger = logger;
        }
        [HttpPost("AddOrder")]
        [ProducesResponseType(typeof(OrderResponseModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<OrderResponseModel>> AddOrder(OrderRequestModel Request)
        {
            var result = await ExecuteWithLogging(()=> _repository.AddOrderAsync(Request));
            return Ok(result);
        }

        [HttpGet("GetOrder")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(OrderResponseModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<OrderResponseModel>> GetOrder(string Id)
        {
            var result = await ExecuteWithLogging(() => _repository.GetOrderByAsync(Id));
            if(result == null)
            {
                _logger.LogError($"Order with ID: {Id} is not Found");
                return NotFound();
            }
            else { return Ok(result); }
        }

        [HttpGet("GetOrders")]
        [ProducesResponseType(typeof(IEnumerable<OrderResponseModel>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<OrderResponseModel>>> GetOrders()
        {
            var result = await _repository.GetOrdersAsync();
            return Ok(result);
        }

        [HttpPut("UpdateOrder")]
        [ProducesResponseType(typeof(OrderResponseModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<OrderResponseModel>> UpdateOrder(OrderRequestModel Request)
        {
            var result = await ExecuteWithLogging(() => _repository.UpdateOrderAsync(Request));
            return Ok(result);
        }
        [HttpDelete("DeleteOrder")]
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

        private async Task<OrderResponseModel> ExecuteWithLogging(Func<Task<Order>> action)
        {
            var logName = MethodBase.GetCurrentMethod()?.Name;
            _logger.LogInformation("[BEGIN] " + logName);
            var result = await action.Invoke();
            _logger.LogInformation("[END] " + logName);
            return new OrderResponseModel(result);
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
    }
}
