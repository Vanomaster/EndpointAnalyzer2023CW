using Grpc.Core;

namespace Client.Clients.Base;

/// <summary>
/// .
/// </summary>
/// <typeparam name="TClient">..</typeparam>
/// <typeparam name="TModel">...</typeparam>
public abstract class ServiceClientBase<TClient> : IServiceClient<TClient>
where TClient : ClientBase
{
    protected ServiceClientBase(TClient client)
    {
        Client = client;
    }

    protected TClient Client { get; }
}