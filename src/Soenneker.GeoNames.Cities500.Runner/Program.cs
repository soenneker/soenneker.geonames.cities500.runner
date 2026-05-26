using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Soenneker.Extensions.LoggerConfiguration;

namespace Soenneker.GeoNames.Cities500.Runner;

public static class Program
{
    public static async Task Main(string[] args)
    {
        IConfigurationRoot config = new ConfigurationBuilder()
                                    .AddJsonFile("appsettings.json", true, true)
                                    .AddEnvironmentVariables()
                                    .AddCommandLine(args)
                                    .Build();

        Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(config)
                                              .Enrich.FromLogContext()
                                              .WriteTo.Console()
                                              .CreateBootstrapLogger();

        await Host.CreateDefaultBuilder(args)
                  .UseSerilog((context, services, configuration) => configuration.ConfigureLogger(context.Configuration, services))
                  .ConfigureServices((_, services) =>
                  {
                      services.AddSingleton<IConfiguration>(config);
                      Startup.ConfigureServices(services);
                  })
                  .RunConsoleAsync();
    }
}
