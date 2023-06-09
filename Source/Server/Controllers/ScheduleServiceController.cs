using CommonService;
using Grpc.Core;
using ScheduleService;
using Server.Services.Base;
using AnalysisScheduleRecordModel = CleanModels.Schedule.AnalysisScheduleRecord;

namespace Server.Controllers;

/// <inheritdoc />
public class ScheduleServiceController : Schedule.ScheduleBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ScheduleServiceController"/> class.
    /// </summary>
    /// <param name="scheduleService">Benchmark service.</param>
    public ScheduleServiceController(IScheduleService<AnalysisScheduleRecordModel> scheduleService)
    {
        ScheduleService = scheduleService;
    }

    private IScheduleService<AnalysisScheduleRecordModel> ScheduleService { get; }

    /// <inheritdoc/>
    public override async Task<GetResponse> Get(GetRequest model, ServerCallContext context)
    {
        var response = await GetAsync(model);

        return response;
    }

    /// <inheritdoc/>
    public override async Task<CommandResponse> AddOrUpdate(AddOrUpdateRequest model, ServerCallContext context)
    {
        var response = await AddOrUpdateAsync(model);

        return response;
    }

    /// <inheritdoc/>
    public override async Task<CommandResponse> Run(RunRequest model, ServerCallContext context)
    {
        var response = await RunAsync(model);

        return response;
    }

    /// <inheritdoc/>
    public override async Task<CommandResponse> Remove(RemoveRequest model, ServerCallContext context)
    {
        var response = await RemoveAsync(model);

        return response;
    }

    private async Task<GetResponse> GetAsync(GetRequest model)
    {
        var queryResult = await ScheduleService.GetAsync(model.PageModel.MapToPageModel());
        if (!queryResult.IsSuccessful)
        {
            return new GetResponse { ErrorMessage = queryResult.ErrorMessage };
        }

        var protoAnalysisScheduleRecords = queryResult.Data.Select(record => record.MapToProtoAnalysisScheduleRecord());
        var data = new Data();
        data.AnalysisScheduleRecords.AddRange(protoAnalysisScheduleRecords);

        return new GetResponse { Data = data };
    }

    private async Task<CommandResponse> AddOrUpdateAsync(AddOrUpdateRequest model)
    {
        var analysisScheduleRecord = model.AnalysisScheduleRecord.MapToAnalysisScheduleRecord();
        if (analysisScheduleRecord == null)
        {
            return new CommandResponse { ErrorMessage = "AnalysisScheduleRecord is null." };
        }

        var queryResult = await ScheduleService.AddOrUpdateAsync(analysisScheduleRecord);
        if (!queryResult.IsSuccessful)
        {
            return new CommandResponse { ErrorMessage = queryResult.ErrorMessage };
        }

        return new CommandResponse();
    }

    private async Task<CommandResponse> RunAsync(RunRequest model)
    {
        var queryResult = await ScheduleService.RunAsync(model.Name);
        if (!queryResult.IsSuccessful)
        {
            return new CommandResponse { ErrorMessage = queryResult.ErrorMessage };
        }

        return new CommandResponse();
    }

    private async Task<CommandResponse> RemoveAsync(RemoveRequest model)
    {
        var benchmarks = model.AnalysisScheduleRecords.Select(b => b.MapToAnalysisScheduleRecord()).ToList();
        var queryResult = await ScheduleService.RemoveAsync(benchmarks);
        if (!queryResult.IsSuccessful)
        {
            return new CommandResponse { ErrorMessage = queryResult.ErrorMessage };
        }

        return new CommandResponse();
    }
}