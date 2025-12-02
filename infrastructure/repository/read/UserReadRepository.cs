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
                    u.USERNAME AS {nameof(UserDTO.Username)},
                    p.ACTIVE AS {nameof(UserDTO.Status)}
                FROM TB_USERS u
                INNER JOIN TB_PERSON p ON p.ID = u.PERSON_ID");

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

            if (param.Status != null)
            {
                if (query.ToString().Contains("WHERE"))
                {
                    query.Append(" AND ");
                }
                else
                {
                    query.Append(" WHERE ");
                }

                query.Append(" ACTIVE = @status ");
                parameters.Add("status", param.Status == domain.enums.ClientStatus.ACTIVE ? 1 : 0, DbType.Int32);
            }
        }

        public Task<bool> ExistsByIdAsync(long personId, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT COUNT(1)
                FROM TB_PERSON
                WHERE ID = @personId AND ACTIVE = 1;
            ";

            var parameters = new DynamicParameters();
            parameters.Add("personId", personId, DbType.Int64);

            return Connection.ExecuteScalarAsync<bool>(sql, parameters);
        }
    }
}