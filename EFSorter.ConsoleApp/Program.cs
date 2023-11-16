using System.Linq.Dynamic.Core;

using Bogus;

using EFSorter.ExtensionsMethod;
using EFSorter.Filters;
Randomizer.Seed = new Random(69);
var Ids = 0;

var test = new Faker<Person>()
    .RuleFor(p => p.Id, f => Ids++)
    .RuleFor(p => p.FirstName, f => f.Name.FullName())
    .RuleFor(p => p.BirthDay, f => f.Date.Past(50).Date)
    .RuleFor(p => p.Addresses, f => new() { new(Guid.NewGuid(), "TEST") });
Console.WriteLine("Before Applying the filter");
Console.WriteLine("<------------------------>");
var list = test.Generate(20);
list.FirstOrDefault()!.Addresses.FirstOrDefault()!.Guid = new Guid("e97eef8f-5636-42a5-894e-9401c65c21dc");
foreach (var item in list)
{
    if (item.Id % 2 == 1)
        item.Blank = "A";
}
var filters = new List<Filter>
{
    new(new("BirthDay","date","equals","12/17/1996"),null,null)
};
var anotherFilter = new List<Filter>
{
    new(new("Blank","object","blank",""),null,null)
};
var yetAnotherFilter = new List<Filter>
{
    new(new("Addresses","guids","notIn","e97eef8f-5636-42a5-894e-9401c65c21dc"),null,null)
};

MultiFilter filter = new(filters);
MultiFilter filter1 = new(anotherFilter);
MultiFilter filter2 = new(yetAnotherFilter);
foreach (Person p in list)
{
    Console.WriteLine(p.BirthDay);
}
var res = list.AsQueryable().ApplyFilters(filter, null);
var res1 = list.AsQueryable().ApplyFilters(filter1, null);
var res2 = list.AsQueryable().ApplyFilters(filter2, null);
var res3 = list.AsQueryable().ApplyFilters(null, null);
Console.WriteLine("After Applying the filter");
Console.WriteLine("<------------------------>");
foreach (Person p in res.ToList())
{
    Console.WriteLine(p);
}

var specificDate = list.FirstOrDefault(p => p.BirthDay == DateTime.Parse("12/17/1996 12:00:00 AM"));
Console.WriteLine(specificDate);

//record Person(int Id, string FirstName, DateTime BirthDay);
public record Person
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public DateTime BirthDay { get; set; }
    public string? Blank { get; set; }
    public List<Address> Addresses { get; set; } = new();
}

public record Address
{
    public Address(Guid guid, string name)
    {
        Guid = guid;
        Name = name;
    }

    public Guid Guid { get; set; }
    public string Name { get; set; }
}
