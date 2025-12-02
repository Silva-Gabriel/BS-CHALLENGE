using System.Data;
using System.Transactions;
using domain.models;

namespace domain.interfaces
{
    public interface IUserWriteRepository
    {
        Task<long> InsertPersonalInfoAsync(PersonalInfo personalInfo, DateTime createdAt, IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken);
        Task<long> InsertUserAsync(User request, long personId, DateTime createdAt, IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken);
        Task<bool> InsertAddressesAsync(IEnumerable<Address> addresses, long personId, DateTime createdAt, IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken);
        Task<bool> InsertContactsAsync(IEnumerable<Contact> contacts, long personId, DateTime createdAt, IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken);
        Task<bool> DeletePersonAsync(long personId, CancellationToken cancellationToken);
    }
}