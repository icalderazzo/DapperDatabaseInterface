using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DapperDatabaseInterface.DbContext.SqlServer;

internal sealed class SqlServerDbContext : IDbContext
{
    private readonly string _connectionString;

    /// <summary>
    ///     List of tuples which stores data to be saved.
    ///     Item1: the sql query
    ///     Item2: data to be saved (or query parameters)
    /// </summary>
    private readonly List<(string, object?)> _dataToSave;

    private IDbConnection? _connection;
    private IDbTransaction? _currentTran;

    public SqlServerDbContext(string connectionString)
    {
        _connectionString = connectionString;
        _dataToSave = new List<(string, object?)>();
    }

    public void Add<T>(string sql, T data)
    {
        if (data == null)
            throw new NullReferenceException();

        _dataToSave.Add((sql, data));
    }

    public void Add(string sql, DynamicParameters parameters)
    {
        try
        {
            _connection = new SqlConnection(_connectionString);
            _connection.Open();
            _currentTran = _connection.BeginTransaction();
            _connection.Execute(sql, parameters, _currentTran);
        }
        catch (Exception)
        {
            _currentTran?.Rollback();
            Reset();
            throw;
        }
    }

    public async Task AddAsync(string sql, DynamicParameters parameters)
    {
        try
        {
            _connection = new SqlConnection(_connectionString);
            _connection.Open();
            _currentTran = _connection.BeginTransaction();
            await _connection.ExecuteAsync(sql, parameters, _currentTran);
        }
        catch (Exception)
        {
            _currentTran?.Rollback();
            Reset();
            throw;
        }
    }

    public void Delete(string sql, object? parameters = null)
    {
        _dataToSave.Add((sql, parameters));
    }

    public ICollection<T> Get<T>(string query, object? parameters = null)
    {
        if (_connection != null) return _connection.Query<T>(query, parameters).ToList();

        using (IDbConnection conn = new SqlConnection(_connectionString))
        {
            var dbResult = conn.Query<T>(query, parameters);
            return dbResult.ToList();
        }
    }

    public async Task<ICollection<T>> GetAsync<T>(string query, object? parameters = null)
    {
        if (_connection != null)
        {
            var result = await _connection.QueryAsync<T>(query, parameters);
            return result.ToList();
        }

        using (IDbConnection conn = new SqlConnection(_connectionString))
        {
            var dbResult = await conn.QueryAsync<T>(query, parameters);
            return dbResult.ToList();
        }
    }

    public void SaveChanges()
    {
        try
        {
            if (_connection == null)
            {
                _connection = new SqlConnection(_connectionString);
                _connection.Open();
                _currentTran = _connection.BeginTransaction();
            }

            foreach (var data in _dataToSave) _currentTran.Connection.Execute(data.Item1, data.Item2, _currentTran);
            _currentTran.Commit();
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
            if (_connection == null)
            {
                _connection = new SqlConnection(_connectionString);
                _connection.Open();
                _currentTran = _connection.BeginTransaction();
            }

            foreach (var (item1, item2) in _dataToSave)
                await _currentTran.Connection.ExecuteAsync(item1, item2, _currentTran);
            _currentTran.Commit();
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

    public void Update<T>(string sql, T data)
    {
        if (data == null)
            throw new NullReferenceException();

        _dataToSave.Add((sql, data));
    }

    /// <summary>
    ///     Resets the current state of the context.
    /// </summary>
    private void Reset()
    {
        _currentTran = null;
        _dataToSave.Clear();
        if (_connection is not {State: ConnectionState.Open}) return;
        _connection.Close();
        _connection = null;
    }
}