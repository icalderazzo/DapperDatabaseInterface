using Microsoft.Data.SqlClient;

namespace DapperDatabaseInterface.DbContext.SqlServer;

internal sealed class SqlServerDbContext : ContextBase<SqlConnection>
{
    public SqlServerDbContext(string connectionString) : base(connectionString)
    {
    }
}