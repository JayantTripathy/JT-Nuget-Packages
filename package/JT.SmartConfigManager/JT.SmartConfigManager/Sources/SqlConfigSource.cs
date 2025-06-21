using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JT.SmartConfigManager.Sources
{
    public class SqlConfigSource : IConfigSource
    {
        private readonly string _connectionString;
        private readonly string _tableName;

        public SqlConfigSource(string connectionString, string tableName)
        {
            _connectionString = connectionString;
            _tableName = tableName;
        }

        public Dictionary<string, string> Load()
        {
            var result = new Dictionary<string, string>();

            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            var cmd = new SqlCommand($"SELECT [Key], [Value] FROM {_tableName}", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result[reader.GetString(0)] = reader.GetString(1);
            }

            return result;
        }
    }
}
