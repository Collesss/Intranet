﻿namespace Intranet.Repository.Entities
{
    public class PageResult<T>
    {
        public IEnumerable<T> Items { get; set; } = new List<T>();

        public int PageNumber { get; set; }

        public int TotalCount { get; set; }
    }
}
