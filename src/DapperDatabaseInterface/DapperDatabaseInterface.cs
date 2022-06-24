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
 
     public T? GetSingle<T>(string query, object? parameters = null)
     {
         return _context.GetSingle<T>(query, parameters);
     }
 
     public async Task<T?> GetSingleAsync<T>(string query, object? parameters = null)
     {
         return await _context.GetSingleAsync<T>(query, parameters);
     }
 
     public void Add(string sql, object? parametersData)
     {
         if (sql.Contains('@') && parametersData == null)
             throw new NullReferenceException("A query with parameters requires parametersData to not be null.");
 
         _context.Add(sql, parametersData); 
     } 
     
     public void Create(string sql, DynamicParameters parameters)
    {
        _context.Create(sql, parameters);
    }
     
     public async Task CreateAsync(string sql, DynamicParameters parameters)
    {
        await _context.CreateAsync(sql, parameters);
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