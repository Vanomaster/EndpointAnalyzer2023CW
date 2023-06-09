using Analyzers;
using Common;
using Dal;
using Dal.Commands;
using Dal.Queries;
using Hangfire;
using Hangfire.PostgreSql;
using NLog;
using NLog.Extensions.Logging;
using Parsers;
using Server.Client;
using Server.Controllers;
using Server.Queries;
using Server.Services;

namespace Server;

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
        services.AddAnalyzers();
        services.AddDal();
        services.AddDalCommands();
        services.AddDalQueries();
        services.AddParsers();
        services.AddClient();
        services.AddQueries();
        services.AddServices();
        // services.AddSingleton<SoftwareServiceController>(); // overload MapGrpcService with Singleton instead of Scoped
        services.AddLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddNLog(LogManager.Configuration);
        });

        services.AddHangfire((provider, configuration) => configuration
            //.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseNLogLogProvider()
            // .UseFilter(provider.GetRequiredService<AutomaticRetryAttribute>())
            .UsePostgreSqlStorage(new ConfigurationHelper().ScheduleConnectionString));

        services.AddHangfireServer();
        // TODO изменить формат даты на подходящий
        // Добавлено из-за ошибки Cannot write DateTime with Kind=Local to PostgreSQL type 'timestamp with time zone', only UTC is supported.
        // Note that it's not possible to mix DateTimes with different Kinds in an array/range. See the Npgsql.EnableLegacyTimestampBehavior AppContext switch to enable legacy behavior.
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    /// <summary>
    /// Configure application.
    /// </summary>
    /// <param name="app">Application builder.</param>
    /// <param name="env">Web host environment.</param>
    /// <param name="serviceProvider">Service provider.</param>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        using (var scope = serviceProvider.CreateScope())
        {
            var initService = scope.ServiceProvider.GetRequiredService<InitService>();
            initService.InitAsync().Wait();
        }

        GlobalConfiguration.Configuration.UseActivator(new ScheduledJobActivator(serviceProvider));
        //GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 5 });
        //app.UseHangfireDashboard();
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGrpcService<AnalysisResultServiceController>();
            endpoints.MapGrpcService<AnalysisServiceController>();
            endpoints.MapGrpcService<BenchmarkServiceController>();
            endpoints.MapGrpcService<ConfigurationRecommendationsBenchmarkServiceController>();
            endpoints.MapGrpcService<ConfigurationRecommendationsServiceController>();
            endpoints.MapGrpcService<NetworkServiceController>();
            endpoints.MapGrpcService<ScheduleServiceController>();
            endpoints.MapGrpcService<TestServiceController>();
            endpoints.MapGrpcService<TrustSoftServiceController>();
            endpoints.MapGrpcService<TrustHardServiceController>();
            endpoints.MapGrpcService<TrustSoftBenchServiceController>();
            endpoints.MapGrpcService<TrustHardBenchServiceController>();
            //endpoints.MapGrpcService<TrustedHardwareBenchmarkServiceController>();
            //endpoints.MapGrpcService<TrustedSoftwareBenchmarkServiceController>();
            //endpoints.MapHangfireDashboard();
            endpoints.MapGet("/", async context =>
            {
                await context.Response
                    .WriteAsync("Communication with gRPC endpoints must be made through a gRPC client.");
            });
        });
    }
}