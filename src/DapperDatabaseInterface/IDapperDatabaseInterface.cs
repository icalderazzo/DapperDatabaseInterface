using Dapper;

namespace DapperDatabaseInterface;

public interface IDapperDatabaseInterface
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
    ///     Adds an instruction to be executed (Insert, Update, Delete). 
    ///     Requires execution of SaveChanges or SaveChangesAsync for the changes to take effect.
    /// </summary>
    /// <param name="sql">SQL Insert, Update, Delete Query/Queries.</param>
    /// <param name="parametersData">An object that has the parameters for the query/queries passed.</param>
    /// <exception cref="NullReferenceException">When the sql instruction has parameters
    /// and parametersData object passed is null.</exception>
    void Add(string sql, object? parametersData);
    
    /// <summary>
    ///     Sends data to the database (on a new transaction) using DynamicParameters in order
    ///     to be able to get 'out' parameters, eg: a database generated primary key.
    /// 
    ///     This should be only executed as the first step of a new transaction.
    /// </summary>
    /// <param name="sql">Sql Insert Query/Queries.</param>
    /// <param name="parameters">Parameters for the query.</param>
    void Create(string sql, DynamicParameters parameters);

    /// <summary>
    ///     Asynchronously sends data to the database (on a new transaction) using DynamicParameters in order
    ///     to be able to get 'out' parameters, eg: a database generated primary key.
    ///
    ///     This should be only executed as the first step of a new transaction.
    /// </summary>
    /// <param name="sql">Sql Insert Query/Queries.</param>
    /// <param name="parameters">Parameters for the query.</param>
    Task CreateAsync(string sql, DynamicParameters parameters);
    
    /// <summary>
    ///     Saves changes in the database using a new transaction. 
    /// </summary>
    void SaveChanges();

    /// <summary>
    ///     Sends changes to the database asynchronously using a new transaction.
    /// </summary>
    /// <returns></returns>
    Task SaveChangesAsync();
}