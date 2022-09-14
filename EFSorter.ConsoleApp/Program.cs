// See https://aka.ms/new-console-template for more information
using Bogus;

using EFSorter.ExtensionsMethod;
using EFSorter.Filters;

Randomizer.Seed = new Random(69);
var Ids = 0;

var test = new Faker<Person>()
    .RuleFor(p => p.Id, f => Ids++)
    .RuleFor(p => p.FirstName, f => f.Name.FullName())
    .RuleFor(p => p.BirthDay, f => f.Date.Past(50).Date);
Console.WriteLine("Before Applying the filter");
Console.WriteLine("<------------------------>");

var list = test.Generate(20);
var filters = new List<Filter>
{
    new(new("BirthDay","date","equals","17/10/1972"),null,null)
};
MultiFilter filter = new(filters);
foreach (Person p in list)
{
    Console.WriteLine(p.BirthDay);
}
var res = list.AsQueryable().ApplyFilters(filter, null);
Console.WriteLine("After Applying the filter");
Console.WriteLine("<------------------------>");
foreach (Person p in res.ToList())
{
    Console.WriteLine(p.BirthDay);
}
var specificDate = list.FirstOrDefault(p => p.BirthDay == DateTime.Parse("17/10/1972 1:40:17 PM"));
Console.WriteLine(specificDate);

//record Person(int Id, string FirstName, DateTime BirthDay);
public class Person
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public DateTime BirthDay { get; set; }
}
