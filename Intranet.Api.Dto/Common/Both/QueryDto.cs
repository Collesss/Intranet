namespace Intranet.Api.Dto.Common.Both
{
    public class QueryDto
    {
        public PageDto Page { get; set; } = new PageDto();

        public IEnumerable<SortDto> Sorts { get; set; } = new List<SortDto>();

        public IEnumerable<FilterDto> Filters { get; set; } = new List<FilterDto>();

        public QueryDto() { }

        public QueryDto(PageDto page, IEnumerable<SortDto> sorts, IEnumerable<FilterDto> filters)
        {
            Page = page ?? throw new ArgumentNullException(nameof(page));
            Sorts = sorts ?? throw new ArgumentNullException(nameof(sorts));
            Filters = filters ?? throw new ArgumentNullException(nameof(filters));
        }
    }
}
