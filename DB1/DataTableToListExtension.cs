using System.Collections.Generic;
using System.Data;
using System.Reflection;

public static class DataTableToListExtension
{
    public static IList<T> ToList<T>(this DataTable dataTable) where T : new()
    {
        IList<PropertyInfo> properties = typeof(T).GetProperties();
        IList<T> result = new List<T>();

        foreach (DataRow row in dataTable.Rows)
        {
            var item = CreateItem<T>(row, properties);
            result.Add(item);
        }
        return result;
    }

    public static T CreateItem<T>(DataRow row, IList<PropertyInfo> properties) where T : new()
    {
        T item = new T();
        foreach (var property in properties)
        {
            property.SetValue(item, row[property.Name], null);
        }
        return item;
    }
}
