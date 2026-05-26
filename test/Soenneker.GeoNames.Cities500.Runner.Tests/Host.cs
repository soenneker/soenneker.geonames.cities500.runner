using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Soenneker.GeoNames.Cities500.Runner.Utils;
using Soenneker.GeoNames.Cities500.Runner.Utils.Abstract;
using Soenneker.TestHosts.Unit;
using Soenneker.Utils.Directory.Registrars;
using Soenneker.Utils.File.Registrars;
using Soenneker.Utils.Test;

namespace Soenneker.GeoNames.Cities500.Runner.Tests;

public sealed class Host : UnitTestHost
{
    public override Task InitializeAsync()
    {
        SetupIoC(Services);

        return base.InitializeAsync();
    }

    private static void SetupIoC(IServiceCollection services)
    {
        services.AddLogging(builder =>
        {
            builder.AddSerilog(dispose: false);
        });

        IConfiguration config = TestUtil.BuildConfig();
        services.AddSingleton(config);

        services.AddSingleton<IFileOperationsUtil, FileOperationsUtil>()
                .AddFileUtilAsSingleton()
                .AddDirectoryUtilAsSingleton();
    }
}
