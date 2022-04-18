using Dapper;

namespace DapperDatabaseInterface.DbContext.MySql;

internal sealed class MySqlDbContext : IDbContext
{
    public MySqlDbContext(string connectionString)
    {
        
    }
    public ICollection<T> Get<T>(string query, object? parameters = null)
    {
        throw new NotImplementedException();
    }

    public async Task<ICollection<T>> GetAsync<T>(string query, object? parameters = null)
    {
        throw new NotImplementedException();
    }

    public void Add<T>(string sql, T data)
    {
        throw new NotImplementedException();
    }

    public void Add(string sql, DynamicParameters parameters)
    {
        throw new NotImplementedException();
    }

    public async Task AddAsync(string sql, DynamicParameters parameters)
    {
        throw new NotImplementedException();
    }

    public void Delete(string sql, object? parameters = null)
    {
        throw new NotImplementedException();
    }

    public void Update<T>(string sql, T data)
    {
        throw new NotImplementedException();
    }

    public void SaveChanges()
    {
        throw new NotImplementedException();
    }

    public async Task SaveChangesAsync()
    {
        throw new NotImplementedException();
    }
}