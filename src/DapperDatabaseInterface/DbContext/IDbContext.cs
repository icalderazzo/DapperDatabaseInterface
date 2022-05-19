using Dapper;

namespace DapperDatabaseInterface.DbContext;

public interface IDbContext
{
    /// <summary>
    ///     Gets a collection of T from the database.
    /// </summary>
    /// <typeparam name="T">Type of return object</typeparam>
    /// <param name="query">Sql Select query.</param>
    /// <param name="parameters">An object that has the parameters for the query</param>
    /// <returns>A List of type T</returns>
    ICollection<T> Get<T>(string query, object? parameters = null);

    /// <summary>
    ///     Gets a collection of T from the database asynchronously.
    /// </summary>
    /// <typeparam name="T">Type of return object.</typeparam>
    /// <param name="query">Sql Select query.</param>
    /// <param name="parameters">An object that has the parameters for the query.</param>
    Task<ICollection<T>> GetAsync<T>(string query, object? parameters = null);

    /// <summary>
    ///     Adds new query with its parameters to the context.
    /// </summary>
    /// <param name="sql">SQL Insert or Update Query/Queries.</param>
    /// <param name="parametersData">An object that has the parameters for the query/queries passed.</param>
    void Add(string sql, object? parametersData);
    
    /// <summary>
    ///     Sends data to the database (on a new transaction) using DynamicParameters in order
    ///     to be able to get 'out' parameters, eg: a database generated primary key.
    /// </summary>
    /// <param name="sql">Sql Insert Query/Queries.</param>
    /// <param name="parameters">Parameters for the query.</param>
    void Create(string sql, DynamicParameters parameters);

    /// <summary>
    ///     Asynchronously sends data to the database (on a new transaction) using DynamicParameters in order
    ///     to be able to get 'out' parameters, eg: a database generated primary key.
    /// </summary>
    /// <param name="sql">Sql Insert Query/Queries.</param>
    /// <param name="parameters">Parameters for the query.</param>
    Task CreateAsync(string sql, DynamicParameters parameters);
    
    /// <summary>
    ///     Sends changes to the database.
    /// </summary>
    void SaveChanges();

    /// <summary>
    ///     Sends changes to the database asychronously.
    /// </summary>
    /// <returns></returns>
    Task SaveChangesAsync();
}