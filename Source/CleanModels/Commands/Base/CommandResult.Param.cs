namespace CleanModels.Commands.Base;

/// <inheritdoc />
public class CommandResult<TResult> : ICommandResult<TResult>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CommandResult"/> class.
    /// </summary>
    /// <param name="data">Data.</param>
    public CommandResult(TResult data = default!)
    {
        Data = data;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandResult"/> class.
    /// </summary>
    /// <param name="errorMessage">Error message.</param>
    public CommandResult(string? errorMessage)
    {
        ErrorMessage = errorMessage;
    }

    /// <inheritdoc/>
    public TResult Data { get; } = default!;

    /// <summary>
    /// Error message.
    /// </summary>
    public string? ErrorMessage { get; }

    /// <summary>
    /// Whether executed successfully.
    /// </summary>
    public bool IsSuccessful => ErrorMessage == null;
}