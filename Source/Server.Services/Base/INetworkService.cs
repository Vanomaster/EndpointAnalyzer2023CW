using CleanModels.Network;
using CleanModels.Queries.Base;

namespace Server.Services.Base;

public interface INetworkService
{
    public Task<QueryResult<List<Host>>> GetHostsWithEaService();

    public QueryResult<Host> GetHostWithEaService(string pcIp);
}