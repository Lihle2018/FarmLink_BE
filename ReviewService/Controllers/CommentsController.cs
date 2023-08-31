using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReviewService.Repositories.Interfaces;

namespace ReviewService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ILogger<CommentsController> _logger;
        private readonly ICommentsRepository _repository;
        public CommentsController(ILogger<CommentsController> logger, ICommentsRepository repository)
        {
            _repository = repository?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(_logger));
        }
    }
}
