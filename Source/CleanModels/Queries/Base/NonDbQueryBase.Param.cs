using NLog;

namespace CleanModels.Queries.Base;

/// <inheritdoc />
public abstract class NonDbQueryBase<TModel, TResult> : QueryBase<TModel, TResult>
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    /// <inheritdoc/>
    public override async Task<QueryResult<TResult>> ExecuteAsync(TModel model = default!)
    {
        try
        {
            return await ExecuteCoreAsync(model);
        }
        catch (Exception exception)
        {
            Logger.Error(exception.ToString());
            return GetFailedResult(Constants.UnidentifiedErrorOccuredOnServer);
        }
    }
}