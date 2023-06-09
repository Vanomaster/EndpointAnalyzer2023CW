namespace CleanModels.Commands.Base;

/// <inheritdoc />
public abstract class NonDbCommandBase : CommandBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NonDbCommandBase{TArgs}"/> class.
    /// </summary>
    protected NonDbCommandBase()
    {
    }

    /// <inheritdoc/>
    public override async Task<CommandResult> ExecuteAsync()
    {
        try
        {
            return await ExecuteCoreAsync();
        }
        catch (Exception exception)
        {
            return GetFailedResult(exception.ToString());
        }
    }
}