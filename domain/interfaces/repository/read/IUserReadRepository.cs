using domain.dtos;
using domain.parameters;

namespace domain.interfaces.repository.read
{
    public interface IUserReadRepository
    {
        Task<IEnumerable<UserDTO>> SearchUsersAsync(SearchUsersParameter param);
    }
}