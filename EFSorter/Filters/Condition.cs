namespace EFSorter.Filters
{
    public record Condition(string Field, string Type, string Operator, string Value)
    {
        public string Build() => Type switch
        {
            "text" => mapTextCondition(),
            "number" => mapNumberCondition(),
            _ => "",
        };
        private string mapNumberCondition() => Operator switch
        {
            "equals" => Field + "=" + Value.ToString(),
            "notEqual" => Field + "!=" + Value.ToString(),
            "lessThan" => Field + "<" + Value.ToString(),
            "lessThanOrEqual" => Field + "<=" + Value.ToString(),
            "greaterThan" => Field + ">" + Value.ToString(),
            "greaterThanOrEqual" => Field + ">=" + Value.ToString(),
            "blank" => Field + "==null",
            "notBlank" => Field + "!=null",
            _ => "",
        };
        private string mapTextCondition() => Operator switch
        {
            "equals" => Field + "=\"" + Value.ToString() + "\"",
            "notEqual" => Field + "!=\"" + Value.ToString() + "\"",
            "contains" => Field + ".Contains(\"" + Value.ToString() + "\")",
            "contains_i" => Field + ".ToLower().Contains(\"" + Value.ToLower().ToString() + "\")",
            "notContains" => "!" + Field + ".Contains(\"" + Value.ToString() + "\")",
            "notContains_i" => "!" + Field + ".ToLower().Contains(\"" + Value.ToLower().ToString() + "\")",
            "startsWith" => Field + ".StartsWith(\"" + Value.ToString() + "\")",
            "endsWith" => Field + ".EndsWith(\"" + Value.ToString() + "\")",
            "blank" => "string.IsNullOrEmpty(\"" + Field.ToString() + "\")",
            "notBlank" => "!string.IsNullOrEmpty(\"" + Field.ToString() + "\")",
            _ => "",
        };
    }
}
