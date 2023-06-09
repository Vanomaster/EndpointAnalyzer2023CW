// using CleanModels.Commands.Base;
// using Dal.Commands.Base;
// using Dal.Entities;
// using Microsoft.EntityFrameworkCore;
//
// namespace Dal.Commands;
//
// /// <inheritdoc />
// public class AddConfigurationRecommendationCommand : DbCommandBase<List<ConfigurationRecommendation>>
// {
//     /// <summary>
//     /// Initializes a new instance of the <see cref="AddConfigurationRecommendationCommand"/> class.
//     /// </summary>
//     /// <param name="contextFactory">Context factory.</param>
//     public AddConfigurationRecommendationCommand(IDbContextFactory<Context> contextFactory)
//         : base(contextFactory)
//     {
//     }
//
//     /// <inheritdoc/>
//     protected override async Task<CommandResult> ExecuteCoreAsync(List<ConfigurationRecommendation> entitiesToAdd)
//     {
//         var configurations = Context.Configurations.AsNoTracking();
//         var existedConfigurations = await configurations
//             .Where(entity => entitiesToAdd.Select(entityToAdd => entityToAdd.Configuration.Name).Contains(entity.Name))
//             .Include(entity => entity.ConfigurationRecommendations)
//             .ToListAsync();
//
//         var existRecNames = new List<string>();
//         foreach (var configuration in existedConfigurations)
//         {
//             var recommendations = entitiesToAdd
//                 .Where(entity => entity.Configuration.Name == configuration.Name)
//                 .ToList();
//
//             var existRec = recommendations
//                 .Where(r => configuration.ConfigurationRecommendations.Select(rec => rec.Name).Contains(r.Name) ||
//                             (configuration.ConfigurationRecommendations.Select(rec => rec.VerificationResult)
//                                  .Contains(r.VerificationResult) &&
//                             configuration.Name == r.Name))
//                 .Select(r => r.Name)
//                 .ToList();
//
//             if (existRec.Any())
//             {
//                 existRecNames.AddRange(existRec);
//
//                 continue;
//             }
//
//             configuration.ConfigurationRecommendations.AddRange(recommendations);
//         }
//
//         if (existRecNames.Any())
//         {
//             if (entitiesToAdd.Count == 1)
//             {
//                 return GetFailedResult(@"Рекомендация конфигурации с таким названием или " +
//                                        @"названием конфигурации и результатом команды уже существует " +
//                                        @"в существующей конфигурации.");
//             }
//
//             if (entitiesToAdd.Count > 1)
//             {
//                 string names = string.Join(',', existRecNames);
//                 string errorMessage = @"Рекомендации конфигураций с таким названием или " +
//                                       @"названием конфигурации и результатом команды уже существуют " +
//                                       @$"в существующих конфигурациях: {names}";
//
//                 return GetFailedResult(errorMessage);
//             }
//         }
//
//         var recsWithNewConfig = entitiesToAdd
//             .ExceptBy(
//                 entitiesToAdd
//                     .Where(e => existedConfigurations.Select(c => c.Name).Contains(e.Configuration.Name))
//                     .Select(e => e.Name)
//                     .ToList(),
//                 recommendation => recommendation.Name)
//             .ToList();
//
//         var entitiesToFetch = Context.ConfigurationRecommendations.AsNoTracking();
//         var existEntityNames = await entitiesToFetch
//             .Where(entity =>
//                 recsWithNewConfig.Select(entityToAdd => entityToAdd.Name).Contains(entity.Name) ||
//                 (recsWithNewConfig.Select(entityToAdd => entityToAdd.VerificationResult)
//                      .Contains(entity.VerificationResult) &&
//                  recsWithNewConfig.Select(entityToAdd => entityToAdd.Configuration.Name)
//                      .Contains(entity.Configuration.Name)))
//             .Select(entity => entity.Name)
//             .ToListAsync();
//
//         if (existEntityNames.Any())
//         {
//             if (entitiesToAdd.Count == 1)
//             {
//                 return GetFailedResult(@"Рекомендация конфигурации с таким названием или " +
//                                        @"названием конфигурации и результатом команды уже существует.");
//             }
//
//             if (entitiesToAdd.Count > 1)
//             {
//                 string names = string.Join(',', existEntityNames);
//                 string errorMessage = @"Рекомендации конфигураций с таким названием или " +
//                                       @$"названием конфигурации и результатом команды уже существуют: {names}";
//
//                 return GetFailedResult(errorMessage);
//             }
//         }
//
//         var parent = await Context.ConfigurationRecommendationBenchmarks.FirstOrDefaultAsync(b => b.Id == entitiesToAdd[0].ConfigurationRecommendationsBenchmarks[0].Id);
//         entitiesToAdd.ForEach(e => e.ConfigurationRecommendationsBenchmarks.Clear());
//         await Context.AddRangeAsync(entitiesToAdd);
//         parent.ConfigurationRecommendations.AddRange(entitiesToAdd);
//         await Context.SaveChangesAsync();
//
//         return GetSuccessfulResult();
//     }
// }