namespace CleanModels.Commands.Base;

/// <inheritdoc />
public abstract class CommandBase : ICommand
{
    /// <inheritdoc/>
    public abstract Task<CommandResult> ExecuteAsync();

    /// <summary>
    /// Execute command core.
    /// </summary>
    /// <returns>Query result.</returns>
    protected abstract Task<CommandResult> ExecuteCoreAsync();

    /// <summary>
    /// Get successful result.
    /// </summary>
    /// <returns>Command result.</returns>
    protected CommandResult GetSuccessfulResult()
    {
        return new CommandResult();
    }

    /// <summary>
    /// Get failed result.
    /// </summary>
    /// <param name="errorMessage">Error message.</param>
    /// <returns>Command result.</returns>
    protected CommandResult GetFailedResult(string? errorMessage)
    {
        return new CommandResult(errorMessage);
    }
}