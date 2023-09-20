using Microsoft.AspNetCore.Mvc;

namespace NorthwindF.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NorthwindController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<NorthwindController> _logger;

        public NorthwindController(ILogger<NorthwindController> logger)
        {
            _logger = logger;
        }
    }
}