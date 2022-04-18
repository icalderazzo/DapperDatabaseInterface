using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DapperDatabaseInterface.DbContext.SqlServer;

internal sealed class SqlServerDbContext : BaseContext
{
    public SqlServerDbContext(string connectionString)
        : base(connectionString)
    {
    }

    public override void Add(string sql, DynamicParameters parameters)
    {
        try
        {
            Connection = new SqlConnection(ConnectionString);
            Connection.Open();
            CurrentTran = Connection.BeginTransaction();
            Connection.Execute(sql, parameters, CurrentTran);
        }
        catch (Exception)
        {
            CurrentTran?.Rollback();
            Reset();
            throw;
        }
    }

    public override async Task AddAsync(string sql, DynamicParameters parameters)
    {
        try
        {
            Connection = new SqlConnection(ConnectionString);
            Connection.Open();
            CurrentTran = Connection.BeginTransaction();
            await Connection.ExecuteAsync(sql, parameters, CurrentTran);
        }
        catch (Exception)
        {
            CurrentTran?.Rollback();
            Reset();
            throw;
        }
    }

    public override ICollection<T> Get<T>(string query, object? parameters = null)
    {
        if (Connection != null) return Connection.Query<T>(query, parameters).ToList();

        using (IDbConnection conn = new SqlConnection(ConnectionString))
        {
            var dbResult = conn.Query<T>(query, parameters);
            return dbResult.ToList();
        }
    }

    public override async Task<ICollection<T>> GetAsync<T>(string query, object? parameters = null)
    {
        if (Connection != null)
        {
            var result = await Connection.QueryAsync<T>(query, parameters);
            return result.ToList();
        }

        using (IDbConnection conn = new SqlConnection(ConnectionString))
        {
            var dbResult = await conn.QueryAsync<T>(query, parameters);
            return dbResult.ToList();
        }
    }

    public override void SaveChanges()
    {
        try
        {
            if (Connection == null)
            {
                Connection = new SqlConnection(ConnectionString);
                Connection.Open();
                CurrentTran = Connection.BeginTransaction();
            }

            foreach (var data in DataToSave) CurrentTran.Connection.Execute(data.Item1, data.Item2, CurrentTran);
            CurrentTran.Commit();
        }
        catch (Exception)
        {
            CurrentTran?.Rollback();
            throw;
        }
        finally
        {
            Reset();
        }
    }

    public override async Task SaveChangesAsync()
    {
        try
        {
            if (Connection == null)
            {
                Connection = new SqlConnection(ConnectionString);
                Connection.Open();
                CurrentTran = Connection.BeginTransaction();
            }

            foreach (var (item1, item2) in DataToSave)
                await CurrentTran.Connection.ExecuteAsync(item1, item2, CurrentTran);
            CurrentTran.Commit();
        }
        catch (Exception)
        {
            CurrentTran?.Rollback();
            throw;
        }
        finally
        {
            Reset();
        }
    }
}