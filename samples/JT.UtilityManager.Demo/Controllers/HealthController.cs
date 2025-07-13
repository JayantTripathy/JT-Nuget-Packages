using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JT.UtilityManager.Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok(new { status = "Healthy" });
    }
}
