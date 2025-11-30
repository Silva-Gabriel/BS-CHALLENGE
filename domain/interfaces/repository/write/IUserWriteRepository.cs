using domain.models;

namespace domain.interfaces
{
    public interface IUserWriteRepository
    {
        Task<long> CreateUser(Authentication auth, CancellationToken cancellationToken);
    }
}