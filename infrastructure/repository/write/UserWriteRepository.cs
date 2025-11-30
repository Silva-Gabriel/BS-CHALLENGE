using System.Data;
using Dapper;
using domain.interfaces;
using domain.models;
using Microsoft.Extensions.Logging;

namespace infra.repository
{
    public class UserWriteRepository : IUserWriteRepository
    {
        private IDbConnection Connection { get; }
        private ILogger<UserWriteRepository> Logger { get; }

        public UserWriteRepository(IDbConnection connection, ILogger<UserWriteRepository> logger)
        {
            Connection = connection;
            Logger = logger;
        }

        public async Task<long> CreateUser(Authentication auth, CancellationToken cancellationToken)
        {
            var parameters = new DynamicParameters();
            parameters.Add("username", auth.User, DbType.String);
            parameters.Add("password_hash", auth.Password, DbType.String);
            parameters.Add("role", (int)auth.Role, DbType.String);
            parameters.Add("created_at", DateTime.Now, DbType.DateTime);
            parameters.Add("active", true, DbType.Boolean);

            var query = @"
                INSERT INTO TB_USERS (USERNAME, PASSWORD_HASH, ACTIVE, CREATED_AT, ACCESS_GROUP)
                OUTPUT INSERTED.id
                VALUES (@username, @password_hash, @active, @created_at, @role);
            ";

            try
            {
                var result = await Connection.QuerySingleAsync<long>(query, parameters);
                return result;
            }
            catch (Exception ex)
            {
                    Logger.LogError(ex, $"[{GetType().Name}] Error creating user: {ex.Message}");
                throw;
            }
        }
    }
}