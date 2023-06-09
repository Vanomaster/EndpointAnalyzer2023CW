using CleanModels.Commands.Base;

namespace Server.Client.Clients.Base;

/// <summary>
/// .
/// </summary>
public interface IServiceClient
{
    public Task<CommandResult> TestAsync(string pcIp);
}