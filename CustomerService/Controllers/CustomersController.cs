using FarmLink.Shared.RequestModels;
using FarmLink.Shared.ResponseModel;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace CustomerService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository _repository;
        private readonly ILogger<CustomersController> _logger;
      
        public CustomersController(ICustomerRepository repository, ILogger<CustomersController> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        [HttpPost("AddCustomer")]
        public async Task<IActionResult> AddCustomer(CustomerRequestModel Request)
        {
            var logName = MethodBase.GetCurrentMethod()?.Name;
            _logger.LogInformation("[BEGIN] " + logName);
            var result =new CustomerResponseModel(await _repository.AddCustomerAsync(Request));
            _logger.LogInformation("[END] " + logName);
            return new JsonResult(result);
        }
        [HttpPut("UpdateCustomer")]
        public async Task<IActionResult> UpdateCustomer(CustomerRequestModel Request)
        {
            var logName = MethodBase.GetCurrentMethod()?.Name;
            _logger.LogInformation("[BEGIN] " + logName);
            var result = new CustomerResponseModel(await _repository.UpdateCustomerAsync(Request));
            _logger.LogInformation("[END] " + logName);
            return new JsonResult(result);
        }
        [HttpPut("SoftDeleteCustomer")]
        public async Task<IActionResult> SoftDeleteCustomer(string Id)
        {
            var logName = MethodBase.GetCurrentMethod()?.Name;
            _logger.LogInformation("[BEGIN] " + logName);
            var result = await _repository.SoftDeleteCustomerAsync(Id);
            if (result == 1)
                return new JsonResult("Successfully deleted a customer");
            if (result == 0)
                return new JsonResult("Customer not deleted");
            else
                return new JsonResult($"More than one customer is deleted CUSTOMERID: {Id}, number of rows deleted {result}");
        }
        [HttpDelete("DeleteCustomer")]
        public async Task<IActionResult> DeleteCustomer(string Id)
        {
            var logName = MethodBase.GetCurrentMethod()?.Name;
            _logger.LogInformation("[BEGIN] " + logName);
            var result = await _repository.DeleteCustomerAsync(Id);
            if (result == 1)
                return new JsonResult("Successfully deleted a customer");
            if (result == 0)
                return new JsonResult("Customer not deleted");
            else
                return new JsonResult($"More than one customer is deleted CUSTOMERID: {Id}, number of rows deleted {result}");
        }
    }
}
