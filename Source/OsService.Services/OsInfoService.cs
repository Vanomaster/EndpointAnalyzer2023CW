using CleanModels;
using CleanModels.Queries.Base;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using OsService.Services.Base;

namespace OsService.Services;

public class OsInfoService : IInfoService<string>
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// Initializes a new instance of the <see cref="OsInfoService"/> class.
    /// </summary>
    /// <param name="serviceProvider">Service provider.</param>
    public OsInfoService(IServiceProvider serviceProvider)
    {
        Logger.Debug("Creating OsInfoService.");
        ServiceProvider = serviceProvider;
    }

    private IServiceProvider ServiceProvider { get; }

    public async Task<QueryResult<string>> GetAsync<TQuery>()
        where TQuery : IQuery<string>
    {
        Logger.Info("OsInfoService Get started.");
        using var scope = ServiceProvider.CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<TQuery>();
        var queryResult = await query.ExecuteAsync();
        if (!queryResult.IsSuccessful)
        {
            Logger.Error($"OsInfoService Get failed. {queryResult.ErrorMessage}");

            return new QueryResult<string>(queryResult.ErrorMessage);
        }

        Logger.Info("OsInfoService Get successfully completed.");

        return new QueryResult<string>(data: queryResult.Data);
    }

    public async Task<QueryResult<List<ConfigurationVerification>>> GetAsync<TQuery>(List<string> verificationCommands)
        where TQuery : IQuery<List<string>, List<ConfigurationVerification>>
    {
        Logger.Info("OsInfoService Get started.");
        using var scope = ServiceProvider.CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<TQuery>();
        var queryResult = await query.ExecuteAsync(verificationCommands);
        if (!queryResult.IsSuccessful)
        {
            Logger.Error($"OsInfoService Get failed. {queryResult.ErrorMessage}");

            return new QueryResult<List<ConfigurationVerification>>(queryResult.ErrorMessage);
        }

        Logger.Info("OsInfoService Get successfully completed.");

        return new QueryResult<List<ConfigurationVerification>>(data: queryResult.Data);
    }
}