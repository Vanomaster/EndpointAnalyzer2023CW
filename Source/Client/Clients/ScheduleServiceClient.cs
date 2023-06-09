using CleanModels;
using CleanModels.Commands.Base;
using CleanModels.Queries.Base;
using Client.Clients.Base;
using ScheduleService;
using static ScheduleService.Schedule;
using AnalysisScheduleRecordModel = CleanModels.Schedule.AnalysisScheduleRecord;

namespace Client.Clients;

/// <inheritdoc />
public class ScheduleServiceClient : ServiceClientBase<ScheduleClient>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ScheduleServiceClient"/> class.
    /// </summary>
    /// <param name="client">Client.</param>
    public ScheduleServiceClient(ScheduleClient client)
         : base(client)
    {
    }

    public async Task<QueryResult<List<AnalysisScheduleRecordModel>>> GetAsync(PageModel pageModel)
    {
        var request = new GetRequest { PageModel = pageModel.MapToProtoPageModel() };
        var response = await Client.GetAsync(request);
        if (!string.IsNullOrEmpty(response.ErrorMessage))
        {
            return new QueryResult<List<AnalysisScheduleRecordModel>>(response.ErrorMessage);
        }

        var analysisScheduleRecords = response.Data.AnalysisScheduleRecords
            .Select(r => r.MapToAnalysisScheduleRecord())
            .ToList();

        return new QueryResult<List<AnalysisScheduleRecordModel>>(data: analysisScheduleRecords);
    }

    public async Task<CommandResult> AddOrUpdateAsync(AnalysisScheduleRecordModel analysisScheduleRecord)
    {
        var request = new AddOrUpdateRequest
            { AnalysisScheduleRecord = analysisScheduleRecord.MapToProtoAnalysisScheduleRecord() };

        var response = await Client.AddOrUpdateAsync(request);
        if (!string.IsNullOrEmpty(response.ErrorMessage))
        {
            return new CommandResult(response.ErrorMessage);
        }

        return new CommandResult();
    }

    public async Task<CommandResult> RunAsync(string name)
    {
        var request = new RunRequest { Name = name };
        var response = await Client.RunAsync(request);
        if (!string.IsNullOrEmpty(response.ErrorMessage))
        {
            return new CommandResult(response.ErrorMessage);
        }

        return new CommandResult();
    }

    public async Task<CommandResult> RemoveAsync(IEnumerable<AnalysisScheduleRecordModel> analysisScheduleRecord)
    {
        var request = new RemoveRequest();
        request.AnalysisScheduleRecords.AddRange(analysisScheduleRecord.Select(r => r.MapToProtoAnalysisScheduleRecord()));
        var response = await Client.RemoveAsync(request);
        if (!string.IsNullOrEmpty(response.ErrorMessage))
        {
            return new CommandResult(response.ErrorMessage);
        }

        return new CommandResult();
    }
}