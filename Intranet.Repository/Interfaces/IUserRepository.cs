using Intranet.Repository.Entities;

namespace Intranet.Repository.Interfaces
{
    public interface IUserRepository : IRepository<UserEntity, string>
    {
        Task<UserEntity> GetByPrincipalName(string principalName, CancellationToken cancellationToken = default);

        Task<IEnumerable<UserEntity>> FindByName(string name, CancellationToken cancellationToken = default);

        Task<IEnumerable<UserEntity>> GetUsersGroup(string groupId);
    }
}
