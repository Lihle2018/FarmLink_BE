using CustomerService.Repositories.Interfaces;
using FarmLink.CustomerService.Models;
using FarmLink.CustomerService.Models.RequestModels;
using FarmLink.CustomerService.Models.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using System.Net;
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
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("AddCustomer")]
        [ProducesResponseType(typeof(CustomerResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<CustomerResponseModel>> AddCustomer(CustomerRequestModel Request)
        {
            var result = await ExecuteWithLogging(() => _repository.AddCustomerAsync(Request));
            if (result.Data == null && !result.Error)
                return Unauthorized(result);
            if (result.Error)
                return StatusCode(500, result);
            return Ok(result);
        }

        [HttpGet("GetCustomer")]
        [ProducesResponseType(typeof(CustomerResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<CustomerResponseModel>> GetCustomer(string Id)
        {
            var result = await ExecuteWithLogging(() => _repository.GetCustomerAsync(Id));
            if (result.Data == null&&!result.Error)
            {
                return NotFound(result);
            }
            if(result.Error)
                return StatusCode(500, result);
            else
            {
                return Ok(result);
            }
        }

        [HttpGet("GetCustomers")]
        [ProducesResponseType(typeof(IEnumerable<CustomerResponseModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IEnumerable<CustomerResponseModel>>> GetCustomers()
        {
            var result = await ExecuteWithLogging(() => _repository.GetCustomersAsync());
            var first = result.FirstOrDefault();
            if (first.Data == null&&!first.Error)
                return NotFound(result);
            if (first.Error)
                return StatusCode(500, result);
            return Ok(result);
        }

        [HttpPut("UpdateCustomer")]
        [ProducesResponseType(typeof(CustomerResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<CustomerResponseModel>> UpdateCustomer(CustomerRequestModel Request)
        {
            var result = await ExecuteWithLogging(() => _repository.UpdateCustomerAsync(Request));
            if(result.Data==null&&!result.Error)
                return NotFound(result);
            if (result.Error)
                return StatusCode(500, result);
            return Ok(result);
        }

        [HttpDelete("SoftDeleteCustomer")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> SoftDeleteCustomer(string Id)
        {
            return await ExecuteActionAsync(() => _repository.SoftDeleteCustomerAsync(Id));
        }

        [HttpDelete("DeleteCustomer")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> DeleteCustomer(string Id)
        {
            return await ExecuteActionAsync(() => _repository.DeleteCustomerAsync(Id));
        }

        #region Helpers
        private async Task<CustomerResponseModel> ExecuteWithLogging(Func<Task<CustomerResponseModel>> action)
        {
            var logName = MethodBase.GetCurrentMethod()?.Name;
            _logger.LogInformation("[BEGIN] " + logName);
            var result = await action.Invoke();
            _logger.LogInformation("[END] " + logName);
            return result;
        }

        private async Task<IEnumerable<CustomerResponseModel>> ExecuteWithLogging(Func<Task<IEnumerable<CustomerResponseModel>>> action)
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
