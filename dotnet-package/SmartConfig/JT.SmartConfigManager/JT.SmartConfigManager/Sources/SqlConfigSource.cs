using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JT.SmartConfigManager.Sources
{
    public class SqlConfigSource : IConfigSource
    {
        private readonly string? _connectionString;
        private readonly string? _query;

        public SqlConfigSource(string? connectionString, string? query)
        {
            // Don't throw — allow null or blank values
            _connectionString = connectionString;
            _query = query;
        }
        public async Task<Dictionary<string, string>> LoadAsync()
        {
            var result = new Dictionary<string, string>();

            if (string.IsNullOrWhiteSpace(_connectionString) || string.IsNullOrWhiteSpace(_query))
            {
                Console.WriteLine("⚠️ SqlConfigSource skipped: missing connection string or query.");
                return result;
            }

            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new SqlCommand(_query, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var key = reader.GetString(0);
                var value = reader.GetString(1);
                result[key] = value;
            }

            return result;
        }

    }
}
