using domain.interfaces;
using domain.interfaces.repository.read;
using MediatR;

namespace application.commands.delete
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserRequest, DeleteUserResponse>
    {
        IUserReadRepository UserReadRepository { get; }
        IUserWriteRepository UserWriteRepository { get; }

        public DeleteUserHandler(IUserReadRepository userReadRepository, IUserWriteRepository userWriteRepository)
        {
            UserReadRepository = userReadRepository;
            UserWriteRepository = userWriteRepository;
        }

        public async Task<DeleteUserResponse> Handle(DeleteUserRequest request, CancellationToken cancellationToken)
        {
            var exists = await UserReadRepository.ExistsByIdAsync(request.UserId, cancellationToken);

            if (!exists)
            {
                throw new Exception($"[{GetType().Name}] User with ID {request.UserId} does not exist");
            }

            var result = await UserWriteRepository.DeletePersonAsync(request.UserId, cancellationToken);

            if (!result)
            {
                throw new Exception($"[{GetType().Name}] Error deleting user with ID {request.UserId}");
            }

            return new DeleteUserResponse();
        }
    }
}