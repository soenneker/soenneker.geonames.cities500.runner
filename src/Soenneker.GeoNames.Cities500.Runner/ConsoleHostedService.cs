using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Soenneker.Managers.Runners.Abstract;
using Soenneker.Utils.File.Download.Abstract;
using Soenneker.GeoNames.Cities500.Runner.Utils.Abstract;

namespace Soenneker.GeoNames.Cities500.Runner;

/// <summary>
/// Represents the console hosted service.
/// </summary>
public sealed class ConsoleHostedService : IHostedService
{
    private readonly ILogger<ConsoleHostedService> _logger;
    private readonly IHostApplicationLifetime _appLifetime;
    private readonly IRunnersManager _runnersManager;
    private readonly IFileDownloadUtil _fileDownloadUtil;
    private readonly IFileOperationsUtil _fileOperationsUtil;
    private int? _exitCode;

    public ConsoleHostedService(ILogger<ConsoleHostedService> logger, IHostApplicationLifetime appLifetime, IRunnersManager runnersManager,
        IFileDownloadUtil fileDownloadUtil, IFileOperationsUtil fileOperationsUtil)
    {
        _logger = logger;
        _appLifetime = appLifetime;
        _runnersManager = runnersManager;
        _fileDownloadUtil = fileDownloadUtil;
        _fileOperationsUtil = fileOperationsUtil;
    }

    /// <summary>
    /// Executes the start async operation.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task StartAsync(CancellationToken cancellationToken = default)
    {
        _appLifetime.ApplicationStarted.Register(() =>
        {
            Task.Run(async () =>
            {
                _logger.LogInformation("Running console hosted service ...");

                try
                {
                    string zipFilePath = (await _fileDownloadUtil.Download(Constants.DownloadUri, fileExtension: ".zip", cancellationToken: cancellationToken))!;
                    string filePath = await _fileOperationsUtil.ExtractDataFile(zipFilePath, cancellationToken);

                    await _runnersManager.PushIfChangesNeeded(filePath, Constants.FileName, Constants.Library,
                        $"https://github.com/soenneker/{Constants.Library}", false, cancellationToken);

                    _logger.LogInformation("Complete!");
                    _exitCode = 0;
                }
                catch (Exception e)
                {
                    if (Debugger.IsAttached)
                        Debugger.Break();

                    _logger.LogError(e, "Unhandled exception");

                    await Task.Delay(2000, cancellationToken);
                    _exitCode = 1;
                }
                finally
                {
                    _appLifetime.StopApplication();
                }
            }, cancellationToken);
        });

        return Task.CompletedTask;
    }

    /// <summary>
    /// Executes the stop async operation.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogDebug("Exiting with return code: {exitCode}", _exitCode);
        Environment.ExitCode = _exitCode.GetValueOrDefault(-1);
        return Task.CompletedTask;
    }
}
