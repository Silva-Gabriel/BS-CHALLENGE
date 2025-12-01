using System.Data;
using System.Text;
using Dapper;
using domain.dtos;
using domain.interfaces.repository.read;
using domain.parameters;
using Microsoft.Extensions.Logging;

namespace infrastructure.repository.read
{
    public class UserReadRepository : IUserReadRepository
    {
        private IDbConnection Connection { get; }
        private ILogger<UserReadRepository> Logger { get; }

        public UserReadRepository(IDbConnection connection, ILogger<UserReadRepository> logger)
        {
            Connection = connection;
            Logger = logger;
        }

        public async Task<IEnumerable<UserDTO>> SearchUsersAsync(SearchUsersParameter param)
        {
            var parameters = new DynamicParameters();
            var query = new StringBuilder();

            query.Append(@$"
                SELECT 
                    USERNAME {nameof(UserDTO.Username)}
                FROM TB_USERS");

            BuildParamsQuery(param, query, parameters);

            try
            {
                var result = await Connection.QueryAsync<UserDTO>(query.ToString(), parameters);
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"[{GetType().Name}] Error searching users: {ex.Message}");
                throw;
            }
        }

        private static void BuildParamsQuery(SearchUsersParameter param, StringBuilder query, DynamicParameters parameters)
        {
            if (!string.IsNullOrEmpty(param.Username))
            {
                query.Append(" WHERE USERNAME LIKE @username ");
                parameters.Add("username", $"%{param.Username}%", DbType.String);
            }
        }
    }
}