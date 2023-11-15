using System.Data;
using System.Linq.Dynamic.Core;

using EFSorter.Filters;
namespace EFSorter.ExtensionsMethod
{
    public static class ReadSetExtensions
    {
        public static IQueryable<TModel> ApplyFilters<TModel>(this IQueryable<TModel> source, MultiFilter? filter, MultiSorting? sorting)
        {
            if (sorting is not null)
                source = source.OrderBy(sorting.ToString()).AsQueryable();
            if (filter is not null || !string.IsNullOrEmpty(filter?.ToString()))
                source = source.Where(filter.ToString()).AsQueryable();
            return source;
        }
    }
}
