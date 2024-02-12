using Intranet.Repository.Entities;

namespace Intranet.Repository.Interfaces
{
    public interface IGroupRepository : IRepository<GroupEntity, string>
    {
        Task<IEnumerable<GroupEntity>> FindByName(string name, CancellationToken cancellationToken = default);

        Task<IEnumerable<GroupEntity>> GetGroupsUser(string userId, CancellationToken cancellationToken = default);
    }
}
