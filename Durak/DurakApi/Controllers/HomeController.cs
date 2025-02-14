using Microsoft.AspNetCore.Mvc;

namespace DurakApi.Controllers
{
    [Route("/")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet("/")]
        public IActionResult Health()
        {
            return Ok(new { status = "alive" });
        }
    }
}
