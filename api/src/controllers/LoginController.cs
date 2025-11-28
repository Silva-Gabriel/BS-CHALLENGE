using Microsoft.AspNetCore.Mvc;

namespace api.src.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        [HttpGet]
        public IActionResult Login()
        {
            var registers = new List<string>
            {
                "user1",
                "user2",
            };

            return Ok(registers);
        }
    }
}