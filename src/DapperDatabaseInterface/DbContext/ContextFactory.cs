using DapperDatabaseInterface.DbContext.MySql;
using DapperDatabaseInterface.DbContext.SqlServer;

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