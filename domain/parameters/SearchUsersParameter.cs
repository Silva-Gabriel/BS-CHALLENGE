using domain.enums;

namespace domain.parameters
{
    public class SearchUsersParameter
    {
        public string Username { get; set; }
        public ClientStatus? Status { get; set; }
    }
}