using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReviewService.Repositories.Interfaces;

namespace ReviewService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingsController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IRatingsRepository _repository;
        public RatingsController(ILogger logger, IRatingsRepository repository)
        {
            _repository= repository??throw new ArgumentNullException(nameof(repository));
            _logger = logger?? throw new ArgumentNullException(nameof(logger));
        }
    }
}
