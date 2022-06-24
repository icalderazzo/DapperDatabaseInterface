using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace DapperDatabaseInterface.Helpers;

internal static class DbConnectionFactory
{
    internal static DbConnection CreateConnection<T>(string connectionString) where T : IDbConnection
    {
        if (typeof(T) == typeof(SqlConnection)) return new SqlConnection(connectionString);
        if (typeof(T) == typeof(MySqlConnection)) return new MySqlConnection(connectionString);

        throw new InvalidOperationException();
    }
}