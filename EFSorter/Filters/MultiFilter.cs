namespace EFSorter.Filters
{
    public class MultiFilter
    {
        public List<Filter> Filters { get; set; }
        public string? BaseOperator { get; set; } = "and";

        private MultiFilter() { }
        public MultiFilter(List<Filter> filters, string? baseOperator = "and")
        {
            Filters = filters;
            BaseOperator = baseOperator;
        }

        public override string ToString()
        {
            if (Filters.Count > 0)
                return string.Join(" " + (BaseOperator ?? "and") + " ", Filters.Select(f => f.ToString()));
            return string.Empty;
        }
    }
}
