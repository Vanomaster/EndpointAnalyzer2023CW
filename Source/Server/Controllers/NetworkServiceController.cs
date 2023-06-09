using Grpc.Core;
using NetworkService;
using NLog;
using Server.Services.Base;

namespace Server.Controllers;

/// <inheritdoc />
public class NetworkServiceController : Network.NetworkBase
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// Initializes a new instance of the <see cref="NetworkServiceController"/> class.
    /// </summary>
    /// <param name="analysisService">Benchmark service.</param>
    public NetworkServiceController(INetworkService analysisService)
    {
        NetworkService = analysisService;
    }

    private INetworkService NetworkService { get; }

    /// <inheritdoc/>
    public override async Task<GetHostsWithEaServiceResponse> GetHostsWithEaService(
        GetHostsWithEaServiceRequest model,
        ServerCallContext context)
    {
        var response = await GetHostsWithEaServiceAsync();

        return response;
    }

    private async Task<GetHostsWithEaServiceResponse> GetHostsWithEaServiceAsync()
    {
        var queryResult = await NetworkService.GetHostsWithEaService();
        if (!queryResult.IsSuccessful)
        {
            return new GetHostsWithEaServiceResponse { ErrorMessage = queryResult.ErrorMessage };
        }

        var protoHosts = queryResult.Data.Select(host => host.MapToProtoHost());
        var data = new Data();
        data.Hosts.AddRange(protoHosts);

        return new GetHostsWithEaServiceResponse { Data = data };
    }
}