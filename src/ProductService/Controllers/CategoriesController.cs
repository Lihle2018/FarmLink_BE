using Microsoft.AspNetCore.Mvc;
using ProductService.Models.ResponseModels;
using ProductService.Repositories.Interfaces;
using System.Reflection;
using ProductService.Models.RequestModels;
using System.Net;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _repository;
        private readonly ILogger<CategoriesController> _logger;
        public CategoriesController(ILogger<CategoriesController> logger, ICategoryRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("AddCategory")]
        [ProducesResponseType(typeof(CategoryResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<CategoryResponseModel>> AddCategory(CategoryRequestModel request)
        {
            var result = await ExecuteWithLogging(()=>_repository.CreateCategoryAsync(request));
            if (result.Data == null && !result.Error)
                return BadRequest(result);
            if (result.Error)
                return StatusCode(500, result);
            return Ok(result);
        }

        [HttpPut("UpdateCategory")]
        [ProducesResponseType(typeof(CategoryResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<CategoryResponseModel>> UpdateCategory(CategoryRequestModel Request)
        {
            var result =await ExecuteWithLogging(()=>_repository.UpdateCategoryAsync(Request));
            if (result.Data == null && !result.Error)
                return NotFound(result); 
            if (result.Error) 
                return StatusCode(500, result);
            return Ok(result);
        }

        [HttpDelete("DeleteCategory")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> DeleteCategory(string Id)
        {
            return await ExecuteActionAsync(() => _repository.DeleteCategoryAsync(Id));
        }

        [HttpGet("GetCategory")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(CategoryResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<CategoryResponseModel>> GetCategory(string Id)
        {
            var result =await ExecuteWithLogging(()=>_repository.GetCategoryAsync(Id));
            if(result.Data == null&&!result.Error)
                return NotFound(result);
            if(result.Error) 
                return StatusCode(500, result);
             return Ok(result);
        }

        [HttpGet("GetCategories")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<CategoryResponseModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IEnumerable<CategoryResponseModel>>> GetCategories()
        {
            var result = await ExecuteWithLogging(() => _repository.GetCategoriesAsync());
            var first = result.FirstOrDefault();
            if (first.Data==null&&!first.Error)
                return NotFound(result);
            if(first.Error) 
                return StatusCode(500, result);
            return Ok(result);
        }
        #region Helpers
        private async Task<CategoryResponseModel> ExecuteWithLogging(Func<Task<CategoryResponseModel>> action)
        {
            var logName = MethodBase.GetCurrentMethod()?.Name;
            _logger.LogInformation("[BEGIN] " + logName);
            var result = await action.Invoke();
            _logger.LogInformation("[END] " + logName);
            return result;
        }

        private async Task<IEnumerable<CategoryResponseModel>> ExecuteWithLogging(Func<Task<IEnumerable<CategoryResponseModel>>> action)
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
