namespace OsService.Server;

/// <summary>
/// Program.
/// </summary>
public static class Program
{
    /// <summary>
    /// Main.
    /// </summary>
    /// <param name="args">Args.</param>
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run(); // use WebApplication https://andrewlock.net/exploring-dotnet-6-part-2-comparing-webapplicationbuilder-to-the-generic-host/
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        var hostBuilder = Host.CreateDefaultBuilder(args)
            .UseSystemd()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });

        return hostBuilder;
    }
}