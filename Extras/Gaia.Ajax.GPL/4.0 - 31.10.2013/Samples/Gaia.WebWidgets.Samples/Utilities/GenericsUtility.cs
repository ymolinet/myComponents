namespace Gaia.WebWidgets.Samples.Utilities
{
    using System.Collections.Generic;
    using System.Data;
    using System.Reflection;

    public class GenericsUtility
    {
        /// <summary>
        /// Convert a List{T} to a DataTable.
        /// </summary>
        public static DataTable ToDataTable<T>(IEnumerable<T> items)
        {
            DataTable tb = new DataTable(typeof(T).Name);

            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in props)
                tb.Columns.Add(prop.Name, prop.PropertyType);

            foreach (T item in items)
            {
                object[] values = new object[props.Length];

                for (int i = 0; i < props.Length; i++)
                    values[i] = props[i].GetValue(item, null);

                tb.Rows.Add(values);
            }

            return tb;
        }
    }
}
