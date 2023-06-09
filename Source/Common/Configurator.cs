using System.Reflection;
using NLog;
using NLog.Config;

namespace Common;

/// <summary>
/// Configurator.
/// </summary>
public static class Configurator
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// Configure exceptions handling.
    /// </summary>
    public static void ConfigureExceptionsHandling()
    {
        AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionThrowing;
        TaskScheduler.UnobservedTaskException += UnobservedTaskExceptionThrowing;
    }

    /// <summary>
    /// Configure NLog.
    /// </summary>
    public static void ConfigureNLog()
    {
        var instanceFolder = InstanceFolderHelper.GetInstanceFolder();
        string nlogPath = Path.Combine(Directory.GetCurrentDirectory(), "nlog.config");
        string logFolder = Path.Combine(instanceFolder.FullName, "Logs");
        var entryAssembly = Assembly.GetEntryAssembly();
        string? applicationName = entryAssembly?.GetName().Name;
        LogManager.Configuration = new XmlLoggingConfiguration(nlogPath);
        LogManager.Configuration.Variables.Add("MainLogFolder", logFolder);
        LogManager.Configuration.Variables.Add("ApplicationName", applicationName);
    }

    private static void UnhandledExceptionThrowing(object sender, UnhandledExceptionEventArgs args)
    {
        var errorMessage = @$"Произошло необработанное исключение: {args.ExceptionObject}";
        Logger.Error(errorMessage);
    }

    private static void UnobservedTaskExceptionThrowing(object? sender, UnobservedTaskExceptionEventArgs args)
    {
        var errorMessage = @$"Произошло необработанное исключение: {args.Exception}";
        Logger.Error(errorMessage);
        args.SetObserved();
    }
}