using System.ComponentModel;
using System.Data;
using System.Reflection;

namespace EnergyDistribution.Domain.Services
{
    public static class ListExtensions
    {
        public static DataTable ToDataTable<T>(this List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            // Obtener todas las propiedades
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            List<PropertyInfo> propiedades = new List<PropertyInfo>();

            // Crear columnas para DataTable
            foreach (PropertyInfo prop in Props)
            {
                if (prop.Name != "Id" && prop.Name != "Tramo") // Excluir propiedades no necesarias
                {
                    dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                    propiedades.Add(prop);
                }
            }

            foreach (T item in items)
            {
                var values = new object[propiedades.Count];
                for (int i = 0; i < propiedades.Count; i++)
                {
                    values[i] = propiedades[i].GetValue(item, null)!;
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }
    }
}
