using AutoMapper;
using Intranet.Repository.ActiveDirectory.Options;
using Intranet.Repository.Entities;
using Intranet.Repository.Interfaces;
using Microsoft.Extensions.Options;
using System.DirectoryServices;

namespace Intranet.Repository.ActiveDirectory.Implementaions
{
    public class UserRepository : IUserRepository
    {
        private readonly ActiveDirectoryOption _activeDirectoryOption;
        private readonly DirectoryEntry _directoryEntry;
        private readonly IMapper _mapper;

        public UserRepository(IOptions<ActiveDirectoryOption> options, IMapper mapper) 
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _activeDirectoryOption = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _directoryEntry = new DirectoryEntry(options.Value.LDAPConnectionString);
        }

        public Task<IEnumerable<UserEntity>> FindByName(string name, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<UserEntity>> GetAll(CancellationToken cancellationToken = default)
        {
            DirectorySearcher searcher = new DirectorySearcher(_directoryEntry, $"(&(objectClass=user)(objectCategory=person))");

            searcher.SizeLimit = 100;

            IEnumerable<SearchResult> searchResultCollection = searcher.FindAll().Cast<SearchResult>();

            return await Task.FromResult(_mapper.Map<IEnumerable<SearchResult>, IEnumerable<UserEntity>>(searchResultCollection));
        }

        public async Task<UserEntity> GetById(string id, CancellationToken cancellationToken = default)
        {
            DirectorySearcher searcher = new DirectorySearcher(_directoryEntry, $"(&(objectClass=user)(objectCategory=person)(objectSid={id}))");

            SearchResult searchResult = searcher.FindOne();

            return await Task.FromResult(_mapper.Map<SearchResult, UserEntity>(searchResult));

            //throw new NotImplementedException();
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
