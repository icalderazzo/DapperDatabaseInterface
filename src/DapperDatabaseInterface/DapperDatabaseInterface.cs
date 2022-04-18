using Dapper;
using DapperDatabaseInterface.DbContext;

namespace DapperDatabaseInterface;

public class DapperDatabaseInterface : IDapperDatabaseInterface
{
    private readonly IDbContext _context;
    public DapperDatabaseInterface(IDbContext context)
    {
        _context = context;
    }
    
    public ICollection<T> Get<T>(string query, object? parameters = null)
    {
        return _context.Get<T>(query, parameters);
    }

    public async Task<ICollection<T>> GetAsync<T>(string query, object? parameters = null)
    {
        var result = await _context.GetAsync<T>(query, parameters);
        return result;
    }

    public void Add<T>(string sql, T data)
    {
        _context.Add(sql, data);
    }

    public void Add(string sql, DynamicParameters parameters)
    {
        _context.Add(sql, parameters);
    }

    public async Task AddAsync(string sql, DynamicParameters parameters)
    {
        await _context.AddAsync(sql, parameters);
    }

    public void Delete(string sql, object? parameters = null)
    {
        _context.Delete(sql, parameters);
    }

    public void Update<T>(string sql, T data)
    {
        _context.Update(sql, data);
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}