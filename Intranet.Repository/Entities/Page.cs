namespace Intranet.Repository.Entities
{
    public class Page<T>
    {
        public IEnumerable<T> Items { get; set; }

        public int PageNumber {  get; set; }

        public int TotalCount { get; set; }
    }
}
