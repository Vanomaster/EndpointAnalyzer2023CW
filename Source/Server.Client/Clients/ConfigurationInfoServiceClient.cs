using CleanModels.Queries.Base;
using OsInfoService;
using Server.Client.Clients.Base;
using static OsInfoService.OsInfo;
using ConfigurationVerification = CleanModels.ConfigurationVerification;

namespace Server.Client.Clients;

/// <inheritdoc /> // or OsInfoServiceClient
public class ConfigurationInfoServiceClient :
    ServiceClientBase<OsInfoClient, List<string>, List<ConfigurationVerification>>, IConfigurationInfoServiceClient
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationInfoServiceClient"/> class.
    /// </summary>
    /// <param name="channelProvider">Channel provider.</param>
    public ConfigurationInfoServiceClient(IChannelProvider channelProvider)
        : base(channelProvider)
    {
    }

    /// <inheritdoc/>
    protected override async Task<QueryResult<List<ConfigurationVerification>>> GetAsync(
        List<string> verificationCommands)
    {
        var request = new GetConfigurationRequest();
        request.VerificationCommands.AddRange(verificationCommands);
        var response = await Client.GetConfigurationAsync(request);
        if (!string.IsNullOrEmpty(response.ErrorMessage))
        {
            return new QueryResult<List<ConfigurationVerification>>(response.ErrorMessage);
        }

        List<ConfigurationVerification> data =
            response.Data.ConfigurationVerifications.Select(v => v.MapToConfigurationVerification()).ToList();

        return new QueryResult<List<ConfigurationVerification>>(data: data);
    }
}