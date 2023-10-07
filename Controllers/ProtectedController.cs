using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProtectedController : ControllerBase
    {
        [HttpGet("protected")]
        public IActionResult GetProtectedData()
        {
            // Return data that requires authentication
            return Ok("This is protected data");
        }
    }
}