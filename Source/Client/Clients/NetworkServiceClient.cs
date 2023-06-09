using CleanModels.Queries.Base;
using Client.Clients.Base;
using NetworkService;
using static NetworkService.Network;
using HostModel = CleanModels.Network.Host;

namespace Client.Clients;

/// <inheritdoc />
public class NetworkServiceClient : ReadOnlyServiceClientBase<NetworkClient, HostModel>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NetworkServiceClient"/> class.
    /// </summary>
    /// <param name="client">Client.</param>
    public NetworkServiceClient(NetworkClient client)
         : base(client)
    {
    }

    /// <inheritdoc/>
    public override async Task<QueryResult<List<HostModel>>> GetAsync()
    {
        var request = new GetHostsWithEaServiceRequest();
        var response = await Client.GetHostsWithEaServiceAsync(request);
        if (!string.IsNullOrEmpty(response.ErrorMessage))
        {
            return new QueryResult<List<HostModel>>(response.ErrorMessage);
        }

        var hosts = response.Data.Hosts
            .Select(r => r.MapToHost())
            .ToList();

        return new QueryResult<List<HostModel>>(data: hosts);
    }
}