using NLog;

namespace CleanModels.Queries.Base;

/// <inheritdoc />
public abstract class NonDbQueryBase<TResult> : QueryBase<TResult>
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    /// <inheritdoc/>
    public override async Task<QueryResult<TResult>> ExecuteAsync()
    {
        try
        {
            return await ExecuteCoreAsync();
        }
        catch (Exception exception)
        {
            Logger.Error(exception.ToString());
            return GetFailedResult(Constants.UnidentifiedErrorOccuredOnServer);
        }
    }
}