namespace CleanModels;

/// <summary>
/// Constants.
/// </summary>
public static class Constants
{
    public const string ProgramName = "Endpoint analyzer 2023";
    public static readonly string ProgramExePath = Directory.GetCurrentDirectory();
    public static readonly string UserDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
    public static readonly string SourceBenchmarksDirectoryPath = @$"{ProgramExePath}\..\..\..\..\..\Benchmarks";
    public const string UnidentifiedErrorOccuredOnServer = @"Ошибка на стороне сервера."; // Непредвиденная/неопознанная
}