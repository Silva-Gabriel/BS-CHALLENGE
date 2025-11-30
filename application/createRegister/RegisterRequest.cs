using domain.enums;
using MediatR;

namespace app.createRegister
{
    public class RegisterRequest : IRequest<RegisterResponse>
    {
        public string User { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public Role Role { get; set; } = Role.USER;
    }
}