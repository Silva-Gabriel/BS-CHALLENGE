using domain.dtos;

namespace application.queries.search
{
    public class SearchUsersResponse
    {
        public IEnumerable<UserDTO> Users { get; set; }
    }
}