namespace EFSorter.Filters
{
    public class Filter
    {
        public Condition? Condition1 { get; private set; }
        public string? LogicalOperator { get; private set; }
        public Condition? Condition2 { get; private set; }

        public Filter() { }
        public Filter(Condition condition1, string? logicalOperator, Condition? condition2)
        {
            Condition1 = condition1;
            LogicalOperator = logicalOperator;
            Condition2 = condition2;
        }

        public override string ToString()
            => Condition2 is null || string.IsNullOrEmpty(LogicalOperator) ?
                Condition1.Build() :
                "(" + Condition1.Build() + " " + LogicalOperator + " " + Condition2.Build() + ")";
    }
}
