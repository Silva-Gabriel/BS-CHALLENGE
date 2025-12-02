using System.Threading.Tasks;
using appication.commands.create;
using application.commands.delete;
using application.queries.search;
using domain.enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api.src.controllers
{
    [ApiController]
    [Route("v1/api/[controller]")]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        private IMediator Mediator { get; }

        public UserController(IMediator mediator)
        {
            Mediator = mediator;
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Criar um novo registro de usuário",
            Description = "Cria um novo registro de usuário e retorna o ID do registro.")]
        [ProducesResponseType(typeof(CreateUserResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] CreateUserRequest request)
        {
            var result = await Mediator.Send(request);

            return Created($"v1/api/user/{result.Id}", result);
        }

        [HttpGet]
        [Authorize(Roles = $"{nameof(Role.ADMIN)},{nameof(Role.MANAGER)}")]
        [SwaggerOperation(
            Summary = "Listar todos os registros de usuários",
            Description = "Retorna uma lista de todos os registros de usuários ativos e inativos.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Search([FromQuery] SearchUsersRequest request)
        {
            var result = await Mediator.Send(request);

            return Ok(result);
        }

        [HttpDelete("{userId}")]
        [Authorize(Roles = nameof(Role.MANAGER))]
        [SwaggerOperation(
            Summary = "Deletar logicamente um registro de usuário",
            Description = "Deleta logicamente um registro de usuário pelo ID da pessoa.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(long userId)
        {
            await Mediator.Send(new DeleteUserRequest { UserId = userId });

            return NoContent();
        }
    }
}