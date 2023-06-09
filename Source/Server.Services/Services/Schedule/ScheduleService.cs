using CleanModels;
using CleanModels.Analysis;
using CleanModels.Commands.Base;
using CleanModels.Queries.Base;
using Dal.Commands;
using Dal.Queries;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using Server.Client;
using Server.Services.Base;
using AnalysisScheduleRecordEntity = Dal.Entities.AnalysisScheduleRecord;
using AnalysisScheduleRecordModel = CleanModels.Schedule.AnalysisScheduleRecord;

namespace Server.Services;

public class ScheduleService : IScheduleService<AnalysisScheduleRecordModel>
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// Initializes a new instance of the <see cref="ScheduleService"/> class.
    /// </summary>
    /// <param name="serviceProvider">Service provider.</param>
    public ScheduleService(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    private IServiceProvider ServiceProvider { get; }

    /// <inheritdoc/>
    public async Task<QueryResult<List<AnalysisScheduleRecordModel>>> GetAsync(PageModel pageModel)
    {
        Logger.Info("ScheduleService Get started.");
        var analysisScheduleRecords = new List<AnalysisScheduleRecordModel>();
        using var scope = ServiceProvider.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<AnalysisScheduleRecordQuery>();
        var queryResult = await service.ExecuteAsync(pageModel);
        if (!queryResult.IsSuccessful)
        {
            Logger.Error($"ScheduleService Get failed. {queryResult.ErrorMessage}");

            return new QueryResult<List<AnalysisScheduleRecordModel>>(queryResult.ErrorMessage);
        }

        foreach (var analysisScheduleRecord in queryResult.Data)
        {
            var networkService = scope.ServiceProvider.GetRequiredService<INetworkService>();
            var hostQueryResult = networkService.GetHostWithEaService(analysisScheduleRecord.PcIp);
            if (!hostQueryResult.IsSuccessful)
            {
                Logger.Error($"ScheduleService Get failed. {hostQueryResult.ErrorMessage}");

                return new QueryResult<List<AnalysisScheduleRecordModel>>(hostQueryResult.ErrorMessage);
            }

            var scheduleRecord = analysisScheduleRecord.MapToAnalysisScheduleRecordModel(hostQueryResult.Data.Name);
            analysisScheduleRecords.Add(scheduleRecord);
        }

        Logger.Info("ScheduleService Get successfully completed.");

        return new QueryResult<List<AnalysisScheduleRecordModel>>(analysisScheduleRecords);
    }

    /// <inheritdoc/>
    public async Task<CommandResult> AddOrUpdateAsync(AnalysisScheduleRecordModel model)
    {
        Logger.Info("ScheduleService AddOrUpdate started.");
        var validatingResult = Validator.ValidateAnalyzerNames(model.AnalyzerNames);
        if (!validatingResult.IsSuccessful)
        {
            Logger.Error($"ScheduleService AddOrUpdate failed. {validatingResult.ErrorMessage}");

            return new CommandResult(validatingResult.ErrorMessage);
        }

        using var scope = ServiceProvider.CreateScope();
        var addOrUpdateUniqueEntityCommand =
            scope.ServiceProvider.GetRequiredService<AddOrUpdateEntityCommand<AnalysisScheduleRecordEntity>>();

        var analysisScheduleRecords = new[] { model.MapToAnalysisScheduleRecordEntity() };
        var commandResult = await addOrUpdateUniqueEntityCommand.ExecuteAsync(analysisScheduleRecords);
        if (!commandResult.IsSuccessful)
        {
            Logger.Error($"ScheduleService AddOrUpdate failed. {commandResult.ErrorMessage}");

            return new CommandResult(commandResult.ErrorMessage);
        }

        var enableCommandResult = await EnableAsync(model);
        if (!enableCommandResult.IsSuccessful)
        {
            Logger.Error($"ScheduleService AddOrUpdate failed. {commandResult.ErrorMessage}");

            return new CommandResult(commandResult.ErrorMessage);
        }

        Logger.Info("ScheduleService AddOrUpdate successfully completed.");

        return new CommandResult();
    }

    public async Task<CommandResult> RunAsync(string name)
    {
        Logger.Info("ScheduleService Run started.");
        //RecurringJob.TriggerJob(name);
        Logger.Info("ScheduleService Run successfully completed.");

        return new CommandResult();
    }

    public async Task<CommandResult> DisableAsync(AnalysisScheduleRecordModel model)
    {
        Logger.Info("ScheduleService Run started.");
        RecurringJob.RemoveIfExists(model.Name);
        // var commandResult = await RemoveUnusedChannelsAsync(model.Host.Ip);
        // if (!commandResult.IsSuccessful)
        // {
        //     Logger.Error($"ScheduleService AddOrUpdate failed. {commandResult.ErrorMessage}");
        //
        //     return new CommandResult(commandResult.ErrorMessage);
        // }

        Logger.Info("ScheduleService Run successfully completed.");

        return new CommandResult();
    }

    public async Task<CommandResult> EnableAsync(AnalysisScheduleRecordModel model)
    {
        Logger.Info("ScheduleService Enable started.");
        AddOrUpdateRecurringAnalysis(model);
        using var scope = ServiceProvider.CreateScope();
        // var channelProvider = scope.ServiceProvider.GetRequiredService<IChannelProvider>();
        // channelProvider.InitChannelsForIp(model.Host.Ip);
        Logger.Info("ScheduleService Enable successfully completed.");

        return new CommandResult();
    }

    public async Task<CommandResult> RemoveAsync(IEnumerable<AnalysisScheduleRecordModel> model)
    {
        Logger.Info("ScheduleService Remove started.");
        using var scope = ServiceProvider.CreateScope();
        var removeCommand = scope.ServiceProvider.GetRequiredService<RemoveEntitiesCommand<AnalysisScheduleRecordEntity>>();
        var commandResult = await removeCommand.ExecuteAsync(model.Select(r => r.MapToAnalysisScheduleRecordEntity()));
        if (!commandResult.IsSuccessful)
        {
            Logger.Error($"ScheduleService Remove failed. {commandResult.ErrorMessage}");

            return new CommandResult(commandResult.ErrorMessage);
        }

        foreach (var record in model)
        {
            RecurringJob.RemoveIfExists(record.Name);
        }
        // var removeUnusedChannelsCommandResult = await RemoveUnusedChannelsAsync(model.Host.Ip);
        // if (!removeUnusedChannelsCommandResult.IsSuccessful)
        // {
        //     Logger.Error($"ScheduleService Remove failed. {commandResult.ErrorMessage}");
        //
        //     return new CommandResult(removeUnusedChannelsCommandResult.ErrorMessage);
        // }

        Logger.Info("ScheduleService Remove successfully completed.");

        return new CommandResult();
    }

    private static void AddOrUpdateRecurringAnalysis(AnalysisScheduleRecordModel model)
    {
        var analysisModel = new AnalysisModel
        {
            PcIp = model.Host.Ip,
            BenchmarkName = model.BenchmarkName,
            AnalyzerNames = model.AnalyzerNames,
        };

        RecurringJob.AddOrUpdate<AnalysisService>(
            model.Name,
            analysisService => analysisService.AnalyzeAsync(analysisModel),
            model.Recurrence,
            TimeZoneInfo.Local
            // new RecurringJobOptions
            // {
            //     TimeZone = TimeZoneInfo.Local,
            //     //MisfireHandling = MisfireHandlingMode.Ignorable,}
            );
    }

    // private async Task<CommandResult> RemoveUnusedChannelsAsync(string pcIp)
    // {
    //     Logger.Info("ScheduleService RemoveUnusedChannels started.");
    //     using var scope = ServiceProvider.CreateScope();
    //     var analysisScheduleRecordByPcNameAnyQuery = scope.ServiceProvider
    //         .GetRequiredService<AnalysisScheduleRecordByPcIpAnyQuery>();
    //
    //     var queryResult = await analysisScheduleRecordByPcNameAnyQuery.ExecuteAsync(pcIp);
    //     if (!queryResult.IsSuccessful)
    //     {
    //         Logger.Error($"ScheduleService RemoveUnusedChannels failed. {queryResult.ErrorMessage}");
    //
    //         return new CommandResult(queryResult.ErrorMessage);
    //     }
    //
    //     bool analysisScheduleRecordWithPcNameExist = queryResult.Data;
    //     if (!analysisScheduleRecordWithPcNameExist)
    //     {
    //         var channelProvider = scope.ServiceProvider.GetRequiredService<IChannelProvider>();
    //         channelProvider.RemoveChannelsForIp(pcIp);
    //     }
    //
    //     Logger.Info("ScheduleService RemoveUnusedChannels successfully completed.");
    //
    //     return new CommandResult();
    // }

    // public async Task<CommandResult> GetStatusAsync(AnalysisScheduleRecord model)
    // {
    //     const string jobId = "jobId";
    //     Logger.Info("ScheduleService Run started.");
    //     var monitoringApi = JobStorage.Current.GetMonitoringApi();
    //     var jobDetails = monitoringApi.JobDetails(jobId);
    //     string currentState = jobDetails.History[0].StateName;
    //     Logger.Info("ScheduleService Run successfully completed.");
    //
    //     return new CommandResult();
    // }
    //
    // public async Task<CommandResult> GetJobResultAsync(AnalysisScheduleRecord model)
    // {
    //     const string jobId = "jobId";
    //     Logger.Info("ScheduleService Run started.");
    //     var jobMonitoringApi = JobStorage.Current.GetMonitoringApi();
    //     var job = jobMonitoringApi.JobDetails(jobId);
    //     string resultSerialized = job.History[0].Data["Result"];
    //     List<TReturn> returnedItems = JsonConvert.DeserializeObject<List<TReturn>>(resultSerialized);
    //     Logger.Info("ScheduleService Run successfully completed.");
    //
    //     return new CommandResult();
    // }
}