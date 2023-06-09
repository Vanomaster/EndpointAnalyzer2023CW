namespace CleanModels.Commands.Base;

/// <inheritdoc />
public abstract class NonDbCommandBase<TArgs> : CommandBase<TArgs>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NonDbCommandBase{TArgs}"/> class.
    /// </summary>
    protected NonDbCommandBase()
    {
    }

    /// <inheritdoc/>
    public override async Task<CommandResult> ExecuteAsync(TArgs args)
    {
        try
        {
            return await ExecuteCoreAsync(args);
        }
        catch (Exception exception)
        {
            return GetFailedResult(exception.ToString());
        }
    }
}