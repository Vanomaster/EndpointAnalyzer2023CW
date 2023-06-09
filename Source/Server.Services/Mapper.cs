using CleanModels.Network;
using AnalysisScheduleRecordEntity = Dal.Entities.AnalysisScheduleRecord;
using AnalysisScheduleRecordModel = CleanModels.Schedule.AnalysisScheduleRecord;
using BenchmarkEntity = Dal.Entities.Benchmark;
using BenchmarkModel = CleanModels.Benchmark.Benchmark;

namespace Server.Services;

public static class Mapper
{
    private const string AnalyzerNamesSeparator = " | ";

    public static AnalysisScheduleRecordEntity MapToAnalysisScheduleRecordEntity(this AnalysisScheduleRecordModel model)
    {
        var resultModel = new AnalysisScheduleRecordEntity
        {
            Id = model.Id,
            Name = model.Name,
            PcIp = model.Host.Ip,
            BenchmarkName = model.BenchmarkName,
            Recurrence = model.Recurrence,
            Enabled = model.Enabled,
        };

        for (var i = 0; i < model.AnalyzerNames.Count; i++)
        {
            resultModel.AnalyzerNames += model.AnalyzerNames[i];
            if (i != model.AnalyzerNames.Count - 1)
            {
                resultModel.AnalyzerNames += AnalyzerNamesSeparator;
            }
        }

        return resultModel;
    }

    public static AnalysisScheduleRecordModel MapToAnalysisScheduleRecordModel(
        this AnalysisScheduleRecordEntity model,
        string pcName)
    {
        var resultModel = new AnalysisScheduleRecordModel
        {
            Id = model.Id,
            Name = model.Name,
            Host = new Host
            {
                Ip = model.PcIp,
                Name = pcName,
            },
            BenchmarkName = model.BenchmarkName,
            Recurrence = model.Recurrence,
            Enabled = model.Enabled,
        };

        string[] names = model.AnalyzerNames.Split(AnalyzerNamesSeparator);
        foreach (string name in names)
        {
            resultModel.AnalyzerNames.Add(name);
        }

        return resultModel;
    }

    public static BenchmarkEntity MapToBenchmarkEntity(this BenchmarkModel model)
    {
        var resultModel = new BenchmarkEntity
        {
            Id = model.Id,
            Name = model.Name,
            Description = model.Description,
        };

        return resultModel;
    }

    public static BenchmarkModel MapToBenchmarkModel(this BenchmarkEntity model)
    {
        var resultModel = new BenchmarkModel
        {
            Id = model.Id,
            Name = model.Name,
            Description = model.Description,
        };

        return resultModel;
    }
}