namespace EFSorter.Filters
{
    public class Sorting
    {
        public string SortBy { get; set; }

        private Sorting() { }
        public Sorting(string sortBy)
            => SortBy = sortBy;

        public override string ToString()
            => SortBy.StartsWith('-') ? SortBy.Split('-')[1] + " desc" : SortBy;
    }
}
