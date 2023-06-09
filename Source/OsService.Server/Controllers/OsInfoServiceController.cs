using CleanModels.Queries.Base;
using Grpc.Core;
using NLog;
using OsInfoService;
using OsService.Queries;
using OsService.Services.Base;
using static OsInfoService.OsInfo;

namespace OsService.Server.Controllers;

/// <inheritdoc />
public class OsInfoServiceController : OsInfoBase
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// Initializes a new instance of the <see cref="OsInfoServiceController"/> class.
    /// </summary>
    /// <param name="osInfoService">Benchmark service.</param>
    public OsInfoServiceController(IInfoService<string> osInfoService)
    {
        Logger.Debug("Creating OsInfoServiceController.");
        OsInfoService = osInfoService;
    }

    private IInfoService<string> OsInfoService { get; }

    /// <inheritdoc/>
    public override async Task<GetConfigurationResponse> GetConfiguration(GetConfigurationRequest model, ServerCallContext context)
    {
        var response = await GetAsync<ConfigurationQuery>(model);

        return response;
    }

    /// <inheritdoc/>
    public override async Task<GetResponse> GetInstalledSoftware(GetRequest model, ServerCallContext context)
    {
        var response = await GetAsync<InstalledSoftwareQuery>();

        return response;
    }

    /// <inheritdoc/>
    public override async Task<GetResponse> GetUpgradableSoftware(GetRequest model, ServerCallContext context)
    {
        var response = await GetAsync<UpgradableSoftwareQuery>();

        return response;
    }

    /// <inheritdoc/>
    public override async Task<GetResponse> GetUsbHardware(GetRequest model, ServerCallContext context)
    {
        var response = await GetAsync<UsbHardwareQuery>();

        return response;
    }

    /// <inheritdoc/>
    public override async Task<GetResponse> GetHostName(GetRequest model, ServerCallContext context)
    {
        var response = await GetAsync<HostNameQuery>();

        return response;
    }

    private async Task<GetResponse> GetAsync<TQuery>()
        where TQuery : CommandLineQueryBase<string>
    {
        var queryResult = await OsInfoService.GetAsync<TQuery>();
        if (!queryResult.IsSuccessful)
        {
            return new GetResponse { ErrorMessage = queryResult.ErrorMessage };
        }

        return new GetResponse { Data = new Data { Data_ = queryResult.Data } };
    }

    private async Task<GetConfigurationResponse> GetAsync<TQuery>(GetConfigurationRequest model)
        where TQuery : CommandLineQueryBase<List<string>, List<CleanModels.ConfigurationVerification>>
    {
        var queryResult = await OsInfoService.GetAsync<TQuery>(model.VerificationCommands.ToList());
        if (!queryResult.IsSuccessful)
        {
            return new GetConfigurationResponse { ErrorMessage = queryResult.ErrorMessage };
        }

        var data = new ConfigurationData();
        var configurationVerifications = queryResult.Data.Select(c => c.MapToProtoConfigurationVerification()).ToList();
        data.ConfigurationVerifications.AddRange(configurationVerifications);

        return new GetConfigurationResponse { Data = data };
    }
}