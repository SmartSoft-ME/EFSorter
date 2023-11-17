using System.Linq.Dynamic.Core;

using Bogus;

using EFSorter.ConsoleApp;
using EFSorter.ExtensionsMethod;
using EFSorter.Filters;

using OfficeOpenXml;

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
Randomizer.Seed = new Random(69);
var Ids = 0;
var test = new Faker<Person>()
    .RuleFor(p => p.Id, f => Ids++)
    .RuleFor(p => p.FirstName, f => f.Name.FullName())
    .RuleFor(p => p.BirthDay, f => f.Date.Past(50).Date)
    .RuleFor(p => p.Addresses, f => new() { new(Guid.NewGuid(), "TEST"), new(Guid.NewGuid(), "TEST1") })
    .RuleFor(p => p.Nachos, f => new() { new(Guid.NewGuid(), "Natcho"), new(Guid.NewGuid(), "Natcho") });

var people = test.Generate(20);
people.First().Addresses.FirstOrDefault()!.Guid = new Guid("e97eef8f-5636-42a5-894e-9401c65c21dc");
Console.WriteLine("Before Applying the filter");
Console.WriteLine("<------------------------>");
foreach (var person in people)
{
    person.Blank = person.Id % 2 == 1 ? "A" : "";
    Console.WriteLine(person.FirstName + ", " + person.BirthDay);
}

// filters
var dateFilter = new List<Filter>
{
    new(new("BirthDay","date","equals","12/17/1996"),null,null)
};
var blankFilter = new List<Filter>
{
    new(new("Blank","text","notBlank",""),null,null)
};
var notInFilter = new List<Filter>
{
    new(new("Addresses","guids","notIn","e97eef8f-5636-42a5-894e-9401c65c21dc"),null,null)
};
var threeFilters = new List<Filter>
{
    new(new("FirstName","text","contains_i","e"), "or", new("FirstName","text","contains_i","a")),
    new(new("Id", "number", "equals", "1"), null, null)
};

var filter = new MultiFilter(dateFilter);
var filter1 = new MultiFilter(blankFilter);
var filter2 = new MultiFilter(notInFilter);
var filter3 = new MultiFilter(threeFilters, "and");

var res = people.AsQueryable().ApplyFilters(filter3, null);

Console.WriteLine("After Applying the filter");
Console.WriteLine("<------------------------>");
res.ToList().ForEach(Console.WriteLine);

var t = new ExcelGenerator<Person>(people);

public record Person
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public DateTime BirthDay { get; set; }
    public string? Blank { get; set; }
    public List<Address> Addresses { get; set; } = new();
    public List<Nacho> Nachos { get; set; } = new();
}

public record Address
{
    public Guid Guid { get; set; }
    public string Name { get; set; }
    public Address(Guid guid, string name)
    {
        Guid = guid;
        Name = name;
    }
    public override string ToString() => Name;
}
public record Nacho
{
    public Guid Guid { get; set; }
    public string Name { get; set; }
    public Nacho(Guid guid, string name)
    {
        Guid = guid;
        Name = name;
    }
    public override string ToString() => Name;
}
