namespace CleanModels.Commands.Base;

/// <inheritdoc />
public abstract class CommandBase<TArgs, TResult> : ICommand<TArgs, TResult>
{
    /// <inheritdoc/>
    public abstract Task<CommandResult<TResult>> ExecuteAsync(TArgs args);

    /// <summary>
    /// Execute command core.
    /// </summary>
    /// <param name="args">Command arguments.</param>
    /// <returns>Query result.</returns>
    protected abstract Task<CommandResult<TResult>> ExecuteCoreAsync(TArgs args);

    /// <summary>
    /// Get successful result.
    /// </summary>
    /// <param name="data">Data.</param>
    /// <typeparam name="TResult">Type of command result.</typeparam>
    /// <returns>Command result.</returns>
    protected CommandResult<TResult> GetSuccessfulResult(TResult data = default!)
    {
        return new CommandResult<TResult>(data: data);
    }

    /// <summary>
    /// Get failed result.
    /// </summary>
    /// <param name="errorMessage">Error message.</param>
    /// <returns>Command result.</returns>
    protected CommandResult<TResult> GetFailedResult(string? errorMessage)
    {
        return new CommandResult<TResult>(errorMessage);
    }
}