using System.Data;
using Dapper;
using DapperDatabaseInterface.Helpers;

namespace DapperDatabaseInterface.DbContext;

/// <summary>
///     Base database context with all the operations.
/// </summary>
/// <typeparam name="TConnectionType">The type of the connection the context is going to be using.</typeparam>
internal abstract class ContextBase<TConnectionType> : IDbContext where TConnectionType : IDbConnection
{
    /// <summary>
    ///     The connection string of the database.
    /// </summary>
    private readonly string _connectionString;

    /// <summary>
    ///     List of tuples which stores data to be saved.
    ///     Item1: the sql query
    ///     Item2: data to be saved (or query parameters)
    /// </summary>
    private readonly List<(string, object?)> _dataToBeCommitted;

    /// <summary>
    ///     Current connection
    /// </summary>
    private IDbConnection? _connection;

    /// <summary>
    ///     Current transaction
    /// </summary>
    private IDbTransaction? _currentTran;

    /// <summary>
    ///     The constructor
    /// </summary>
    /// <param name="connectionString"></param>
    protected ContextBase(string connectionString)
    {
        _connectionString = connectionString;
        _dataToBeCommitted = new List<(string, object?)>();
    }

    public ICollection<T> Get<T>(string query, object? parameters = null)
    {
        if (_connection is not null) return _connection.Query<T>(query, parameters, _currentTran).ToList();

        using IDbConnection conn = DbConnectionFactory.CreateConnection<TConnectionType>(_connectionString);
        return conn.Query<T>(query, parameters).ToList();
    }

    public async Task<ICollection<T>> GetAsync<T>(string query, object? parameters = null)
    {
        if (_connection is not null) return (await _connection.QueryAsync<T>(query, parameters, _currentTran)).ToList();

        using IDbConnection conn = DbConnectionFactory.CreateConnection<TConnectionType>(_connectionString);
        return (await conn.QueryAsync<T>(query, parameters)).ToList();
    }

    public T? GetSingle<T>(string query, object? parameters = null)
    {
        if (_connection is not null) return _connection.QuerySingle<T>(query, parameters, _currentTran);

        using IDbConnection conn = DbConnectionFactory.CreateConnection<TConnectionType>(_connectionString);
        return conn.QuerySingle<T>(query, parameters);
    }

    public async Task<T?> GetSingleAsync<T>(string query, object? parameters = null)
    {
        if (_connection is not null) return await _connection.QuerySingleAsync<T>(_connectionString);

        using IDbConnection conn = DbConnectionFactory.CreateConnection<TConnectionType>(_connectionString);
        return await _connection.QuerySingleAsync<T>(query, parameters);
    }

    public void Add(string sql, object? parametersData)
    {
        _dataToBeCommitted.Add((sql, parametersData));
    }

    public void Create(string sql, DynamicParameters parameters)
    {
        try
        {
            UpdateContextState();
            _connection.Execute(sql, parameters, _currentTran);
        }
        catch (Exception)
        {
            _currentTran?.Rollback();
            Reset();
            throw;
        }
    }

    public async Task CreateAsync(string sql, DynamicParameters parameters)
    {
        try
        {
            UpdateContextState();
            await _connection.ExecuteAsync(sql, parameters, _currentTran);
        }
        catch (Exception)
        {
            _currentTran?.Rollback();
            Reset();
            throw;
        }
    }

    public void SaveChanges()
    {
        try
        {
            UpdateContextState();

            foreach (var (sql, paramsData) in _dataToBeCommitted)
                _connection.Execute(sql, paramsData, _currentTran);

            _currentTran!.Commit();
        }
        catch (Exception)
        {
            _currentTran?.Rollback();
            throw;
        }
        finally
        {
            Reset();
        }
    }

    public async Task SaveChangesAsync()
    {
        try
        {
            UpdateContextState();

            foreach (var (sql, paramsData) in _dataToBeCommitted)
                await _connection.ExecuteAsync(sql, paramsData, _currentTran);

            _currentTran!.Commit();
        }
        catch (Exception)
        {
            _currentTran?.Rollback();
            throw;
        }
        finally
        {
            Reset();
        }
    }

    /// <summary>
    ///     Updates the state: opens connection and transaction if not opened yet
    /// </summary>
    private void UpdateContextState()
    {
        _connection ??= DbConnectionFactory.CreateConnection<TConnectionType>(_connectionString);
        if (_connection.State is ConnectionState.Closed) _connection.Open();
        _currentTran ??= _connection.BeginTransaction();
    }

    /// <summary>
    ///     Resets context state: closes connection and transaction and also clears data to be committed.
    /// </summary>
    private void Reset()
    {
        _currentTran = null;
        _dataToBeCommitted.Clear();
        if (_connection is not {State: ConnectionState.Open}) return;
        _connection.Close();
        _connection = null;
    }
}