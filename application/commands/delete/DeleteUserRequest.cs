using MediatR;

namespace application.commands.delete
{
    public class DeleteUserRequest : IRequest<DeleteUserResponse>
    {
        public long UserId { get; set; }
    }
}