namespace EFSorter.Filters
{
    public record MultiFilter(List<Filter> Filters)
    {
        public override string ToString()
        {
            if (Filters.Count > 0)
                return string.Join(" and ", Filters.Select(f => f.ToString()));
            return string.Empty;
        }
    }
}
