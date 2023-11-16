using System.Data;
using System.Reflection;

using Mapster;

using OfficeOpenXml;
using OfficeOpenXml.Table;

namespace EFSorter.ConsoleApp
{
    internal class ExcelGenerator<T>
    {
        public ExcelGenerator(List<T> items, TableStyles? style = TableStyles.Dark1)
        {
            var dt = ExcelGenerator<T>.ToDataTable(items);
            using (var package = new ExcelPackage())
            {
                SetTheme(1, package);

                var sheet = package.Workbook.Worksheets.Add("Html export sample 1");
                var tableRange = sheet.Cells["A1"].LoadFromDataTable(dt, target => { target.PrintHeaders = true; target.TableStyle = style; });
                //// set number format for the BirthDate column
                //sheet.Cells[tableRange.Start.Row + 1, 4, tableRange.End.Row, 4].Style.Numberformat.Format = "yyyy-MM-dd";
                tableRange.AutoFitColumns();

                var table = sheet.Tables.GetFromRange(tableRange);

                package.SaveAs(new FileInfo("Test.xlsx"));
            }
        }
        private static void SetTheme(int theme, ExcelPackage package)
        {
            if (theme > 0)
            {
                var fileInfo = theme switch
                {
                    1 => new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"themes\\Ion.thmx")),
                    2 => new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"themes\\Banded.thmx")),
                    3 => new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"themes\\Parallax.thmx")),
                    _ => new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"themes\\Ion.thmx")),
                };
                package.Workbook.ThemeManager.Load(fileInfo);
            }
        }

        public static DataTable ToDataTable(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    var propValue = Props[i].GetValue(item, null) ?? "";
                    var propType = Props[i].PropertyType;
                    if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(List<>))
                        values[i] = string.Join(",", propValue.Adapt<List<object>>().Select(o => o.ToString()));
                    else
                        values[i] = propValue;
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        //public DataTable ToDataTable<T>(List<T> items)
        //{
        //    DataTable dataTable = new DataTable(typeof(T).Name);

        //    PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        //    foreach (PropertyInfo prop in props)
        //    {
        //        Type propType = prop.PropertyType;

        //        if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(List<>))
        //        {
        //            // Handle IEnumerable<T> properties
        //            Type itemType = propType.GetGenericArguments()[0];

        //            // If the item type is a complex type, add separate columns for its properties
        //            if (IsComplexType(itemType))
        //            {
        //                foreach (var itemProp in itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        //                {
        //                    // Add columns with a unique name for each occurrence
        //                    dataTable.Columns.Add($"{prop.Name}_{itemProp.Name}");
        //                }
        //            }
        //            else
        //            {
        //                // If the item type is a simple type, add a single column
        //                dataTable.Columns.Add(prop.Name);
        //            }
        //        }
        //        else
        //        {
        //            // Add columns for non-enumerable properties
        //            dataTable.Columns.Add(prop.Name);
        //        }
        //    }

        //    foreach (T item in items)
        //    {
        //        var values = new List<List<object>>();
        //        var firstValue = new List<object>();
        //        values.Add(firstValue);
        //        var firstRow = true;
        //        foreach (PropertyInfo prop in props)
        //        {
        //            Type propType = prop.PropertyType;

        //            if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(List<>))
        //            {
        //                // Handle IEnumerable<T> properties
        //                Type itemType = propType.GetGenericArguments()[0];
        //                var itemListTyped = prop.GetValue(item);
        //                List<object>? itemList = itemListTyped.Adapt<List<object>>();

        //                if (itemList != null)
        //                {
        //                    var secondValue = new List<object>();
        //                    if (firstRow)
        //                    {
        //                        secondValue.AddRange(firstValue);
        //                        firstRow = false;
        //                    }
        //                    foreach (var listItem in itemList)
        //                    {
        //                        if (itemList.Count > 1)
        //                        {
        //                            secondValue = secondValue.Select(i => i = "").ToList();
        //                            values.Add(secondValue);
        //                            if (IsComplexType(itemType))
        //                            {
        //                                foreach (var itemProp in itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        //                                {
        //                                    secondValue.Add(itemProp.GetValue(listItem));
        //                                }
        //                            }
        //                            else
        //                            {
        //                                // If the item type is a simple type, add the values directly
        //                                secondValue.AddRange(itemList);
        //                            }
        //                        }
        //                        else
        //                        {

        //                            if (IsComplexType(itemType))
        //                            {
        //                                foreach (var itemProp in itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        //                                {
        //                                    firstValue.Add(itemProp.GetValue(listItem));
        //                                }
        //                            }
        //                            else
        //                            {
        //                                // If the item type is a simple type, add the values directly
        //                                firstValue.AddRange(itemList);
        //                            }
        //                        }

        //                    }
        //                }
        //                else
        //                {
        //                    // Add null values for the IEnumerable<T> column
        //                    for (int i = 0; i < prop.PropertyType.GetGenericArguments()[0].GetProperties().Length; i++)
        //                    {
        //                        firstValue.Add(null);
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                // Add values for non-enumerable properties
        //                firstValue.Add(prop.GetValue(item));
        //            }
        //        }
        //        foreach (var value in values)
        //            dataTable.Rows.Add(value.ToArray());
        //    }

        //    return dataTable;
        //}



        //private static bool IsComplexType(Type type)
        //{
        //    // Check if a type is a complex type (i.e., not a simple/primitive type)
        //    return type.IsClass && type != typeof(string) && !type.IsPrimitive;
        //}
    }
}
