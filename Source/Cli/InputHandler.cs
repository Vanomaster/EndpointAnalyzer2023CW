using Cli.Base;
using Cli.Models;
using Common;

namespace Cli;

/// <summary>
/// Input handler.
/// </summary>
public class InputHandler : IHandler
{
    private static readonly List<string> MenuItems = new ()
    {
        @"Запустить анализ параметров групповых политик",
        @"Запустить анализ подключенных устройств",
        @"Запустить анализ программного обеспечения",
        @"Выйти",
    };

    private static readonly List<string> SoftwareMenuItems = new ()
    {
        @"Запустить анализ обновлений программного обеспечения",
        @"Запустить анализ довереного программного обеспечения",
        @"Назад",
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="InputHandler"/> class.
    /// </summary>
    /// <param name="benchmarkModel">Model.</param>
    public InputHandler(BenchmarkModel benchmarkModel)
    {
        BenchmarkModel = benchmarkModel;
    }

    private BenchmarkModel BenchmarkModel { get; }

    /// <summary>
    /// Handle input.
    /// </summary>
    public void Handle()
    {
        while (true)
        {
            DrawMainMenu();
        }
    }

    private void DrawMainMenu()
    {
        Console.Clear();
        int selectedMenuItem = Drawer.DrawMenu(MenuItems);
        switch (selectedMenuItem)
        {
            case -2:
            {
                Console.Clear();
                break;
            }

            case -1:
            {
                DisplayInvalidInputErrorMessage();
                break;
            }

            case 0:
            {
                string path = $"{InstanceFolderHelper.GetInstanceFolder()}" +
                              @"/Db/Программное обеспечение/Доверенное программное обеспечение.csv";

                var commandResult = Task.Run(() => BenchmarkModel.GetFromFileAsync(path)).Result;
                // var service = ServiceProvider.GetRequiredService<ConfigurationsAnalyzer>();
                // DisplayRecommendations(service, @"анализатора параметров групповых политик");
                break;
            }

            case 1:
            {
                // var service = ServiceProvider.GetRequiredService<HardwareAnalyzer>();
                // DisplayRecommendations(service, @"анализатора подключенных устройств");
                break;
            }

            case 2:
            {
                DrawSoftwareMenu();
                Drawer.ResetCursorPosition();
                break;
            }

            default:
            {
                Environment.Exit(0);
                break;
            }
        }
    }

    private void DrawSoftwareMenu()
    {
        Drawer.ResetCursorPosition();
        while (true)
        {
            Console.Clear();
            int selectedMenuItem = Drawer.DrawMenu(SoftwareMenuItems);
            switch (selectedMenuItem)
            {
                case -2:
                {
                    Console.Clear();
                    break;
                }

                case -1:
                {
                    DisplayInvalidInputErrorMessage();
                    break;
                }

                case 0:
                {
                    // var service = ServiceProvider.GetRequiredService<SoftwareUpdateAnalyzer>();
                    // DisplayRecommendations(service, @"анализатора обновлений программного обеспечения");
                    break;
                }

                case 1:
                {
                    // var service = ServiceProvider.GetRequiredService<SoftwareTrustAnalyzer>();
                    // DisplayRecommendations(service, @"анализатора доверенного программного обеспечения");
                    break;
                }

                default:
                {
                    return;
                }
            }
        }
    }

    // private void DisplayRecommendations(IAnalyzer<List<string>> analyzer, string analyzerName)
    // {
    //     Console.Clear();
    //     Console.WriteLine(@"Выполнение анализа...");
    //     var analyzeResult = analyzer.Analyze();
    //     Console.Clear();
    //     if (!analyzeResult.IsSuccessful)
    //     {
    //         string errorMessage = GetErrorMessage(analyzeResult.ErrorMessage);
    //         Console.WriteLine(@$"В результате работы {analyzerName} произошла ошибка:" + "\n\n" + errorMessage);
    //         string errorMessageLogResult = LogAnalyzeResult(analyzerName, new List<string> { errorMessage });
    //         Console.WriteLine("\n" + errorMessageLogResult);
    //         DisplayContinueMessage();
    //
    //         return;
    //     }
    //
    //     Console.WriteLine(@$"В результате работы {analyzerName} был получен следующий результат:" + "\n");
    //     foreach (string line in analyzeResult.Data)
    //     {
    //         Console.WriteLine(line);
    //     }
    //
    //     string dataLogResult = LogAnalyzeResult(analyzerName, analyzeResult.Data);
    //     Console.WriteLine("\n\n" + dataLogResult);
    //     DisplayContinueMessage();
    // }

    private static void DisplayInvalidInputErrorMessage()
    {
        Console.Clear();
        Console.WriteLine(@"Была нажата неподдерживаемая для работы с меню клавиша." + "\n"
            + "Выберите пункт меню с помощью клавиш \"Вверх\" и \"Вниз\" и нажмите клавишу \"Ввод\" для подтверждения выбора.");

        DisplayContinueMessage();
    }

    private static void DisplayContinueMessage()
    {
        Console.WriteLine("\n\n_____________________________________");
        Console.WriteLine(@"Нажмите любую клавишу для продолжения");
        Console.SetCursorPosition(0, 0);
        Console.ReadKey();
        Console.Clear();
    }

    private string LogAnalyzeResult(string analyzerName, List<string> analyzeResult)
    {
        var logFileName = @$"Журнал {analyzerName}.txt";
        // var logger = ServiceProvider.GetRequiredService<Logger>();
        // string logResult = logger.Log(analyzeResult, logFileName);

        return "logResult";
    }

    private static string GetErrorMessage(string analyzeResultErrorMessage)
    {
        const string errorMessagePreStartString = ": ";
        const string errorMessageEndString = "  at";
        int errorMessagePreStartStringLength = errorMessagePreStartString.Length;
        int errorMessagePreStartIndex = analyzeResultErrorMessage
            .IndexOf(errorMessagePreStartString, StringComparison.Ordinal);

        int errorMessageEndIndex = analyzeResultErrorMessage.IndexOf(errorMessageEndString, StringComparison.Ordinal);
        string errorMessage = analyzeResultErrorMessage.Substring(
            errorMessagePreStartIndex + errorMessagePreStartStringLength,
            errorMessageEndIndex - errorMessagePreStartIndex);

        return errorMessage;
    }
}