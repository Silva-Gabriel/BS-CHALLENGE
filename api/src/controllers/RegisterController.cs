using app.createRegister;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace api.src.controllers
{
    [ApiController]
    [Route("v1/api/[controller]")]
    [Produces("application/json")]
    public class RegisterController : ControllerBase
    {
        private IMediator Mediator { get; }

        public RegisterController(IMediator mediator)
        {
            Mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await Mediator.Send(request);

            return Created($"v1/api/register/{result.Id}", result);
        }
    }
}