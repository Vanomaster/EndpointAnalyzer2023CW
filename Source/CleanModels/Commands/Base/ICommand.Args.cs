namespace CleanModels.Commands.Base;

/// <summary>
/// Command.
/// </summary>
/// <typeparam name="TArgs">Type of command arguments.</typeparam>
public interface ICommand<in TArgs>
{
    /// <summary>
    /// Execute command.
    /// </summary>
    /// <param name="args">Command arguments.</param>
    /// <returns>Command result.</returns>
    Task<CommandResult> ExecuteAsync(TArgs args); // TODO add cancellation token
}