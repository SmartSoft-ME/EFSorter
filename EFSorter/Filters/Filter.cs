namespace EFSorter.Filters
{
    public class Filter
    {
        public Condition? Condition1 { get; set; }
        public string? LogicalOperator { get; set; }
        public Condition? Condition2 { get; set; }

        public Filter() { }
        public Filter(Condition condition1, string? logicalOperator, Condition? condition2)
        {
            Condition1 = condition1;
            LogicalOperator = logicalOperator;
            Condition2 = condition2;
        }

        public override string ToString()
            => Condition1 is not null ?
                    (Condition2 is null || string.IsNullOrEmpty(LogicalOperator)) ?
                        Condition1.Build() :
                        "(" + Condition1.Build() + " " + LogicalOperator + " " + Condition2.Build() + ")" : "";
    }
}
