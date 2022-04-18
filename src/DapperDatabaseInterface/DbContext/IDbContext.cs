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
    ///     Adds new data to the context.
    /// </summary>
    /// <typeparam name="T">Type of object to save.</typeparam>
    /// <param name="sql">SQL Insert Query/Queries.</param>
    /// <param name="data">An object that has the parameters for the query/queries passed.</param>
    /// <exception cref="NullReferenceException">When the data object passed is null.</exception>
    void Add<T>(string sql, T data);

    /// <summary>
    ///     Sends data to the database (on a new transaction) using DynamicParamters in order
    ///     to be able to get 'out' parameters, eg: a database generated primary key.
    /// </summary>
    /// <param name="sql">Sql Insert Query/Queries.</param>
    /// <param name="parameters">Parameters for the query.</param>
    void Add(string sql, DynamicParameters parameters);

    /// <summary>
    ///     Asynchronusly sends data to the database (on a new transaction) using DynamicParamters in order
    ///     to be able to get 'out' parameters, eg: a database generated primary key.
    /// </summary>
    /// <param name="sql">Sql Insert Query/Queries.</param>
    /// <param name="parameters">Parameters for the query.</param>
    Task AddAsync(string sql, DynamicParameters parameters);

    /// <summary>
    ///     Stores data to be deleted in the database context.
    /// </summary>
    /// <param name="sql">SQL Delete Query/Queries.</param>
    /// <param name="parameters">An object that has the parameters for the query/queires passed.</param>
    void Delete(string sql, object? parameters = null);

    /// <summary>
    ///     Stores updated data locally in the database context.
    /// </summary>
    /// <typeparam name="T">Type of object to update.</typeparam>
    /// <param name="sql">SQL Update Query/Queries.</param>
    /// <param name="data">An object that has the parameters for the query/queries passed.</param>
    /// <exception cref="NullReferenceException">When the data object passed is null.</exception>
    void Update<T>(string sql, T data);

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