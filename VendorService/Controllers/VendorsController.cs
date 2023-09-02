using Microsoft.AspNetCore.Mvc;
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
    }
}
