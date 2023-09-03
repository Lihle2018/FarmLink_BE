﻿using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Reflection;
using VendorService.Models;
using VendorService.Models.RequestModels;
using VendorService.Models.ResponseModels;
using VendorService.Repositories.Interfaces;

namespace VendorService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorsController : ControllerBase
    {
        private readonly ILogger<VendorsController> _logger;
        private readonly IVendorRepository _repository;
        public VendorsController(ILogger<VendorsController> logger, IVendorRepository repository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpPost("AddVendor")]
        [ProducesResponseType(typeof(VendorResponseModel), ((int)HttpStatusCode.OK))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<VendorResponseModel>> AddVendor(VendorRequestModel Request)
        {
            var result = await ExecuteWithLogging(async () => await _repository.CreateVendorAsync(Request));
            if (result.Data == null && !result.Error)
                return Unauthorized(result);
            if (result.Error)
                return StatusCode(500, result);
            return Ok(result);
        }

        [HttpPut("UpdateVendor")]
        [ProducesResponseType(typeof(VendorResponseModel), ((int)HttpStatusCode.OK))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<VendorResponseModel>> UpdateVendor(VendorRequestModel Request)
        {
            var result = await ExecuteWithLogging(async () => await _repository.UpdateVendorAsync(Request));
            if (result.Data == null && !result.Error)
                return NotFound(result);
            if (result.Error)
                return StatusCode(500, result);
            return Ok(result);
        }

        [HttpDelete("DeleteVendor")]
        [ProducesResponseType(typeof(string), ((int)HttpStatusCode.OK))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> DeleteVendor(string Id)
        {
            return await ExecuteActionAsync(async () => await _repository.DeleteVendorAsync(Id));
        }

        [HttpGet("GetVendor")]
        [ProducesResponseType(typeof(VendorResponseModel), ((int)HttpStatusCode.OK))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<VendorResponseModel>> GetVendor(string Id)
        {
            var result = await ExecuteWithLogging(async () => await _repository.GetVendorByIdAsync(Id));
            if (result.Data == null && !result.Error)
                return NotFound(result);
            if (result.Error)
                return StatusCode(500, result);
            return Ok(result);
        }

        [HttpGet("GetVendors")]
        [ProducesResponseType(typeof(VendorResponseModel), ((int)HttpStatusCode.OK))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IEnumerable<VendorResponseModel>>> GetVendors()
        {
            var result = await ExecuteWithLogging(async () => await _repository.GetVendorsAsync());
            var first = result.FirstOrDefault();
            if (first.Data == null && !first.Error)
                return NotFound(result);
            if (first.Error)
                return StatusCode(500, result);
            return Ok(result);
        }

        [HttpGet("GetVendorsByProduct")]
        [ProducesResponseType(typeof(IEnumerable<VendorResponseModel>), ((int)HttpStatusCode.OK))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IEnumerable<VendorResponseModel>>> GetVendorsByProduct(string productId)
        {
            var result = await ExecuteWithLogging(async () => await _repository.GetVendorsByProductAsync(productId));
            var first = result.FirstOrDefault();
            if (first.Data == null && !first.Error)
                return NotFound(result);
            if (first.Error)
                return StatusCode(500, result);
            return Ok(result);
        }

        [HttpGet("GetVendorsByLocation")]
        [ProducesResponseType(typeof(IEnumerable<VendorResponseModel>), ((int)HttpStatusCode.OK))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IEnumerable<VendorResponseModel>>> GetVendorsByLocation(string locationId)
        {
            var result = await ExecuteWithLogging(async () => await _repository.GetVendorsByLocationAsync(locationId));
            var first = result.FirstOrDefault();
            if (first.Data == null && !first.Error)
                return NotFound(result);
            if (first.Error)
                return StatusCode(500, result);
            return Ok(result);
        }

        #region Helpers
        private async Task<VendorResponseModel> ExecuteWithLogging(Func<Task<Vendor>> action)
        {
            var logName = MethodBase.GetCurrentMethod()?.Name;
            try
            {
                _logger.LogInformation("[BEGIN] " + logName);
                var result = await action.Invoke();
                if (result != null)
                {
                    _logger.LogInformation("[END] " + logName);
                    return new VendorResponseModel(result);
                }
                _logger.LogInformation("[END] " + logName);
                return new VendorResponseModel(result, $"{logName} operation failed.");
            }
            catch (Exception e)
            {
                _logger.LogInformation("[END] " + logName);
                return new VendorResponseModel(null, e.Message, true);
            }
        }
        private async Task<IEnumerable<VendorResponseModel>> ExecuteWithLogging(Func<Task<IEnumerable<Vendor>>> action)
        {
            var logName = MethodBase.GetCurrentMethod()?.Name;
            try
            {
                _logger.LogInformation("[BEGIN] " + logName);
                var result = await action.Invoke();
                if (result != null)
                {
                    _logger.LogInformation("[END] " + logName);
                    return result.Select(x => new VendorResponseModel(x));
                }
                else
                {
                    _logger.LogInformation("[END] " + logName);
                    return new[] { new VendorResponseModel(null, $"{logName} operation failed.") };
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation("[END] " + logName);
                return new[] { new VendorResponseModel(null, e.Message, true) };
            }
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
