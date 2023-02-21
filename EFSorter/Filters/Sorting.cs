namespace EFSorter.Filters
{
    public class Sorting
    {
        public string SortBy { get; private set; }

        private Sorting() { }
        public Sorting(string sortBy)
        {
            SortBy = sortBy;
        }

        public override string ToString()
        {
            return SortBy.StartsWith('-') ? SortBy.Split('-')[1] + " desc" : SortBy;
        }
    }
}
