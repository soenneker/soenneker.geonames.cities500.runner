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
    /// Sets up io c.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The result of the operation.</returns>
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
