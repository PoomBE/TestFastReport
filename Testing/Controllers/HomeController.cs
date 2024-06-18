using Microsoft.AspNetCore.Mvc;

namespace HomeController.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly IHostEnvironment _env;

        public HomeController(IHostEnvironment env)
        {
            _env = env ?? throw new ArgumentNullException(nameof(env));
        }

        
    }
}
