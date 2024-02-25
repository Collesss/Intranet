using Intranet.Api.Dto.Common.Both;

namespace Intranet.Api.Dto.Common.Response
{
    public class PageResultResponseDto<T> : QueryDto
    {
        public IEnumerable<T> Data { get; set; } = new List<T>();

        public int TotalCount { get; set; }


        public PageResultResponseDto() { }

        public PageResultResponseDto(QueryDto queryDto) : base(queryDto.Page, queryDto.Sorts, queryDto.Filters) { }
    }
}
