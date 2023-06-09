using CleanModels.Network;
using CleanModels.Queries.Base;
using NLog;
using Parsers.Base;
using Server.Queries;
using Server.Services.Base;

namespace Server.Services;

public class NetworkService : INetworkService
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// Initializes a new instance of the <see cref="NetworkService"/> class.
    /// </summary>
    /// <param name="hostIpsWithEaServiceQuery">AnalysisScheduleRecordPcNameQuery.</param>
    /// <param name="hostsParser">Hosts parser.</param>
    public NetworkService(
        HostIpsWithEaServiceQuery hostIpsWithEaServiceQuery,
        IParser<string, List<Host>> hostsParser,
        HostsWithEaService hostsWithEaService)
    {
        HostsWithEaService = hostsWithEaService;
        // HostIpsWithOsServiceQuery = hostIpsWithOsServiceQuery;
        // HostsParser = hostsParser;
    }

    // private HostIpsWithOsServiceQuery HostIpsWithOsServiceQuery { get; }
    //
    // private IParser<string, List<Host>> HostsParser { get; }
    private HostsWithEaService HostsWithEaService { get; }

    public async Task<QueryResult<List<Host>>> GetHostsWithEaService()
    {
        Logger.Info("NetworkService GetHostsWithEaService started.");
        // var queryResult = await HostsWithOsServiceQuery.ExecuteAsync();
        // if (!queryResult.IsSuccessful)
        // {
        //     Logger.Error($"NetworkService GetHostsWithOsService failed. {queryResult.ErrorMessage}");
        //
        //     return new QueryResult<List<Host>>(queryResult.ErrorMessage);
        // }
        //
        // var parseResult = HostsParser.Parse(queryResult.Data);
        // if (!parseResult.IsSuccessful)
        // {
        //     Logger.Error($"NetworkService GetHostsWithOsService failed. {queryResult.ErrorMessage}");
        //
        //     return new QueryResult<List<Host>>(parseResult.ErrorMessage);
        // }
        var hostsWithEaService = HostsWithEaService.GetAll();
        Logger.Info("NetworkService GetHostsWithEaService successfully completed.");

        return new QueryResult<List<Host>>(hostsWithEaService);
    }

    public QueryResult<Host> GetHostWithEaService(string pcIp)
    {
        Logger.Info("NetworkService GetHostWithEaService started.");
        var queryResult = HostsWithEaService.Get(pcIp);
        if (!queryResult.IsSuccessful)
        {
            Logger.Error($"NetworkService GetHostWithEaService failed. {queryResult.ErrorMessage}");

            return new QueryResult<Host>(queryResult.ErrorMessage);
        }

        Logger.Info("NetworkService GetHostWithEaService successfully completed.");

        return new QueryResult<Host>(queryResult.Data);
    }
}