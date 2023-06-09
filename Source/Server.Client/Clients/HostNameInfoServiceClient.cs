using CleanModels.Queries.Base;
using OsInfoService;
using Server.Client.Clients.Base;
using static OsInfoService.OsInfo;

namespace Server.Client.Clients;

/// <inheritdoc />
public class HostNameInfoServiceClient : ServiceClientBase<OsInfoClient, string>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UsbHardwareInfoServiceClient"/> class.
    /// </summary>
    /// <param name="channelProvider">Channel provider.</param>
    public HostNameInfoServiceClient(IChannelProvider channelProvider)
        : base(channelProvider)
    {
    }

    /// <inheritdoc/>
    protected override async Task<QueryResult<string>> GetAsync()
    {
        var request = new GetRequest();
        var response = await Client.GetHostNameAsync(request);
        if (!string.IsNullOrEmpty(response.ErrorMessage))
        {
            return new QueryResult<string>(response.ErrorMessage);
        }

        string data = response.Data.Data_;

        return new QueryResult<string>(data: data);
    }
}