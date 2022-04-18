using DapperDatabaseInterface.DbContext;
using Microsoft.Extensions.DependencyInjection;

namespace DapperDatabaseInterface;

public enum Engine
{
    SqlServer,
    MySql
}

public static class Injector
{
    public static void AddDapperDatabaseInterface(this IServiceCollection services, Engine engine, string connectionString)
    {
        switch (engine)
        {
            case Engine.SqlServer:
                services.AddScoped<IDapperDatabaseInterface>(_ => 
                    new DapperDatabaseInterface(ContextFactory.CreateSqlServerContext(connectionString)));
                break;
            case Engine.MySql:
                services.AddScoped<IDapperDatabaseInterface>(_ => 
                    new DapperDatabaseInterface(ContextFactory.CreateMySqlContext(connectionString)));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(engine), engine, "Invalid engine selected");
        }
    }
}