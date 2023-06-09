using CleanModels;
using CleanModels.Commands.Base;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace Dal.Commands.Base;

/// <inheritdoc />
public abstract class DbCommandBase : CommandBase
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// Initializes a new instance of the <see cref="DbCommandBase{TArgs}"/> class.
    /// </summary>
    /// <param name="contextFactory">Context factory.</param>
    protected DbCommandBase(IDbContextFactory<Context> contextFactory)
    {
        ContextFactory = contextFactory;
    }

    /// <summary>
    /// Context.
    /// </summary>
    protected Context Context { get; private set; } = null!;

    private IDbContextFactory<Context> ContextFactory { get; }

    /// <inheritdoc/>
    public override async Task<CommandResult> ExecuteAsync()
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