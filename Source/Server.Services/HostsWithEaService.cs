using System.Collections.Concurrent;
using CleanModels.Network;
using CleanModels.Queries.Base;

namespace Server.Services;

public class HostsWithEaService
{
    private readonly ConcurrentDictionary<string, Host> hostsWithEaServiceByIps = new ();

    public bool TryAdd(Host host)
    {
        return hostsWithEaServiceByIps.TryAdd(host.Ip, host);
    }

    public List<Host> GetAll()
    {
        return hostsWithEaServiceByIps.Values.ToList();
    }

    public QueryResult<Host> Get(string pcIp)
    {
        if (hostsWithEaServiceByIps.TryGetValue(pcIp, out var host))
        {
            return new QueryResult<Host>(host);
        }

        return new QueryResult<Host>("Адреса нет в списке компьютеров с сервисом.");
    }

    public bool TryRemove(Host host)
    {
        return hostsWithEaServiceByIps.TryRemove(host.Ip, out _);
    }
}