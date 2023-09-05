using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductService.Models.ResponseModels;
using ProductService.Models;
using ProductService.Repositories.Interfaces;
using System.Reflection;
using ProductService.Models.RequestModels;
using System.Net;

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _repository;
        private readonly ILogger<CategoryController> _logger;
        public CategoryController(ILogger<CategoryController> logger, ICategoryRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("AddCategory")]
        [ProducesResponseType(typeof(CategoryResponseModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<CategoryResponseModel>> AddCategory(CategoryRequestModel request)
        {
            var result = await ExecuteWithLogging(()=>_repository.CreateCategoryAsync(request));
            return Ok(result);
        }

        [HttpPut("UpdateCategory")]
        [ProducesResponseType(typeof(CategoryResponseModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<CategoryResponseModel>> UpdateCategory(CategoryRequestModel Request)
        {
            var result =await ExecuteWithLogging(()=>_repository.UpdateCategoryAsync(Request));
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
        public async Task<ActionResult<CategoryResponseModel>> GetCategory(string Id)
        {
            var result =await ExecuteWithLogging(()=>_repository.GetCategoryAsync(Id));
            if(result.Data == null)
            {
                _logger.LogError("Category not found");
                return NotFound();
            }
            {
                return Ok(result);
            }
        }

        [HttpGet("GetCategories")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<CategoryResponseModel>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<CategoryResponseModel>>> GetCategories()
        {
            var result = await ExecuteWithLogging(() => _repository.GetCategoriesAsync());
            if (result== null)
            {
                _logger.LogError("Categories not found");
                return NotFound();
            }
            {
                return Ok(result);
            }
        }
        #region Helpers
        private async Task<CategoryResponseModel> ExecuteWithLogging(Func<Task<Category>> action)
        {
            var logName = MethodBase.GetCurrentMethod()?.Name;
            _logger.LogInformation("[BEGIN] " + logName);
            var result = await action.Invoke();
            _logger.LogInformation("[END] " + logName);
            return new CategoryResponseModel(result);
        }

        private async Task<IEnumerable<CategoryResponseModel>> ExecuteWithLogging(Func<Task<IEnumerable<Category>>> action)
        {
            var logName = MethodBase.GetCurrentMethod()?.Name;
            _logger.LogInformation("[BEGIN] " + logName);
            var result = await action.Invoke();
            _logger.LogInformation("[END] " + logName);
            return result.Select(x => new CategoryResponseModel(x));
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
