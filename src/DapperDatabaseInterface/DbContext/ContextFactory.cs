using DapperDatabaseInterface.DbContext.SqlServer;
using DapperDatabaseInterface.DbContext.MySql;

namespace DapperDatabaseInterface.DbContext;

internal static class ContextFactory
{
    public static SqlServerDbContext CreateSqlServerContext(string connectionString)
    {
        return new SqlServerDbContext(connectionString);
    }

    public static MySqlDbContext CreateMySqlContext(string connectionString)
    {
        return new MySqlDbContext(connectionString);
    }
}