using domain.interfaces;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace appication.commands.create
{
    public class CreateUserHandler : IRequestHandler<CreateUserRequest, CreateUserResponse>
    {
        private IUserWriteRepository Repository { get; }

        private ILogger<CreateUserHandler> Logger { get; }

        private IConfiguration Config { get; } 

        public CreateUserHandler(IUserWriteRepository repository, ILogger<CreateUserHandler> logger, IConfiguration config)
        {
            Logger = logger;
            Repository = repository;
            Config = config;
        }

        public async Task<CreateUserResponse> Handle(CreateUserRequest request, CancellationToken cancellationToken)
        {
            Logger.LogInformation($"[{GetType().Name}] Creating user {request.Username}");

            using var connection = new SqlConnection(Config["DB_CONNECTION_STRING"]
                    ?? Config.GetSection("DBConfig")["ConnectionString"]);
            connection.Open();
            using var transaction = connection.BeginTransaction();
        
            try
            {
                var now = DateTime.Now;
                long userId = 0;

                var personId = await Repository.InsertPersonalInfoAsync(request.PersonalInfo, now, connection, transaction, cancellationToken);

                if (personId > 0)
                {
                    userId = await Repository.InsertUserAsync(request, personId, now, connection, transaction, cancellationToken);
                    if (request.PersonalInfo?.Contacts.Any() == true)
                    {
                        await Repository.InsertContactsAsync(request.PersonalInfo.Contacts, personId, now, connection, transaction, cancellationToken);
                    }

                    if (request.PersonalInfo?.Addresses.Any() == true)
                    {
                        await Repository.InsertAddressesAsync(request.PersonalInfo.Addresses, personId, now, connection, transaction, cancellationToken);
                    }
                }

                transaction.Commit();
                return new CreateUserResponse { Id = userId };
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"[{GetType().Name}] Error creating user: {ex.Message}");
                transaction.Rollback();
                throw;
            }
        }
    }
}