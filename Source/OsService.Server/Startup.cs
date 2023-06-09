using Common;
using NLog;
using NLog.Extensions.Logging;
using OsService.Client;
using OsService.Commands;
using OsService.Queries;
using OsService.Server.Controllers;
using OsService.Services;
using OsService.Watchers;

namespace OsService.Server;

/// <summary>
/// Startup.
/// </summary>
public class Startup
{
    /// <summary>
    /// Configure services.
    /// </summary>
    /// <param name="services">Services.</param>
    public void ConfigureServices(IServiceCollection services)
    {
        Configurator.ConfigureExceptionsHandling();
        Configurator.ConfigureNLog();
        services.AddGrpc();
        services.AddClient();
        services.AddCommands();
        services.AddQueries();
        services.AddServices();
        services.AddWatchers();
        services.AddLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddNLog(LogManager.Configuration);
        });
    }

    /// <summary>
    /// Configure application.
    /// </summary>
    /// <param name="app">Application builder.</param>
    /// <param name="env">Web host environment.</param>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGrpcService<TestServiceController>();
            endpoints.MapGrpcService<OsInfoServiceController>();
            endpoints.MapGet("/", async context =>
            {
                await context.Response
                    .WriteAsync("Communication with gRPC endpoints must be made through a gRPC client.");
            });
        });
    }
}