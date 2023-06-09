using Hangfire;
using Microsoft.Extensions.DependencyInjection;

namespace Server.Services;

/// <inheritdoc />
public class ScheduledJobActivator : JobActivator
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ScheduledJobActivator"/> class.
    /// </summary>
    /// <param name="serviceProvider">Service provider.</param>
    public ScheduledJobActivator(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    private IServiceProvider ServiceProvider { get; }

    /// <inheritdoc/>
    public override object ActivateJob(Type type)
    {
        return ServiceProvider.GetRequiredService(type);
    }
}