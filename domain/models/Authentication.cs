using domain.enums;

namespace domain.models
{
    public class Authentication
    {
        public string User { get; set; }

        public string Password { get; set; }

        public Role Role { get; set; }
    }
}