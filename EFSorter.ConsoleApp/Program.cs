// See https://aka.ms/new-console-template for more information
using Bogus;
using EFSorter.ExtensionsMethod;
using EFSorter.Filters;

Randomizer.Seed = new Random(69);
var Ids = 0;

var test = new Faker<Person>()
    .RuleFor(p => p.Id, f => Ids++)
    .RuleFor(p => p.FirstName, f => f.Name.FullName());
Console.WriteLine("Before Applying the filter");
Console.WriteLine("<------------------------>");

var list = test.Generate(20);
var filters = new List<Filter>
{
    new(new("FirstName","text","equals","Omari Berge"),null,null)
};
MultiFilter filter = new(filters);
foreach (Person p in list)
{
    Console.WriteLine(p.FirstName);
}
var res = list.AsQueryable().ApplyFilters(filter,null);
Console.WriteLine("After Applying the filter");
Console.WriteLine("<------------------------>");
foreach (Person p in res.ToList())
{
    Console.WriteLine(p.FirstName);
}


public class Person
{
    public int Id { get; set; }
    public string FirstName { get; set; }
}
