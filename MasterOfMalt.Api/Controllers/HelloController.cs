using Microsoft.AspNetCore.Mvc;

namespace MasterOfMalt.Api.Controllers
{
    [Route("api/mom/[controller]")]
    [ApiController]
    public class HelloController : ControllerBase
    {
        [HttpGet]
        public IActionResult HelloWorld()
        {
            return Ok("Hello from Master of Malt Image API.");
        }
    }
}
