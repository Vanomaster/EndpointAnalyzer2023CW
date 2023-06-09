using CleanModels.Queries.Base;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using OsInfoService;

namespace Server.Client.Clients.Base;

/// <summary>
/// .
/// </summary>
/// <typeparam name="TClient">..</typeparam>
/// <typeparam name="TResult">...</typeparam>
public abstract class ServiceClientBase<TClient, TModel, TResult> : IServiceClient<TModel, TResult> // TODO переделать для масштабируемости
    where TClient : ClientBase
{
    protected ServiceClientBase(IChannelProvider channelProvider)
    {
        ChannelProvider = channelProvider;
    }

    private IChannelProvider ChannelProvider { get; }

    protected OsInfo.OsInfoClient Client { get; set; }

    public async Task<QueryResult<TResult>> GetAsync(string pcIp, TModel model)
    {
        var channel = ChannelProvider.AcquireChannel(pcIp);
        if (typeof(TClient) != typeof(OsInfo.OsInfoClient))
        {
            //throw new Exception(@$"Тип {typeof(TClient)} не добавлен в словарь клиентов.");
            throw new Exception(@$"Тип {typeof(TClient)} не {typeof(OsInfo.OsInfoClient)}.");
        }

        Client = new OsInfo.OsInfoClient(channel);
        var result = await GetAsync(model);
        ChannelProvider.ReleaseChannel(pcIp, channel);

        return result;
    }

    protected abstract Task<QueryResult<TResult>> GetAsync(TModel model);
}