namespace EFSorter.Filters
{
    public class Condition
    {
        public string Field { get; set; }
        public string Type { get; set; }
        public string Operator { get; set; }
        public string? Value { get; set; }

        public Condition() { }

        public Condition(string field, string type, string @operator, string? value)
        {
            Field = field;
            Type = type;
            Operator = @operator;
            Value = value;
        }

        public string Build()
        {
            if (!Field.StartsWith('@') && (Field.Contains("guid", StringComparison.InvariantCultureIgnoreCase) || Field.Contains("parent", StringComparison.InvariantCultureIgnoreCase)))
                Field = Field.Insert(0, "@");
            Value = Value.Contains('\\') ? Value.Replace("\\", "") : Value;

            return Type.ToLowerInvariant() switch
            {
                "text" => MapTextCondition(),
                "number" => MapNumberCondition(),
                "date" => MapDateCondition(),
                "datetime" => MapDateTimeCondition(),
                "guids" => MapGuidsCondition(),
                "object" => MapObjectCondition(),
                _ => "",
            };
        }

        private string MapDateCondition()
        {
            var validDate = string.IsNullOrEmpty(Value) ? DateTime.UtcNow : DateTime.Parse(Value).ToUniversalTime().Date;
            return Operator switch
            {
                "equals" => $"{Field}.Date = DateTime({validDate.Ticks},1)",
                "notEqual" => $"{Field}.Date != DateTime({validDate.Ticks},1)",
                "lessThan" => $"{Field}.Date < DateTime({validDate.Ticks},1)",
                "greaterThan" => $"{Field}.Date > DateTime({validDate.Ticks},1)",
                "blank" => $"np({Field}.Date) == null",
                "notBlank" => $"np({Field}.Date) != null",
                _ => "",
            };
        }
        private string MapDateTimeCondition()
        {
            var isDate = DateTime.TryParse(Value, out var validDate);
            if (!isDate) return "";

            return Operator switch
            {
                "equals" => $"{Field} = \"{validDate:s}\"",
                "notEqual" => $"{Field} != \"{validDate:s}\"",
                "lessThan" => $"{Field} < \"{validDate:s}\"",
                "greaterThan" => $"{Field} > \"{validDate:s}\"",
                "blank" => $"{Field} != null",
                "notBlank" => $"{Field} == null",
                _ => "",
            };
        }

        private string MapNumberCondition() => Operator switch
        {
            "equals" => $"{Field} = {Value}",
            "notEqual" => $"{Field} != {Value}",
            "lessThan" => $"{Field} < {Value}",
            "lessThanOrEqual" => $"{Field} <= {Value}",
            "greaterThan" => $"{Field} > {Value}",
            "greaterThanOrEqual" => $"{Field} >= {Value}",
            "blank" => $"{Field} != null",
            "notBlank" => $"{Field} == null",
            _ => "",
        };

        private string MapTextCondition()
            => Field.Split('.').Length > 1 ? MapNestedTextCondition() : MapNotNestedTextCondition();

        private string MapNotNestedTextCondition()
        {
            return Operator switch
            {
                "equals" => $"{Field} = \"{Value}\"",
                "notEqual" => $"{Field} != \"{Value}\"",
                "contains" => $"{Field}.Contains(\"{Value}\")",
                "contains_i" => $"{Field}.ToLower().Contains(\"{Value.ToLower()}\")",
                "notContains" => $"!{Field}.Contains(\"{Value}\")",
                "notContains_i" => $"!{Field}.ToLower().Contains(\"{Value.ToLower()}\")",
                "startsWith" => $"{Field}.StartsWith(\"{Value}\")",
                "endsWith" => $"{Field}.EndsWith(\"{Value}\")",
                "blank" => $"!string.IsNullOrEmpty({Field})",
                "notBlank" => $"string.IsNullOrEmpty({Field})",
                _ => "",
            };
        }
        private string MapNestedTextCondition() => Operator switch
        {
            "equals" => $"np({Field}) = \"{Value}\"",
            "notEqual" => $"np({Field}) != \"{Value}\"",
            "contains" => $"np({Field}) != null && {Field}.Contains(\"{Value}\")",
            "contains_i" => $"np({Field}) != null && {Field}.ToLower().Contains(\"{Value.ToLower()}\")",
            "notContains" => $"np({Field}) != null && !{Field}.Contains(\"{Value}\")",
            "notContains_i" => $"np({Field}) != null &&!{Field}.ToLower().Contains(\"{Value.ToLower()}\")",
            "startsWith" => $"np({Field}) != null &&{Field}.StartsWith(\"{Value}\")",
            "endsWith" => $"np({Field}) != null &&{Field}.EndsWith(\"{Value}\")",
            "blank" => $"!string.IsNullOrEmpty(np({Field}))",
            "notBlank" => $"string.IsNullOrEmpty(np({Field}))",
            _ => "",
        };
        private string MapGuidsCondition() => Operator switch
        {
            "in" => $"{Field}.Any(x => x.Guid == \"{Value}\")",
            "notIn" => $"!{Field}.Any(x => x.Guid == \"{Value}\")",
            _ => "",
        };

        private string MapObjectCondition() => Operator switch
        {
            "blank" => $"{Field} != null",
            "notBlank" => $"{Field} == null",
            _ => "",
        };
    }
}
