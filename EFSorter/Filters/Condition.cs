﻿namespace EFSorter.Filters
{
    public class Condition
    {
        public string Field { get; set; }
        public string Type { get; set; }
        public string Operator { get; set; }
        public string Value { get; set; }

        public Condition() { }

        public Condition(string field, string type, string @operator, string value)
        {
            Field = field;
            Type = type;
            Operator = @operator;
            Value = type == "number" ? value : "\"" + value + "\"";
        }

        public string Build() => Type.ToLowerInvariant() switch
        {
            "text" => MapTextCondition(),
            "number" => MapNumberCondition(),
            "date" => MapDateCondition(),
            "guids" => MapGuidsCondition(),
            "object" => MapObjectCondition(),
            _ => "",
        };

        private string MapDateCondition()
        {
            var isDate = DateTime.TryParse(Value, out var validDate);
            if (!isDate) return "";

            return Operator switch
            {
                "equals" => $"{Field} = {validDate:s}",
                "notEqual" => $"{Field} != {validDate:s}",
                "lessThan" => $"{Field} < {validDate:s}",
                "greaterThan" => $"{Field} > {validDate:s}",
                "blank" => $"{Field} == null",
                "notBlank" => $"{Field} != null",
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
            "blank" => $"{Field} == null",
            "notBlank" => $"{Field} != null",
            _ => "",
        };

        private string MapTextCondition() => Operator switch
        {
            "equals" => $"{Field} = {Value}",
            "notEqual" => $"{Field} != {Value}",
            "contains" => $"{Field}.Contains({Value})",
            "contains_i" => $"{Field}.ToLower().Contains({Value.ToLower()})",
            "notContains" => $"!{Field}.Contains({Value})",
            "notContains_i" => $"!{Field}.ToLower().Contains({Value.ToLower()})",
            "startsWith" => $"{Field}.StartsWith({Value})",
            "endsWith" => $"{Field}.EndsWith({Value})",
            "blank" => $"string.IsNullOrEmpty({Field})",
            "notBlank" => $"!string.IsNullOrEmpty({Field})",
            _ => "",
        };

        private string MapGuidsCondition() => Operator switch
        {
            "in" => $"{Field}.Any(x => x.Guid == {Value})",
            "notIn" => $"!{Field}.Any(x => x.Guid == {Value})",
            _ => "",
        };

        private string MapObjectCondition() => Operator switch
        {
            "blank" => $"{Field} == null",
            "notBlank" => $"{Field} != null",
            _ => "",
        };
    }
}
