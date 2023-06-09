namespace CleanModels.Queries.Base;

/// <summary>
/// Query.
/// </summary>
/// <typeparam name="TResult">Type of query result.</typeparam>
public interface IQuery<TResult>
{
    /// <summary>
    /// Execute query.
    /// </summary>
    /// <returns>Query result.</returns>
    Task<QueryResult<TResult>> ExecuteAsync();
}