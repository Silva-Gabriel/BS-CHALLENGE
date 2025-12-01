using domain.enums;
using domain.models;
using MediatR;

namespace appication.commands.create
{
    public class CreateUserRequest : User, IRequest<CreateUserResponse>
    {
    }
}