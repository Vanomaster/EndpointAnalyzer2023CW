using AnalysisResultService;
using AnalysisService;
using BenchmarkService;
using CleanModels.Analysis;
using CleanModels.Benchmark;
using Common.Extensions;
using ConfigurationRecommendationsBenchmarkService;
using ConfigurationRecommendationsService;
using Google.Protobuf.WellKnownTypes;
using ScheduleService;
using TrustedHardwareBenchmarkService;
using TrustedHardwareService;
using TrustedSoftwareBenchmarkService;
using TrustedSoftwareService;
using AnalysisResult = CleanModels.Analysis.AnalysisResult;
using AnalysisScheduleRecordModel = CleanModels.Schedule.AnalysisScheduleRecord;
using BenchmarkModel = CleanModels.Benchmark.Benchmark;
using ConfigurationRecommendationsBenchmark = CleanModels.Benchmark.ConfigurationRecommendationsBenchmark;
using Host = CleanModels.Network.Host;
using NetworkProtoHost = NetworkService.ProtoHost;
using PageModel = CleanModels.PageModel;
using ProtoPageModel = CommonService.PageModel;
using TrustedHardware = CleanModels.Benchmark.TrustedHardware;
using TrustedHardwareBenchmark = CleanModels.Benchmark.TrustedHardwareBenchmark;
using TrustedSoftware = CleanModels.Benchmark.TrustedSoftware;
using TrustedSoftwareBenchmark = CleanModels.Benchmark.TrustedSoftwareBenchmark;

namespace Client;

public static class Mapper
{
    public static ProtoBenchmark? MapToProtoBenchmark(this BenchmarkModel? model)
    {
        if (model == null)
        {
            return null;
        }

        var resultModel = new ProtoBenchmark
        {
            Id = model.Id.ToByteString(),
            Name = model.Name,
            Description = model.Description,
        };

        return resultModel;
    }

    public static BenchmarkModel? MapToBenchmark(this ProtoBenchmark? model)
    {
        if (model == null)
        {
            return null;
        }

        var resultModel = new BenchmarkModel
        {
            Id = model.Id.ToGuid(),
            Name = model.Name,
            Description = model.Description,
        };


        return resultModel;
    }

    // public static ProtoBenchmark? MapToProtoBenchmark(this BenchmarkModel? model)
    // {
    //     if (model == null)
    //     {
    //         return null;
    //     }
    //
    //     var resultModel = new ProtoBenchmark
    //     {
    //         Id = model.Id.ToByteString(),
    //         Name = model.Name,
    //         Description = model.Description,
    //         ConfigurationRecommendationsBenchmark = model.ConfigurationRecommendationsBenchmark
    //             .MapToProtoConfigurationRecommendationsBenchmark(),
    //         TrustedSoftwareBenchmark = model.TrustedSoftwareBenchmark.MapToProtoTrustedSoftwareBenchmark(),
    //         TrustedHardwareBenchmark = model.TrustedHardwareBenchmark.MapToProtoTrustedHardwareBenchmark(),
    //     };
    //
    //     return resultModel;
    // }
    //
    // public static BenchmarkModel? MapToBenchmark(this ProtoBenchmark? model)
    // {
    //     if (model == null)
    //     {
    //         return null;
    //     }
    //
    //     var resultModel = new BenchmarkModel
    //     {
    //         Id = model.Id.ToGuid(),
    //         Name = model.Name,
    //         Description = model.Description,
    //         ConfigurationRecommendationsBenchmark = model.ConfigurationRecommendationsBenchmark
    //             .MapToConfigurationRecommendationsBenchmark(),
    //         TrustedSoftwareBenchmark = model.TrustedSoftwareBenchmark.MapToTrustedSoftwareBenchmark(),
    //         TrustedHardwareBenchmark = model.TrustedHardwareBenchmark.MapToTrustedHardwareBenchmark(),
    //     };
    //
    //
    //     return resultModel;
    // }

    public static ProtoPageModel? MapToProtoPageModel(this PageModel? model)
    {
        if (model == null)
        {
            return null;
        }

        var resultModel = new ProtoPageModel
        {
            PageNumber = model.PageNumber,
            SearchPhrase = model.SearchPhrase,
            SortingPropertyName = model.SortingPropertyName,
            SortingOrderIsAscending = model.SortingOrderIsAscending,
            ParentName = model.ParentName,
        };

        return resultModel;
    }

    public static ProtoAnalysisModel? MapToProtoAnalysisModel(this AnalysisModel? model)
    {
        if (model == null)
        {
            return null;
        }

        var resultModel = new ProtoAnalysisModel
        {
            PcIp = model.PcIp,
            BenchmarkName = model.BenchmarkName,
        };

        resultModel.AnalyzerNames.AddRange(model.AnalyzerNames);

        return resultModel;
    }

    public static ProtoAnalysisResult? MapToProtoAnalysisResult(this AnalysisResult? model)
    {
        if (model == null)
        {
            return null;
        }

        var resultModel = new ProtoAnalysisResult
        {
            Id = model.Id.ToByteString(),
            PcName = model.PcName,
            BenchmarkName = model.BenchmarkName,
            AnalyzerName = model.AnalyzerName,
            Text = model.Text.ToByteString(),
            DateTime = Timestamp.FromDateTimeOffset(model.DateTime),
        };

        return resultModel;
    }

    public static AnalysisResult? MapToAnalysisResult(this ProtoAnalysisResult? model)
    {
        if (model == null)
        {
            return null;
        }

        var resultModel = new AnalysisResult
        {
            Id = model.Id.ToGuid(),
            PcName = model.PcName,
            BenchmarkName = model.BenchmarkName,
            AnalyzerName = model.AnalyzerName,
            Text = model.Text.ToBytes(),
            DateTime = model.DateTime.ToDateTime().AddHours(3),
        };

        return resultModel;
    }

    public static Host? MapToHost(this NetworkProtoHost? model)
    {
        if (model == null)
        {
            return null;
        }

        var resultModel = new Host
        {
            Ip = model.Ip,
            Name = model.Name,
        };

        return resultModel;
    }

    public static ProtoAnalysisScheduleRecord? MapToProtoAnalysisScheduleRecord(this AnalysisScheduleRecordModel? model)
    {
        if (model == null)
        {
            return null;
        }

        var resultModel = new ProtoAnalysisScheduleRecord
        {
            Id = model.Id.ToByteString(),
            Name = model.Name,
            PcIp = model.Host.Ip,
            BenchmarkName = model.BenchmarkName,
            Recurrence = model.Recurrence,
            Enabled = model.Enabled,
        };

        resultModel.AnalyzerNames.AddRange(model.AnalyzerNames);

        return resultModel;
    }

    public static AnalysisScheduleRecordModel? MapToAnalysisScheduleRecord(this ProtoAnalysisScheduleRecord? model)
    {
        if (model == null)
        {
            return null;
        }

        var resultModel = new AnalysisScheduleRecordModel
        {
            Id = model.Id.ToGuid(),
            Name = model.Name,
            Host = new Host
            {
                Ip = model.Host.Ip,
                Name = model.Host.Name,
            },
            BenchmarkName = model.BenchmarkName,
            Recurrence = model.Recurrence,
            Enabled = model.Enabled,
        };

        resultModel.AnalyzerNames.AddRange(model.AnalyzerNames);

        return resultModel;
    }

    public static ProtoConfigurationRecommendationsBenchmark? MapToProtoConfigurationRecommendationsBenchmark(
        this ConfigurationRecommendationsBenchmark? model)
    {
        if (model == null)
        {
            return null;
        }

        var resultModel = new ProtoConfigurationRecommendationsBenchmark
        {
            Id = model.Id.ToByteString(),
            Name = model.Name,
            Description = model.Description,
            ParentId = model.ParentId?.ToByteString(),
        };

        // var protoConfigurationRecommendations = MapToProtoConfigurationRecommendations(model.ConfigurationRecommendations);
        // resultModel.ConfigurationRecommendations.AddRange(protoConfigurationRecommendations);

        return resultModel;
    }

    public static IEnumerable<ProtoConfigurationRecommendation> MapToProtoConfigurationRecommendations(this IEnumerable<ConfigurationRecommendation> model)
    {
        return model.Select(recommendation =>
            new ProtoConfigurationRecommendation
            {
                Id = recommendation.Id.ToByteString(),
                Name = recommendation.Name,
                VerificationCommand = recommendation.VerificationCommand,
                VerificationResult = recommendation.VerificationResult,
                Configuration = new ProtoConfiguration
                {
                    Id = recommendation.Configuration.Id.ToByteString(),
                    Name = recommendation.Configuration.Name,
                    Description = recommendation.Configuration.Description,
                },
                ParentId = recommendation.ParentId?.ToByteString(),
            });
    }

    public static ProtoTrustedSoftwareBenchmark? MapToProtoTrustedSoftwareBenchmark(
        this TrustedSoftwareBenchmark? model)
    {
        if (model == null)
        {
            return null;
        }

        var resultModel = new ProtoTrustedSoftwareBenchmark
        {
            Id = model.Id.ToByteString(),
            Name = model.Name,
            Description = model.Description,
            ParentId = model.ParentId?.ToByteString(),
        };

        // var protoTrustedSoftware = MapToProtoTrustedSoftware(model.TrustedSoftware);
        // resultModel.TrustedSoftware.AddRange(protoTrustedSoftware);

        return resultModel;
    }

    public static IEnumerable<ProtoTrustedSoftware> MapToProtoTrustedSoftware(this IEnumerable<TrustedSoftware> model)
    {
        return model.Select(software =>
            new ProtoTrustedSoftware
            {
                Id = software.Id.ToByteString(),
                Name = software.Name,
                Version = software.Version,
                ParentId = software.ParentId?.ToByteString(),
            });
    }

    public static ProtoTrustedHardwareBenchmark? MapToProtoTrustedHardwareBenchmark(
        this TrustedHardwareBenchmark? model)
    {
        if (model == null)
        {
            return null;
        }

        var resultModel = new ProtoTrustedHardwareBenchmark
        {
            Id = model.Id.ToByteString(),
            Name = model.Name,
            Description = model.Description,
            ParentId = model.ParentId?.ToByteString(),
        };

        // var protoTrustedHardware = MapToProtoTrustedHardware(model.TrustedHardware);
        // resultModel.TrustedHardware.AddRange(protoTrustedHardware);

        return resultModel;
    }

    public static IEnumerable<ProtoTrustedHardware> MapToProtoTrustedHardware(this IEnumerable<TrustedHardware> model)
    {
        return model.Select(hardware =>
            new ProtoTrustedHardware
            {
                Id = hardware.Id.ToByteString(),
                Name = hardware.Name,
                HardwareId = hardware.HardwareId,
                ParentId = hardware.ParentId?.ToByteString(),
            });
    }

    public static ConfigurationRecommendationsBenchmark? MapToConfigurationRecommendationsBenchmark(
        this ProtoConfigurationRecommendationsBenchmark? model)
    {
        if (model == null)
        {
            return null;
        }

        var resultModel = new ConfigurationRecommendationsBenchmark
        {
            Id = model.Id.ToGuid(),
            Name = model.Name,
            Description = model.Description,
            ParentId = model.ParentId?.ToGuid(),
            // ConfigurationRecommendations = MapToConfigurationRecommendations(model.ConfigurationRecommendations),
        };

        return resultModel;
    }

    public static IEnumerable<ConfigurationRecommendation> MapToConfigurationRecommendations(this IEnumerable<ProtoConfigurationRecommendation> model)
    {
        return model.Select(recommendation =>
            new ConfigurationRecommendation
            {
                Id = recommendation.Id.ToGuid(),
                Name = recommendation.Name,
                VerificationCommand = recommendation.VerificationCommand,
                VerificationResult = recommendation.VerificationResult,
                Configuration = new CleanModels.Benchmark.Configuration
                {
                    Id = recommendation.Configuration.Id.ToGuid(),
                    Name = recommendation.Configuration.Name,
                    Description = recommendation.Configuration.Description,
                },
            });
    }

    public static TrustedSoftwareBenchmark? MapToTrustedSoftwareBenchmark(this ProtoTrustedSoftwareBenchmark? model)
    {
        if (model == null)
        {
            return null;
        }

        var resultModel = new TrustedSoftwareBenchmark
        {
            Id = model.Id.ToGuid(),
            Name = model.Name,
            Description = model.Description,
            // TrustedSoftware = MapToTrustedSoftware(model.TrustedSoftware),
        };

        return resultModel;
    }

    public static IEnumerable<TrustedSoftware> MapToTrustedSoftware(this IEnumerable<ProtoTrustedSoftware> model)
    {
        return model.Select(software =>
            new TrustedSoftware
            {
                Id = software.Id.ToGuid(),
                Name = software.Name,
                Version = software.Version,
            });
    }

    public static TrustedHardwareBenchmark? MapToTrustedHardwareBenchmark(this ProtoTrustedHardwareBenchmark? model)
    {
        if (model == null)
        {
            return null;
        }

        var resultModel = new TrustedHardwareBenchmark
        {
            Id = model.Id.ToGuid(),
            Name = model.Name,
            Description = model.Description,
            // TrustedHardware = MapToTrustedHardware(model.TrustedHardware),
        };

        return resultModel;
    }

    public static IEnumerable<TrustedHardware> MapToTrustedHardware(this IEnumerable<ProtoTrustedHardware> model)
    {
        return model.Select(hardware =>
            new TrustedHardware
            {
                Id = hardware.Id.ToGuid(),
                Name = hardware.Name,
                HardwareId = hardware.HardwareId,
            });
    }
}