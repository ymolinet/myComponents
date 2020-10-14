using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myComponents.MySqlDataReaderExtensions
{
    public static class MySqlDataReaderExtensions
    {
        public static string GetSafeString(this MySqlDataReader reader, int colIndex)
        {
            return reader.IsDBNull(colIndex) ? string.Empty : reader.GetString(colIndex);
        }

        public static string GetSafeString(this MySqlDataReader reader, string colName)
        {
            return reader.GetSafeString(reader.GetOrdinal(colName));
        }
    }
}
