using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TImesheet_TEST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TestController : ControllerBase
    {
        [HttpGet("user")]
        [Authorize(Roles = "User,Admin")]
        public IActionResult UserEndpoint()
        {
            return Ok(new { message = "Hello World! You are a user!" });
        }

        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminEndpoint()
        {
            return Ok(new { message = "Hello World! You are an admin!" });
        }

        [HttpGet("public")]
        [AllowAnonymous]
        public IActionResult PublicEndpoint()
        {
            return Ok(new { message = "Hello World!!" });
        }
    }
}