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
    protected readonly List<(string, object?)> DataToSave;

    protected IDbConnection? Connection;
    protected IDbTransaction? CurrentTran;

    protected BaseContext(string connectionString)
    {
        ConnectionString = connectionString;
        DataToSave = new List<(string, object?)>();
    }

    public abstract ICollection<T> Get<T>(string query, object? parameters = null);

    public abstract Task<ICollection<T>> GetAsync<T>(string query, object? parameters = null);

    public void Add<T>(string sql, T data)
    {
        if (data == null)
            throw new NullReferenceException("Data cannot be null");

        DataToSave.Add((sql, data));
    }

    public abstract void Add(string sql, DynamicParameters parameters);

    public abstract Task AddAsync(string sql, DynamicParameters parameters);

    public void Delete(string sql, object? parameters = null)
    {
        DataToSave.Add((sql, parameters));
    }

    public void Update<T>(string sql, T data)
    {
        if (data == null)
            throw new NullReferenceException();

        DataToSave.Add((sql, data));
    }

    public abstract void SaveChanges();

    public abstract Task SaveChangesAsync();

    protected void Reset()
    {
        CurrentTran = null;
        DataToSave.Clear();
        if (Connection is not {State: ConnectionState.Open}) return;
        Connection.Close();
        Connection = null;
    }
}