using domain.interfaces;
using domain.models;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace app.createRegister
{
    public class RegisterHandler : IRequestHandler<RegisterRequest, RegisterResponse>
    {
        private IUserWriteRepository Repository { get; }
    
        private ILogger<RegisterHandler> Logger { get; }

        private IConfiguration Configuration { get; }

        public RegisterHandler(IUserWriteRepository repository, ILogger<RegisterHandler> logger, IConfiguration configuration)
        {
            Configuration = configuration;
            Logger = logger;
            Repository = repository;
        }

        public async Task<RegisterResponse> Handle(RegisterRequest request, CancellationToken cancellationToken)
        {
            var responseAuth = new Authentication 
            {
                User = request.User,
                Password = request.Password,
                Role = request.Role
            };

            Logger.LogInformation($"[{GetType().Name}] Registering user {request.User}");

            var salt = int.Parse(Configuration["bcrypt:salt"]);
            responseAuth.Password = BCrypt.Net.BCrypt.HashPassword(request.Password, salt);
            var result = await Repository.CreateUser(responseAuth, cancellationToken);

            Logger.LogInformation($"[{GetType().Name}] User registered with id {result}");

            return new RegisterResponse { Id = result };
        }
    }
}