using MySql.Data.MySqlClient;

namespace DapperDatabaseInterface.DbContext.MySql;

internal sealed class MySqlDbContext : ContextBase<MySqlConnection>
{
    public MySqlDbContext(string connectionString) : base(connectionString)
    {
    }
}