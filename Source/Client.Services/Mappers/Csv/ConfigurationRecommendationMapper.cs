using CleanModels.Benchmark;
using CsvHelper.Configuration;

namespace Client.Services.Mappers.Csv;

public class ConfigurationRecommendationMapper : ClassMap<ConfigurationRecommendation>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationRecommendationMapper"/> class.
    /// </summary>
    public ConfigurationRecommendationMapper()
    {
        Map(recommendation => recommendation.Configuration.Name).Name(@"Название конфигурации");
        Map(recommendation => recommendation.Configuration.Description).Name(@"Описание конфигурации");
        Map(recommendation => recommendation.Name).Name(@"Название рекомендации");
        Map(recommendation => recommendation.VerificationCommand).Name(@"Команда");
        Map(recommendation => recommendation.VerificationResult).Name(@"Результат команды");
    }
}