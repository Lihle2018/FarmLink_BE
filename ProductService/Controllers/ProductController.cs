using Microsoft.AspNetCore.Mvc;
using ProductService.Models;
using ProductService.Models.RequestModels;
using ProductService.Models.ResponseModels;
using ProductService.Repositories.Interfaces;
using System.Net;
using System.Reflection;

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly ILogger<ProductController> _logger;
        public ProductController(ILogger<ProductController> logger, IProductRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("AddProduct")]
        [ProducesResponseType(typeof(ProductResponseModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ProductResponseModel>> CreateProduct(ProductRequestModel Request)
        {
            var result = await ExecuteWithLogging(() => _repository.CreateProductAsync(Request));
            return Ok(result);
        }

        [HttpPut("UpdateProduct")]
        [ProducesResponseType(typeof(ProductResponseModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ProductResponseModel>> UpdateProduct(ProductRequestModel Request)
        {
            var result = await ExecuteWithLogging(() => _repository.UpdateProductAsync(Request));
            return Ok(result);
        }

        [HttpDelete("DeleteProduct")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> DeleteProduct(string Id)
        {
            return await ExecuteActionAsync(() => _repository.DeleteProductAsync(Id));
        }

        [HttpGet("GetProduct")]
        [ProducesResponseType(typeof(ProductResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ProductResponseModel>> GetProduct(string Id)
        {
            var result = await ExecuteWithLogging(() => _repository.GetProductByIdAsync(Id));
            if (result.Data == null)
            {
                _logger.LogError($"Product with Id: {Id} is not found");
                return NotFound();
            }
            {
                return Ok(result);
            }
        }

        [HttpGet("GetProducts")]
        [ProducesResponseType(typeof(IEnumerable<ProductResponseModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<IEnumerable<ProductResponseModel>>> GetProducts()
        {
            var result = await ExecuteWithLogging(() => _repository.GetProductsAsync());
            if (result == null)
            {
                _logger.LogError("Products not found");
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("GetProductsByCategory")]
        [ProducesResponseType(typeof(IEnumerable<ProductResponseModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<IEnumerable<ProductResponseModel>>> GetProductsByCategory(string CategoryId)
        {
            var result = await ExecuteWithLogging(() => _repository.GetProductsByCategoryAsync(CategoryId));
            if (result == null)
            {
                _logger.LogError($"No products under the category Id: {CategoryId}  found");
                return NotFound();
            }
            return Ok(result);
        }
        [HttpGet("SearchProducts")]
        [ProducesResponseType(typeof(IEnumerable<ProductResponseModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<IEnumerable<ProductResponseModel>>> SearchProducts(string searchString)
        {
            var result = await ExecuteWithLogging(() => _repository.SearchProductsAsync(searchString));
            if (result == null)
            {
                _logger.LogError($"The search string {searchString} did not match any product");
                return NotFound();
            }
            return Ok(result);
        }
        #region Helpers
        private async Task<ProductResponseModel> ExecuteWithLogging(Func<Task<Product>> action)
        {
            var logName = MethodBase.GetCurrentMethod()?.Name;
            _logger.LogInformation("[BEGIN] " + logName);
            var result = await action.Invoke();
            _logger.LogInformation("[END] " + logName);
            return new ProductResponseModel(result);
        }

        private async Task<IEnumerable<ProductResponseModel>> ExecuteWithLogging(Func<Task<IEnumerable<Product>>> action)
        {
            var logName = MethodBase.GetCurrentMethod()?.Name;
            _logger.LogInformation("[BEGIN] " + logName);
            var result = await action.Invoke();
            _logger.LogInformation("[END] " + logName);
            return result.Select(x => new ProductResponseModel(x));
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
