using System.ComponentModel.DataAnnotations;

namespace Intranet.Models.PageItems.Dto
{
    public class PageItemsDto
    {
        [Range(1, 200)]
        public int PageSize { get; set; } = 50;

        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; } = 1;
    }
}
