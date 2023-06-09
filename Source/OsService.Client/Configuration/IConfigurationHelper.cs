namespace OsService.Client.Configuration;

/// <summary>
/// Configuration helper.
/// </summary>
public interface IConfigurationHelper
{
    /// <summary>
    /// Address of main server.
    /// </summary>
    public string? ServerAddress { get; }
}