// using CleanModels;
// using CleanModels.Commands.Base;
// using CleanModels.Queries.Base;
// using Dal.Commands;
// using Dal.Queries;
// using Microsoft.Extensions.DependencyInjection;
// using NLog;
// using Server.Services.Base;
// using Benchmark = Dal.Entities.TrustedHardwareBenchmark;
//
// namespace Server.Services;
//
// public class TrustedHardwareBenchmarkService : IDbService<Benchmark>
// {
//     private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
//
//     /// <summary>
//     /// Initializes a new instance of the <see cref="TrustedHardwareBenchmarkService"/> class.
//     /// </summary>
//     /// <param name="serviceProvider">Service provider.</param>
//     public TrustedHardwareBenchmarkService(IServiceProvider serviceProvider)
//     {
//         ServiceProvider = serviceProvider;
//     }
//
//     private IServiceProvider ServiceProvider { get; }
//
//     /// <inheritdoc/>
//     public async Task<QueryResult<List<Benchmark>>> GetAsync(PageModel pageModel)
//     {
//         Logger.Info("BenchmarkService Get started.");
//         using var scope = ServiceProvider.CreateScope();
//         var benchmarkQuery = scope.ServiceProvider.GetRequiredService<BenchmarkQuery>();
//         var queryResult = await benchmarkQuery.ExecuteAsync(pageModel);
//         if (!queryResult.IsSuccessful)
//         {
//             Logger.Error($"BenchmarkService Get failed. {queryResult.ErrorMessage}");
//
//             return new QueryResult<List<Benchmark>>(queryResult.ErrorMessage);
//         }
//
//         Logger.Info("BenchmarkService Get successfully completed.");
//
//         return new QueryResult<List<Benchmark>>(queryResult.Data);
//     }
//
//     /// <inheritdoc/>
//     public Task<CommandResult> AddOrUpdateAsync(Benchmark model)
//     {
//         throw new NotImplementedException();
//     }
//
//     public async Task<CommandResult> AddAsync(Benchmark model)
//     {
//         Logger.Info("BenchmarkService Add started.");
//         using var scope = ServiceProvider.CreateScope();
//         var addCommand = scope.ServiceProvider.GetRequiredService<AddBenchmarkCommand<Benchmark>>();
//         var commandResult = await addCommand.ExecuteAsync(model);
//         if (!commandResult.IsSuccessful)
//         {
//             Logger.Error($"BenchmarkService Add failed. {commandResult.ErrorMessage}");
//
//             return new CommandResult(commandResult.ErrorMessage);
//         }
//
//         Logger.Info("BenchmarkService Add successfully completed.");
//
//         return new CommandResult();
//     }
//
//     public async Task<CommandResult> UpdateAsync(Benchmark model)
//     {
//         Logger.Info("BenchmarkService Update started.");
//         using var scope = ServiceProvider.CreateScope();
//         var command = scope.ServiceProvider.GetRequiredService<UpdateNamedEntityCommand<Benchmark>>();
//         var commandResult = await command.ExecuteAsync(new List<Benchmark> { model });
//         if (!commandResult.IsSuccessful)
//         {
//             Logger.Error($"BenchmarkService Update failed. {commandResult.ErrorMessage}");
//
//             return new CommandResult(commandResult.ErrorMessage);
//         }
//
//         Logger.Info("BenchmarkService Update successfully completed.");
//
//         return new CommandResult();
//     }
//
//     /// <inheritdoc/>
//     public async Task<CommandResult> RemoveAsync(List<Benchmark> model)
//     {
//         Logger.Info("BenchmarkService Update started.");
//         using var scope = ServiceProvider.CreateScope();
//         var command = scope.ServiceProvider.GetRequiredService<RemoveEntityCommand<Benchmark>>();
//         var commandResult = await command.ExecuteAsync(model);
//         if (!commandResult.IsSuccessful)
//         {
//             Logger.Error($"BenchmarkService Update failed. {commandResult.ErrorMessage}");
//
//             return new CommandResult(commandResult.ErrorMessage);
//         }
//
//         Logger.Info("BenchmarkService Update successfully completed.");
//
//         return new CommandResult();
//     }
// }