using DinamicCharts.Application.DTOs.ConnectionInfo;
using DinamicCharts.Application.Interfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DinamicCharts.Infrasturucture
{
    public class DbInspectorServices : IDbInspectorServices
    {
        private string BuildConnectionString(ConnectionInfoDto connection)
        {
            return $"Server={connection.Host};Database={connection.DbAdi};User Id={connection.Username};Password={connection.Password};TrustServerCertificate=True;";
        }

        public async Task<List<string>> GetViewsAsync(ConnectionInfoDto connection)
        {
            var result = new List<string>();
            using SqlConnection conn = new(BuildConnectionString(connection));
            await conn.OpenAsync();

            string query = "SELECT name FROM sys.views";
            using SqlCommand cmd = new(query, conn);
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
                result.Add(reader.GetString(0));

            return result;
        }

        public async Task<List<string>> GetStoredProceduresAsync(ConnectionInfoDto connection)
        {
            var result = new List<string>();
            using SqlConnection conn = new(BuildConnectionString(connection));
            await conn.OpenAsync();

            string query = "SELECT name FROM sys.procedures WHERE type = 'P'";
            using SqlCommand cmd = new(query, conn);
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
                result.Add(reader.GetString(0));

            return result;
        }

        public async Task<List<string>> GetFunctionsAsync(ConnectionInfoDto connection)
        {
            var result = new List<string>();
            using SqlConnection conn = new(BuildConnectionString(connection));
            await conn.OpenAsync();

            string query = "SELECT name FROM sys.objects WHERE type IN ('IF', 'TF')";
            using SqlCommand cmd = new(query, conn);
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
                result.Add(reader.GetString(0));

            return result;
        }
        public async Task<List<Dictionary<string, object>>> ExecuteObjectAsync(ConnectionInfoDto connection, string objectType, string objectName)
        {
            var resultList = new List<Dictionary<string, object>>();

            using SqlConnection conn = new(BuildConnectionString(connection));
            await conn.OpenAsync();

            string sql = objectType.ToLower() switch
            {
                "view" => $"SELECT * FROM [{objectName}]",
                "procedure" => $"EXEC [{objectName}]",
                "function" => $"SELECT * FROM [{objectName}]()",
                _ => throw new ArgumentException("Geçersiz objectType")
            };

            using var cmd = new SqlCommand(sql, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var row = new Dictionary<string, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                    row[reader.GetName(i)] = reader.GetValue(i);

                resultList.Add(row);
            }

            return resultList;
        }
    }
}
