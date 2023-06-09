using CleanModels.Queries.Base;
using NLog;

namespace Server.Queries;

/// <inheritdoc />
public class HostIpsWithEaServiceQuery : CommandLineQueryBase<string> // get all hosts in network with 12124 opened port
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    /// <inheritdoc/>
    protected override async Task<QueryResult<string>> ExecuteCoreAsync()
    {
        // TODO У masscan задержка после работы 10 сек. Стоит пользоваться nmap, пока он работает не более 10 сек. Перейти на masscan, когда nmap начнёт работать более 10 сек.
        string hosts = await ExecuteBashAsync("sudo masscan -p12124 --range 10.1.0.0-10.1.1.254 --rate=1000000"); // nmap -n --privileged --send-eth --min-rate 999999 --min-hostgroup 4096 --max-retries 0 --defeat-rst-ratelimit -T2 -PS -p 12124 10.1.0.* -oG -
        Logger.Error(hosts);

        return GetSuccessfulResult(hosts);
    }
}