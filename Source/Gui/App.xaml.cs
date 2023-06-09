using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Client;
using Client.Services;
using Common;
using Gui.Common;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace Gui;

/// <summary>
/// Interaction logic for App.xaml.
/// </summary>
public partial class App : Application
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public static IServiceProvider ServiceProvider { get; set; } = null!;

    /// <inheritdoc/>
    protected override void OnStartup(StartupEventArgs args)
    {
        ConfigureExceptionsHandling();
        Configurator.ConfigureNLog();
        ConfigureServices();
        RunMainWindow();
        RunConnectionTest();
    }

    private static void ConfigureExceptionsHandling()
    {
        Current.DispatcherUnhandledException += DispatcherUnhandledExceptionThrowing;
        AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionThrowing;
        TaskScheduler.UnobservedTaskException += UnobservedTaskExceptionThrowing;
    }

    private static void ConfigureServices()
    {
        var services = new ServiceCollection();
        services.AddGui();
        services.AddClient();
        services.AddServices();
        ServiceProvider = services.BuildServiceProvider(/*validateScopes: true*/);
    }

    private static async void RunConnectionTest()
    {
        using var scope = ServiceProvider.CreateScope();
        var testService = scope.ServiceProvider.GetRequiredService<TestConnectionService>();
        var testConnectionResult = await Task.Run(() => testService.TestConnectionAsync());
        if (!testConnectionResult.IsSuccessful)
        {
            await ShowErrorMessageAsync(TextConstants.ConnectionErrorOccured);
            Current.Shutdown();
        }
    }

    private static void RunMainWindow()
    {
        Current.ShutdownMode = ShutdownMode.OnLastWindowClose;
        var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }

    private static void DispatcherUnhandledExceptionThrowing(object sender, DispatcherUnhandledExceptionEventArgs args)
    {
        var errorMessage = @$"Произошло необработанное исключение: {args.Exception}";
        Logger.Error(errorMessage);
        args.Handled = true;
        ShowErrorMessage(TextConstants.UnidentifiedErrorOccured);
    }

    private static void UnhandledExceptionThrowing(object sender, UnhandledExceptionEventArgs args)
    {
        var errorMessage = @$"Произошло необработанное исключение: {args.ExceptionObject}";
        Logger.Error(errorMessage);
        ShowErrorMessage(TextConstants.UnidentifiedErrorOccured);
    }

    private static void UnobservedTaskExceptionThrowing(object? sender, UnobservedTaskExceptionEventArgs args)
    {
        var errorMessage = @$"Произошло необработанное исключение: {args.Exception}";
        Logger.Error(errorMessage);
        args.SetObserved();
        ShowErrorMessage(TextConstants.UnidentifiedErrorOccured);
    }

    private static async Task ShowErrorMessageAsync(string message)
    {
        await ShowMessageAsync(TextConstants.ErrorHeader, message, MessageBoxImage.Error);
    }

    private static async Task ShowMessageAsync(string header, string message, MessageBoxImage messageBoxImage)
    {
        await Current.Dispatcher.InvokeAsync(async () =>
        {
            if (Current.Windows.Count == 0)
            {
                MessageBox.Show(
                    message,
                    header,
                    MessageBoxButton.OK,
                    messageBoxImage);

                return;
            }

            var messageDialog = new MessageDialog(header, message);
            await DialogHost.Show(messageDialog, "RootError");
        }).Result;
    }

    private static void ShowErrorMessage(string message)
    {
        ShowMessage(TextConstants.ErrorHeader, message, MessageBoxImage.Error);
    }

    private static void ShowMessage(string header, string message, MessageBoxImage messageBoxImage)
    {
        Current.Dispatcher.Invoke(async () =>
        {
            if (Current.Windows.Count == 0)
            {
                MessageBox.Show(
                    message,
                    header,
                    MessageBoxButton.OK,
                    messageBoxImage);

                return;
            }

            var messageDialog = new MessageDialog(header, message);
            await DialogHost.Show(messageDialog, "RootError");
        });
    }
}