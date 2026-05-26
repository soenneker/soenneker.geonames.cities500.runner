using Microsoft.Extensions.DependencyInjection;
using Soenneker.Managers.Runners.Registrars;
using Soenneker.Utils.Directory.Registrars;
using Soenneker.Utils.File.Registrars;
using Soenneker.Utils.File.Download.Registrars;
using Soenneker.GeoNames.Cities500.Runner.Utils;
using Soenneker.GeoNames.Cities500.Runner.Utils.Abstract;

namespace Soenneker.GeoNames.Cities500.Runner;

public static class Startup
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.SetupIoC();
    }

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
