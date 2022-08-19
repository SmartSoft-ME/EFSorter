namespace EFSorter.Filters
{
    public record Sorting(string SortBy)
    {
        public override string ToString()
        {
            return SortBy.StartsWith('-') ? SortBy.Split('-')[1] + " desc" : SortBy;
        }
    }
}
