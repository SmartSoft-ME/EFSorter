namespace EFSorter.Filters
{
    public record MultiSorting(string sortBy)
    {
        public override string ToString()
        {
            if (string.IsNullOrEmpty(sortBy))
                return string.Empty;
            var sortings = new List<Sorting>();
            sortings.AddRange(sortBy.Split(',').Select(s => new Sorting(s)));

            return string.Join(',', sortings.Select(s => s.ToString()));
        }
    }
}
