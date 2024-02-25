using AutoMapper;
using Intranet.Api.Db;
using Intranet.Api.Db.Entities;
using Intranet.Api.Dto.Common.Both;
using Intranet.Api.Dto.Common.Response;
using Intranet.Api.Dto.User.Response;
using Intranet.Api.Dto.ValidationAttributes;
using Intranet.Api.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.RegularExpressions;

namespace Intranet.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IntranetApiDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public UserController(IntranetApiDbContext dbContext, IMapper mapper, ILogger<UserController> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("{sid}")]
        [ProducesResponseType(typeof(UserEntity), StatusCodes.Status200OK)]
        public async Task<ActionResult<UserEntity>> GetUserBySid(string sid, CancellationToken cancellationToken) =>
            await _dbContext.Users.FirstAsync(user => user.SID == sid, cancellationToken);


        [HttpGet]
        [ProducesResponseType(typeof(PageResultResponseDto<UserResponseDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PageResultResponseDto<UserResponseDto>>> GetPage([QueryValidationAttribute<UserResponseDto>][FromQuery] QueryDto query, CancellationToken cancellationToken)
        {
            query.Sorts = query.Sorts.DefaultIfEmpty(new SortDto() { Field = "DisplayName", Asc = true });

            IQueryable<UserEntity> collection = _dbContext.Users.AsQueryable();

            collection = collection.OrderBy(GetArrTupleSorts(query.Sorts));
            collection = collection.WhereRegexAnd(GetArrTupleFilters(query.Filters));

            int count = 0;
            IEnumerable<UserEntity> users = null;

            using (var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken))
            {
                count = await collection.CountAsync(cancellationToken);

                int maxPage = (count / query.Page.PageSize) + ((count % query.Page.PageSize) > 0 ? 1 : 0);

                query.Page.PageNumber = query.Page.PageNumber <= maxPage ? query.Page.PageNumber : maxPage;

                users = collection
                    .Include(user => user.Phones)
                    .Skip((query.Page.PageNumber - 1) * query.Page.PageSize)
                    .Take(query.Page.PageSize)
                    .AsEnumerable();
            }

            return Ok(new PageResultResponseDto<UserResponseDto>()
            {
                Data = _mapper.Map<IEnumerable<UserEntity>, IEnumerable<UserResponseDto>>(users),
                TotalCount = count
            });
        }



        private IEnumerable<(string field, bool asc)> GetArrTupleSorts(IEnumerable<SortDto> sorts) =>
            sorts.Select(sort => (TransformField(sort.Field), sort.Asc));

        private IEnumerable<(string field, string stringSearch)> GetArrTupleFilters(IEnumerable<FilterDto> filters) =>
            filters.Select(filter => (TransformField(filter.Field), $".*{Regex.Replace(filter.StringSearch, "\\*", ".*")}.*"));


        private string TransformField(string field)
        {
            Dictionary<string, string> transDictFields = new Dictionary<string, string>()
            {
                ["Id"] = "Id",
                ["SID"] = "SID",
                ["UserPrincipalName"] = "UserPrincipalName",
                ["DisplayName"] = "DisplayName",
                ["Email"] = "Email"
            };

            return transDictFields.TryGetValue(field, out string transformedFields) ? transformedFields : throw new Exception("field not contains in transDictFields.");
        }
    }
}
