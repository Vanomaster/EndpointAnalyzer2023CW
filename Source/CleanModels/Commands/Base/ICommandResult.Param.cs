namespace CleanModels.Commands.Base;

/// <summary>
/// Command result.
/// </summary>
public interface ICommandResult<TResult>
{
    /// <summary>
    /// Data.
    /// </summary>
    public TResult Data { get; }

    /// <summary>
    /// Error message.
    /// </summary>
    public string? ErrorMessage { get; }

    /// <summary>
    /// Whether executed successfully.
    /// </summary>
    public bool IsSuccessful { get; }
}