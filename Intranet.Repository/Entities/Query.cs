namespace Intranet.Repository.Entities
{
    public class Query
    {
        public Page Page { get; set; }

        public IEnumerable<Sort> Sorts { get; set; } = Enumerable.Empty<Sort>();

        public IEnumerable<Filter> Filters { get; set; } = Enumerable.Empty<Filter>();
    }
}
