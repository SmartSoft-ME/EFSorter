namespace EFSorter.Filters
{
    public record MultiFilter(List<Filter> Filters, string BaseOperator = "and")
    {
        public override string ToString()
        {
            if (Filters.Count > 0)
                return string.Join(" " + BaseOperator + " ", Filters.Select(f => f.ToString()));
            return string.Empty;
        }
    }
}
