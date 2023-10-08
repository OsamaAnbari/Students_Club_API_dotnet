using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProtectedController : ControllerBase
    {
        [Authorize(Roles="admin")]
        [HttpGet("protectedAdmin")]
        public IActionResult GetProtectedData1()
        {
            // Return data that requires authentication
            return Ok("This is protected data");
        }

        [Authorize(Roles = "user")]
        [HttpGet("protectedUser")]
        public IActionResult GetProtectedData2()
        {
            // Return data that requires authentication
            return Ok("This is protected data");
        }
    }
}