using Intranet.Repository.ActiveDirectory.Options;
using Intranet.Repository.Entities;
using Intranet.Repository.Interfaces;
using Microsoft.Extensions.Options;

namespace Intranet.Repository.ActiveDirectory.Implementaions
{
    public class UserRepository : IUserRepository
    {
        private readonly IOptions<ActiveDirectoryOption> _activeDirectoryOption;

        public UserRepository(IOptions<ActiveDirectoryOption> options) 
        {
            _activeDirectoryOption = options ?? throw new ArgumentNullException(nameof(options));
        }

        public Task<IEnumerable<UserEntity>> FindByName(string name, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserEntity>> GetAll(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<UserEntity> GetById(string id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<UserEntity> GetByPrincipalName(string principalName, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserEntity>> GetUsersGroup(string groupId)
        {
            throw new NotImplementedException();
        }
    }
}
