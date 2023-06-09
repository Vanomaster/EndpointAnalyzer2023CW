using CleanModels.Queries.Base;
using OsInfoService;
using Server.Client.Clients.Base;
using static OsInfoService.OsInfo;

namespace Server.Client.Clients;

/// <inheritdoc />
public class UpgradableSoftwareInfoServiceClient : ServiceClientBase<OsInfoClient, string>, IUpgradableSoftwareInfoServiceClient
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpgradableSoftwareInfoServiceClient"/> class.
    /// </summary>
    /// <param name="channelProvider">Channel provider.</param>
    public UpgradableSoftwareInfoServiceClient(IChannelProvider channelProvider)
        : base(channelProvider)
    {
    }

    /// <inheritdoc/>
    protected override async Task<QueryResult<string>> GetAsync()
    {
        var request = new GetRequest();
        var response = await Client.GetUpgradableSoftwareAsync(request);
        if (!string.IsNullOrEmpty(response.ErrorMessage))
        {
            return new QueryResult<string>(response.ErrorMessage);
        }

        string data = response.Data.Data_;

        return new QueryResult<string>(data: data);
    }
}