namespace EFSorter.Filters
{
    public class MultiSorting
    {
        public string? SortBy { get; set; }

        private MultiSorting() { }
        public MultiSorting(string sortBy)
        {
            SortBy = sortBy;
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(SortBy))
                return string.Empty;
            List<Sorting> sortings = new();
            sortings.AddRange(SortBy.Split(',').Select(s => new Sorting(s)));
            return string.Join(',', sortings.Select(s => s.ToString()));
        }
    }
}
