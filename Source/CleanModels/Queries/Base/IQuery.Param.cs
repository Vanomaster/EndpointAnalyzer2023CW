namespace CleanModels.Queries.Base;

/// <summary>
/// Query.
/// </summary>
/// <typeparam name="TModel">Type of query model.</typeparam>
/// <typeparam name="TResult">Type of query result.</typeparam>
public interface IQuery<in TModel, TResult>
{
    /// <summary>
    /// Execute query.
    /// </summary>
    /// <param name="model">Query model.</param>
    /// <returns>Query result.</returns>
    Task<QueryResult<TResult>> ExecuteAsync(TModel model);
}