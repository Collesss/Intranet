namespace Intranet.Repository.Entities
{
    public class PageResult<T>
    {
        public IEnumerable<T> Items { get; set; }

        public int PageNumber { get; set; }

        public int TotalCount { get; set; }
    }
}
