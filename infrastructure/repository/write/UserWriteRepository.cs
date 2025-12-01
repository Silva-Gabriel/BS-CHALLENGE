using System.Data;
using System.Transactions;
using Dapper;
using domain.interfaces;
using domain.models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace infra.repository
{
    public class UserWriteRepository : IUserWriteRepository
    {
        private ILogger<UserWriteRepository> Logger { get; }
        private IConfiguration Configuration { get; }

        public UserWriteRepository(ILogger<UserWriteRepository> logger, IConfiguration configuration)
        {
            Logger = logger;
            Configuration = configuration;
        }

        public async Task<long> InsertPersonalInfoAsync(PersonalInfo personalInfo, DateTime createdAt, IDbConnection connection,IDbTransaction transaction, CancellationToken cancellationToken)
        {
            var parameters = new DynamicParameters();
            
            parameters.Add("fullName", personalInfo.FullName, DbType.String);
            parameters.Add("documentNumber", personalInfo.DocumentNumber, DbType.String);
            parameters.Add("birthDate", personalInfo.BirthDate, DbType.DateTime);
            parameters.Add("genderId", personalInfo.GenderId, DbType.Int32);
            parameters.Add("createdAt", createdAt, DbType.DateTime);

            const string sql = @"
                INSERT INTO TB_PERSON (FULL_NAME, DOCUMENT_NUMBER, BIRTH_DATE, GENDER_ID, CREATED_AT)
                OUTPUT INSERTED.ID, INSERTED.FULL_NAME, INSERTED.DOCUMENT_NUMBER, INSERTED.BIRTH_DATE, INSERTED.GENDER_ID, INSERTED.CREATED_AT
                VALUES (@fullName, @documentNumber, @birthDate, @genderId, @createdAt);
            ";

            try
            {
                long personId = await connection.QuerySingleAsync<long>(sql, parameters, transaction);

                if (personId <= 0)
                {
                    Logger.LogError($"[{GetType().Name}] Error inserting personal info for documentNumber: {personalInfo.DocumentNumber}");
                    return -1;
                }   

                return personId;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"[{GetType().Name}] Error inserting personal info for documentNumber: {personalInfo.DocumentNumber}: {ex.Message}");
                throw;
            }
        }

        public async Task<long> InsertUserAsync(User request, long personId, DateTime createdAt, IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken)
        {
            var parameters = new DynamicParameters();

            var salt = int.Parse(Configuration["bcrypt:salt"]);
            request.Password = BCrypt.Net.BCrypt.HashPassword(request.Password, salt);

            parameters.Add("personId", personId, DbType.Int64);
            parameters.Add("username", request.Username, DbType.String);
            parameters.Add("password_hash", request.Password, DbType.String);
            parameters.Add("role", (int)request.Role, DbType.String);
            parameters.Add("created_at", createdAt, DbType.DateTime);
            parameters.Add("active", true, DbType.Boolean);

            var sql = @"
                INSERT INTO TB_USERS (PERSON_ID, USERNAME, PASSWORD_HASH, ACTIVE, CREATED_AT, ACCESS_GROUP)
                OUTPUT INSERTED.id
                VALUES (@personId, @username, @password_hash, @active, @created_at, @role);
            ";

            try
            {
                long userId = await connection.QuerySingleAsync<long>(sql, parameters, transaction);

                if (userId <= 0)
                {
                    Logger.LogError($"[{GetType().Name}] Error inserting user for personId: {personId}");
                    return -1;
                }   

                return userId;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"[{GetType().Name}] Error creating user: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> InsertAddressesAsync(IEnumerable<Address> addresses, long personId, DateTime createdAt, IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken)
        {
            var sucessResponse = false;

            const string sql = @"
                INSERT INTO TB_PERSON_ADDRESS (PERSON_ID, STREET, CITY, STATE, ZIP_CODE, COUNTRY, CREATED_AT)
                OUTPUT INSERTED.ID, INSERTED.PERSON_ID, INSERTED.STREET, INSERTED.CITY, INSERTED.STATE, INSERTED.ZIP_CODE, INSERTED.COUNTRY, INSERTED.CREATED_AT
                VALUES (@personId, @street, @city, @state, @zipCode, @country, @createdAt);
            ";

            foreach (var address in addresses)
            {
                var parameters = new DynamicParameters();

                parameters.Add("personId", personId, DbType.Int32);
                parameters.Add("street", address.Street, DbType.String);
                parameters.Add("number", address.Number, DbType.String);
                parameters.Add("complement", address.Complement, DbType.String);
                parameters.Add("neighborhood", address.Neighborhood, DbType.String);
                parameters.Add("city", address.City, DbType.String);
                parameters.Add("state", address.State, DbType.String);
                parameters.Add("zipCode", address.ZipCode, DbType.String);
                parameters.Add("country", address.Country, DbType.String);
                parameters.Add("isPrimary", address.IsPrimary, DbType.Boolean);
                parameters.Add("createdAt", createdAt, DbType.DateTime);

                sucessResponse = await connection.ExecuteAsync(sql, parameters, transaction) > 0;

                if (!sucessResponse)
                {
                    Logger.LogError($"[{GetType().Name}] Error inserting address for personId: {personId}");
                    return false;
                }
            }

            return sucessResponse;
        }
        
        public async Task<bool> InsertContactsAsync(IEnumerable<Contact> contacts, long personId, DateTime createdAt, IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken)
        {
            var sucessResponse = false;

            const string sql = @"
                INSERT INTO TB_PERSON_CONTACT (PERSON_ID, TYPE, VALUE, CREATED_AT)
                OUTPUT INSERTED.ID, INSERTED.PERSON_ID, INSERTED.TYPE, INSERTED.VALUE, INSERTED.CREATED_AT
                VALUES (@personId, @type, @value, @createdAt);
            ";

            foreach (var contact in contacts)
            {
                var parameters = new DynamicParameters();
                parameters.Add("personId", personId, DbType.Int32);
                parameters.Add("type", contact.Type, DbType.String);
                parameters.Add("value", contact.Value, DbType.String);
                parameters.Add("createdAt", createdAt, DbType.DateTime);

                sucessResponse = await connection.ExecuteAsync(sql, parameters, transaction) > 0;

                if (!sucessResponse)
                {
                    Logger.LogError($"[{GetType().Name}] Error inserting contact for personId: {personId}");
                    return false;
                }
            }

            return sucessResponse;
        }
    }
}