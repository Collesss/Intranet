using Intranet.Repository.Db.Extensions;
using Intranet.Repository.Entities;
using Intranet.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.RegularExpressions;

namespace Intranet.Repository.Db.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly RepositoryDbContext _dbContext;

        public UserRepository(RepositoryDbContext dbContext) 
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IEnumerable<UserEntity>> GetAll(CancellationToken cancellationToken = default) =>
            await _dbContext.Users.ToListAsync(cancellationToken);
            
        public async Task<UserEntity> GetById(int id, CancellationToken cancellationToken = default) =>
            await _dbContext.Users.FindAsync(new object[] { id }, cancellationToken);

        public async Task<UserEntity> GetByPrincipalName(string principalName, CancellationToken cancellationToken = default) =>
            await _dbContext.Users.FirstOrDefaultAsync(user => user.UserPrincipalName == principalName, cancellationToken);

        public async Task<UserEntity> GetBySid(string sid, CancellationToken cancellationToken = default) =>
            await _dbContext.Users.FirstOrDefaultAsync(user => user.SID == sid, cancellationToken);

        public async Task<PageResult<UserEntity>> GetPage(Query query, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(nameof(query));

            CheckPage(query.Page);
            CheckSorts(query.Sorts);
            CheckFilters(query.Filters);

            query.Filters = query.Filters.Select(filter => new Filter() { Field = filter.Field, StringSearch = $".+{Regex.Replace(filter.StringSearch, "\\*", ".*")}.+" });

            IQueryable<UserEntity> collection = _dbContext.Users.AsQueryable();

            collection = query.Sorts.Aggregate(collection, (coll, sort) => coll.OrderBy(sort.Field, sort.Asc));
            collection = collection.WhereRegexAnd(query.Filters.Select(filter => (filter.Field, filter.StringSearch)));

            int Count = 0;
            IEnumerable<UserEntity> users = null;
            int pageNum = 0;

            using (var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.Snapshot, cancellationToken))
            {
                Count = await collection.CountAsync(cancellationToken);

                int maxPage = (Count / query.Page.PageSize) + ((Count % query.Page.PageSize) > 0 ? 1 : 0);

                pageNum = query.Page.PageNumber <= maxPage ? query.Page.PageNumber : maxPage;

                users = collection
                    .Include(user => user.Phones)
                    .Skip((pageNum - 1) * query.Page.PageSize)
                    .Take(query.Page.PageSize)
                    .AsEnumerable();
            }

            return new PageResult<UserEntity>() { Items = users, PageNumber = pageNum, TotalCount = Count };
        }


        private readonly string[] validFields = new string[]
        {
            "Id",
            "SID",
            "UserPrincipalName",
            "DisplayName",
            "Email"
        };

        private void CheckPage(Page page)
        {
            ArgumentNullException.ThrowIfNull(page, nameof(page));

            if (page.PageSize <= 0)
                throw new ArgumentException("page.PageSize can't less or equal 0.", nameof(page));

            if (page.PageNumber <= 0)
                throw new ArgumentException("page.PageNumber can't less or equal 0.", nameof(page));
        }

        private void CheckSorts(IEnumerable<Sort> sorts)
        {
            ArgumentNullException.ThrowIfNull(sorts, nameof(sorts));

            string[] fieldsSort = sorts.Select(sort => sort.Field).ToArray();

            if (!fieldsSort.All(fieldSort => validFields.Contains(fieldSort)))
                throw new ArgumentException($"Sorts list contains not accepted fields. Accepted fields: {string.Join(", ", validFields)}.", nameof(sorts));

            if (fieldsSort.GroupBy(fieldSort => fieldSort)
                .Any(groupedFields => groupedFields.Count() > 1))
                throw new ArgumentException("Sort list contains duplicated fields.", nameof(sorts));
        }

        private void CheckFilters(IEnumerable<Filter> filters)
        {
            ArgumentNullException.ThrowIfNull(filters, nameof(filters));

            string[] fieldsFilter = filters.Select(filter => filter.Field).ToArray();

            string[] stringsSearchFilter = filters.Select(filter => filter.StringSearch).ToArray();


            if (!fieldsFilter.All(fieldFilter => validFields.Contains(fieldFilter)))
                throw new ArgumentException($"Filter list contains not accepted fields. Accepted fields: {string.Join(", ", validFields)}.", nameof(filters));

            if (fieldsFilter.GroupBy(fieldFiltert => fieldFiltert)
                .Any(groupedFields => groupedFields.Count() > 1))
                throw new ArgumentException("Filter list contains duplicated fields.", nameof(filters));

            string regexStr = "^[a-zA-Z*@_0-9\\-а-яА-ЯёЁ ]+$";
            Regex regex = new Regex(regexStr);

            if (!stringsSearchFilter.All(regex.IsMatch))
                throw new ArgumentException($"Filter list contains not accepted SearchString. Regex for accepted SearchString: {regexStr}.", nameof(filters));
        }
    }
}
