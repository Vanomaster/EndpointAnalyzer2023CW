namespace CleanModels.Queries.Base;

/// <inheritdoc />
public abstract class QueryBase<TModel, TResult> : IQuery<TModel, TResult>
{
    /// <inheritdoc/>
    public abstract Task<QueryResult<TResult>> ExecuteAsync(TModel model = default!);

    /// <summary>
    /// Execute query core.
    /// </summary>
    /// <param name="model">Query model.</param>
    /// <returns>Query result.</returns>
    protected abstract Task<QueryResult<TResult>> ExecuteCoreAsync(TModel model);

    /// <summary>
    /// Get successful result.
    /// </summary>
    /// <param name="data">Data.</param>
    /// <returns>Query result.</returns>
    protected QueryResult<TResult> GetSuccessfulResult(TResult data)
    {
        return new QueryResult<TResult>(data: data);
    }

    /// <summary>
    /// Get failed result.
    /// </summary>
    /// <param name="errorMessage">Error message.</param>
    /// <returns>Query result.</returns>
    protected QueryResult<TResult> GetFailedResult(string? errorMessage)
    {
        return new QueryResult<TResult>(errorMessage);
    }
}