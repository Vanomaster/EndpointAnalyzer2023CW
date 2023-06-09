using CleanModels;
using CleanModels.Queries.Base;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace Dal.Queries.Base;

/// <inheritdoc />
public abstract class DbQueryBase<TResult> : QueryBase<TResult>
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// Initializes a new instance of the <see cref="DbQueryBase{TResult}"/> class.
    /// </summary>
    /// <param name="contextFactory">Context factory.</param>
    protected DbQueryBase(IDbContextFactory<Context> contextFactory)
    {
        ContextFactory = contextFactory;
    }

    /// <summary>
    /// Context.
    /// </summary>
    protected Context Context { get; private set; } = null!;

    private IDbContextFactory<Context> ContextFactory { get; }

    /// <inheritdoc/>
    public override async Task<QueryResult<TResult>> ExecuteAsync()
    {
        try
        {
            Context = await ContextFactory.CreateDbContextAsync();
            return await ExecuteCoreAsync();
        }
        catch (Exception exception)
        {
            Logger.Error(exception.ToString());
            return GetFailedResult(Constants.UnidentifiedErrorOccuredOnServer);
        }
    }
}