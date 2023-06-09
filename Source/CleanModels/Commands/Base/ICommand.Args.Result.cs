namespace CleanModels.Commands.Base;

/// <summary>
/// Command.
/// </summary>
/// <typeparam name="TArgs">Type of command arguments.</typeparam>
/// <typeparam name="TResult">Type of command result.</typeparam>
public interface ICommand<in TArgs, TResult>
{
    /// <summary>
    /// Execute command.
    /// </summary>
    /// <param name="args">Command arguments.</param>
    /// <returns>Command result.</returns>
    Task<CommandResult<TResult>> ExecuteAsync(TArgs args);
}