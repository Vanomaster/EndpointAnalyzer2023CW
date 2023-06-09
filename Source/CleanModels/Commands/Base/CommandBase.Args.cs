namespace CleanModels.Commands.Base;

/// <inheritdoc />
public abstract class CommandBase<TArgs> : ICommand<TArgs>
{
    /// <inheritdoc/>
    public abstract Task<CommandResult> ExecuteAsync(TArgs args);

    /// <summary>
    /// Execute command core.
    /// </summary>
    /// <param name="args">Command arguments.</param>
    /// <returns>Query result.</returns>
    protected abstract Task<CommandResult> ExecuteCoreAsync(TArgs args);

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