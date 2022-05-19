using System.Data;
using Dapper;

namespace DapperDatabaseInterface.DbContext;

internal abstract class BaseContext : IDbContext
{
    protected readonly string ConnectionString;

    /// <summary>
    ///     List of tuples which stores data to be saved.
    ///     Item1: the sql query
    ///     Item2: data to be saved (or query parameters)
    /// </summary>
    protected readonly List<(string, object?)> DataToBeCommitted;

    protected IDbConnection? Connection;
    protected IDbTransaction? CurrentTran;

    protected BaseContext(string connectionString)
    {
        ConnectionString = connectionString;
        DataToBeCommitted = new List<(string, object?)>();
    }

    public abstract ICollection<T> Get<T>(string query, object? parameters = null);

    public abstract Task<ICollection<T>> GetAsync<T>(string query, object? parameters = null);
    
    public void Add(string sql, object? parametersData)
    {
        DataToBeCommitted.Add((sql, parametersData));
    }
    
    public abstract void Create(string sql, DynamicParameters parameters);

    public abstract Task CreateAsync(string sql, DynamicParameters parameters);
    
    public abstract void SaveChanges();

    public abstract Task SaveChangesAsync();

    protected void Reset()
    {
        CurrentTran = null;
        DataToBeCommitted.Clear();
        if (Connection is not {State: ConnectionState.Open}) return;
        Connection.Close();
        Connection = null;
    }
}