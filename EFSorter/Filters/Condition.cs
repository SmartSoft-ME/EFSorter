namespace EFSorter.Filters
{
    public record Condition(string Field, string Type, string Operator, string Value)
    {
        public string Build() => Type switch
        {
            "text" => mapTextCondition(),
            "number" => mapNumberCondition(),
            "date" => mapDateCondition()
            ,
            _ => "",
        };

        private string mapDateCondition()
        {
            var isDate = DateTime.TryParse(Value, out var validDate);
            if (isDate)
                return Operator switch
                {
                    "equals" => Field + "= \"" + validDate.ToString("s") + "\"",
                    "notEqual" => Field + "!= \"" + validDate.ToString("s") + "\"",
                    "lessThan" => Field + "< \"" + validDate.ToString("s") + "\"",
                    "greaterThan" => Field + "> \"" + validDate.ToString("s") + "\"",
                    "blank" => Field + "==null",
                    "notBlank" => Field + "!=null",
                    _ => "",
                };
            return "";
        }

        private string mapNumberCondition() => Operator switch
        {
            "equals" => Field + "=" + Value,
            "notEqual" => Field + "!=" + Value,
            "lessThan" => Field + "<" + Value,
            "lessThanOrEqual" => Field + "<=" + Value,
            "greaterThan" => Field + ">" + Value,
            "greaterThanOrEqual" => Field + ">=" + Value,
            "blank" => Field + "==null",
            "notBlank" => Field + "!=null",
            _ => "",
        };
        private string mapTextCondition() => Operator switch
        {
            "equals" => Field + "=\"" + Value + "\"",
            "notEqual" => Field + "!=\"" + Value + "\"",
            "contains" => Field + ".Contains(\"" + Value + "\")",
            "contains_i" => Field + ".ToLower().Contains(\"" + Value.ToLower() + "\")",
            "notContains" => "!" + Field + ".Contains(\"" + Value + "\")",
            "notContains_i" => "!" + Field + ".ToLower().Contains(\"" + Value.ToLower() + "\")",
            "startsWith" => Field + ".StartsWith(\"" + Value + "\")",
            "endsWith" => Field + ".EndsWith(\"" + Value + "\")",
            "blank" => "string.IsNullOrEmpty(\"" + Field + "\")",
            "notBlank" => "!string.IsNullOrEmpty(\"" + Field + "\")",
            _ => "",
        };
    }
}
