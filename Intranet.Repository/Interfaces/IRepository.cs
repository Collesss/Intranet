using Intranet.Repository.Entities;

namespace Intranet.Repository.Interfaces
{
    public interface IRepository<TEntity, Vid> where TEntity : BaseEntity<Vid>
    {
        Task<IEnumerable<TEntity>> GetAll(CancellationToken cancellationToken = default); 

        Task<TEntity> GetById(Vid id, CancellationToken cancellationToken = default);
    }
}
