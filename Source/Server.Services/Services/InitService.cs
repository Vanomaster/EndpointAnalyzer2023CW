using CleanModels.Network;
using Dal.Commands.Test;
using Dal.Queries;
using NLog;
using Parsers.Base;
using Server.Client;
using Server.Client.Clients;
using Server.Queries;

namespace Server.Services;

/// <summary>
/// InitService.
/// </summary>
public class InitService
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// Initializes a new instance of the <see cref="InitService"/> class.
    /// </summary>
    /// <param name="testConnectionCommand">TestConnectionCommand.</param>
    /// <param name="channelProvider">ChannelProvider.</param>
    /// <param name="hostIpsWithEaServiceQuery">HostIpsWithOsServiceQuery.</param>
    /// <param name="hostsParser">HostsParser.</param>
    /// <param name="hostNameInfoServiceClient">HostNameInfoServiceClient.</param>
    /// <param name="hostsWithEaService">HostsWithEaService.</param>
    /// <param name="analysisScheduleRecordPcIpQuery">AnalysisScheduleRecordPcNameQuery.</param>
    public InitService(
        TestConnectionCommand testConnectionCommand,
        IChannelProvider channelProvider,
        HostIpsWithEaServiceQuery hostIpsWithEaServiceQuery,
        IParser<string, List<Host>> hostsParser,
        HostNameInfoServiceClient hostNameInfoServiceClient,
        HostsWithEaService hostsWithEaService,
        AnalysisScheduleRecordPcIpQuery analysisScheduleRecordPcIpQuery)
    {
        TestConnectionCommand = testConnectionCommand;
        ChannelProvider = channelProvider;
        HostIpsWithEaServiceQuery = hostIpsWithEaServiceQuery;
        HostsParser = hostsParser;
        AnalysisScheduleRecordPcIpQuery = analysisScheduleRecordPcIpQuery;
        HostNameInfoServiceClient = hostNameInfoServiceClient;
        HostsWithEaService = hostsWithEaService;
    }

    private TestConnectionCommand TestConnectionCommand { get; }

    private IChannelProvider ChannelProvider { get; }

    private HostIpsWithEaServiceQuery HostIpsWithEaServiceQuery { get; }

    private IParser<string, List<Host>> HostsParser { get; }

    private HostNameInfoServiceClient HostNameInfoServiceClient { get; }

    private HostsWithEaService HostsWithEaService { get; }

    private AnalysisScheduleRecordPcIpQuery AnalysisScheduleRecordPcIpQuery { get; }

    public async Task InitAsync()
    {
        await TestDbConnection();
        await InitHosts();
        // await InitChannels();
    }

    private async Task TestDbConnection()
    {
        Logger.Info("InitService TestDbConnection started.");
        var commandResult = await TestConnectionCommand.ExecuteAsync();
        if (!commandResult.IsSuccessful)
        {
            Logger.Error($"InitService TestDbConnection failed. {commandResult.ErrorMessage}");
        }

        Logger.Info("InitService TestDbConnection successfully completed.");
    }

    private async Task InitHosts()
    {
        Logger.Info("InitService InitHosts started.");
        var hostIpsQueryResult = await HostIpsWithEaServiceQuery.ExecuteAsync();
        if (!hostIpsQueryResult.IsSuccessful)
        {
            Logger.Error($"InitService InitHosts failed. {hostIpsQueryResult.ErrorMessage}");

            return;
        }

        var parseResult = HostsParser.Parse(hostIpsQueryResult.Data);
        if (!parseResult.IsSuccessful)
        {
            Logger.Error($"InitService InitHosts failed. {parseResult.ErrorMessage}");

            return;
        }

        foreach (var host in parseResult.Data)
        {
            ChannelProvider.InitChannelsForIp(host.Ip, 1);
            var queryResult = await HostNameInfoServiceClient.GetAsync(host.Ip);
            if (!queryResult.IsSuccessful)
            {
                Logger.Error($"InitService InitHosts failed. {queryResult.ErrorMessage}");

                return;
            }

            host.Name = queryResult.Data.Trim();
            if (!HostsWithEaService.TryAdd(host))
            {
                Logger.Error("InitService InitHosts failed. Host can not be added to HostsWithEAService");
            }

            // ChannelProvider.RemoveChannelsForIp(host.Ip);
        }

        Logger.Info("InitService InitHosts successfully completed.");
    }

    private async Task InitChannels()
    {
        Logger.Info("InitService InitChannels started.");
        var queryResult = await AnalysisScheduleRecordPcIpQuery.ExecuteAsync();
        if (!queryResult.IsSuccessful)
        {
            Logger.Error($"InitService InitChannels failed. {queryResult.ErrorMessage}");

            return;
        }

        foreach (string pcIp in queryResult.Data)
        {
            ChannelProvider.InitChannelsForIp(pcIp);
        }

        Logger.Info("InitService InitChannels successfully completed.");
    }
}