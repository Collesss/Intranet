namespace Intranet.Models.PageItems.Dto
{
    public class ItemsDto
    {
        public PageItemsDto Page { get; set; } = new PageItemsDto();

        public IEnumerable<SortItemsDto> Sorts { get; set; } = Enumerable.Empty<SortItemsDto>();

        public IEnumerable<FilterItemsDto> Filters { get; set; } = Enumerable.Empty<FilterItemsDto>();
    }
}
