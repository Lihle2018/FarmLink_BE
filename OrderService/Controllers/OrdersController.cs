using FarmLink.Shared.RequestModels;
using FarmLink.Shared.ResponseModel;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> AddOrder(OrderRequestModel Request)
        {
            var logName = MethodBase.GetCurrentMethod()?.Name;
            _logger.LogInformation("[BEGIN] " + logName);
            var result = new OrderResponseModel(await _repository.AddOrderAsync(Request));
            _logger.LogInformation("[END] " + logName);
            return new JsonResult(result);
        }
        [HttpPut("UpdateOrder")]
        public async Task<IActionResult> UpdateOrder(OrderRequestModel Request)
        {
            var logName = MethodBase.GetCurrentMethod()?.Name;
            _logger.LogInformation("[BEGIN] " + logName);
            var result = new OrderResponseModel(await _repository.UpdateOrderAsync(Request));
            _logger.LogInformation("[END] " + logName);
            return new JsonResult(result);
        }
        [HttpDelete("DeleteOrder")]
        public async Task<IActionResult> DeleteOrder(string Id)
        {
            var logName = MethodBase.GetCurrentMethod()?.Name;
            _logger.LogInformation("[BEGIN] " + logName);
            var result = await _repository.DeleteOrderAsync(Id);
            if (result == 1)
                return new JsonResult("Successfully deleted an order");
            if (result == 0)
                return new JsonResult("Order not deleted");
            else
                return new JsonResult($"More than one order is deleted ORDERID: {Id}, number of rows deleted {result}");
        }
        [HttpDelete("SoftDeleteOrder")]
        public async Task<IActionResult> SoftDeleteOrder(string Id)
        {
            var logName = MethodBase.GetCurrentMethod()?.Name;
            _logger.LogInformation("[BEGIN] " + logName);
            var result = await _repository.SoftDeleteOrderAsync(Id);
            if (result == 1)
                return new JsonResult("Successfully deleted an order");
            if (result == 0)
                return new JsonResult("Order not deleted");
            else
                return new JsonResult($"More than one order is deleted ORDERID: {Id}, number of rows deleted {result}");
        }
    }
}
