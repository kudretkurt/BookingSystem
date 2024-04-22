using BookingSystem.API.LifeTimeHooks;

namespace BookingSystem.API.Installers;

public static class HostLifeTimeInstaller
{
    //For Gracefully shutdown
    //https://github.com/dotnet/dotnet-docker/blob/main/samples/kubernetes/graceful-shutdown/graceful-shutdown.md
    public static void AddDelayedShutdownHostLifeTime(this IServiceCollection services)
    {
        services.AddSingleton<IHostLifetime>(sp =>
            new DelayedShutdownHostLifetime(sp.GetRequiredService<IHostApplicationLifetime>(),
                TimeSpan.FromSeconds(5)));
    }
}