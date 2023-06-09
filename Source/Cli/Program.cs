using Cli.Base;
using Client;
using Client.Clients;
using Client.Services;
using Common;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace Cli;

/// <summary>
/// The main entry point for the application.
/// </summary>
internal static class Program
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    private static IServiceProvider ServiceProvider { get; set; } = null!;

    private static void Main(string[] args)
    {
        Configurator.ConfigureExceptionsHandling();
        Configurator.ConfigureNLog();
        ConfigureConsole();
        ConfigureServices();
        RunConnectionTest();
        RunInputHandler();
    }

    private static void ConfigureConsole()
    {
        Console.Title = Constants.ProgramName;
        Console.WriteLine(@"Запуск...");
        Logger.Info("Console configured successfully");
    }

    private static void ConfigureServices()
    {
        var services = new ServiceCollection();
        services.AddCli();
        services.AddClient();
        services.AddServices();
        ServiceProvider = services.BuildServiceProvider(validateScopes: true);
        Logger.Info("Services configured successfully");
    }

    private static void RunConnectionTest()
    {
        var testServiceClient = ServiceProvider.GetRequiredService<TestServiceClient>(); // TODO to scope
        var commandResult = Task.Run(() => testServiceClient.TestAsync()).Result;
        if (!commandResult.IsSuccessful)
        {
            throw new Exception(commandResult.ErrorMessage);
        }
    }

    private static void RunInputHandler()
    {
        var inputHandler = ServiceProvider.GetRequiredService<IHandler>();
        inputHandler.Handle();
    }
}