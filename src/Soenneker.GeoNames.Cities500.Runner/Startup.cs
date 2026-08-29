using Microsoft.Extensions.DependencyInjection;
using Soenneker.Managers.Runners.Registrars;
using Soenneker.Utils.Directory.Registrars;
using Soenneker.Utils.File.Registrars;
using Soenneker.Utils.File.Download.Registrars;
using Soenneker.GeoNames.Cities500.Runner.Utils;
using Soenneker.GeoNames.Cities500.Runner.Utils.Abstract;

namespace Soenneker.GeoNames.Cities500.Runner;

/// <summary>
/// Represents the startup.
/// </summary>
public static class Startup
{
    /// <summary>
    /// Configures services.
    /// </summary>
    /// <param name="services">The service collection.</param>
    public static void ConfigureServices(IServiceCollection services)
    {
        services.SetupIoC();
    }

    /// <summary>
    /// Registers the services required by the application.
    /// </summary>
    /// <param name="services">Service collection that receives the registration.</param>
    /// <returns>The same service collection, so additional registrations can be chained.</returns>
    public static IServiceCollection SetupIoC(this IServiceCollection services)
    {
        services.AddHostedService<ConsoleHostedService>()
                .AddSingleton<IFileOperationsUtil, FileOperationsUtil>()
                .AddFileUtilAsSingleton()
                .AddDirectoryUtilAsSingleton()
                .AddFileDownloadUtilAsSingleton()
                .AddRunnersManagerAsSingleton();

        return services;
    }
}
