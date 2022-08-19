namespace EFSorter.Filters
{
    public record Filter(Condition Condition1, string? LogicalOperator, Condition? Condition2)
    {
        public override string ToString()
            => Condition2 is null || string.IsNullOrEmpty(LogicalOperator) ?
                Condition1.Build() :
                "(" + Condition1.Build() + " " + LogicalOperator + " " + Condition2.Build() + ")";
    }
}
