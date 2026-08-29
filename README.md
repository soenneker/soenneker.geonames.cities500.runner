[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.geonames.cities500.runner/build-and-test.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.geonames.cities500.runner/actions/workflows/build-and-test.yml)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.geonames.cities500.runner/daily-automatic-update.yml?style=for-the-badge&label=Daily%20Update)](https://github.com/soenneker/soenneker.geonames.cities500.runner/actions/workflows/daily-automatic-update.yml)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.geonames.cities500.runner/codeql.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.geonames.cities500.runner/actions/workflows/codeql.yml)

# Soenneker.GeoNames.Cities500.Runner

Defines the file operations util contract.

> This is an automation runner, not a package intended for application consumption.

## What the runner does

- `IFileOperationsUtil.ExtractDataFile(zipFilePath, cancellationToken)` — Extracts data File.
- `Constants.DownloadUri` — The download uri.
- `Constants.SourceFileName` — The source file name.
- `Constants.FileName` — The file name.
- `Constants.Library` — The library.

## What you get

- `IFileOperationsUtil` — Defines the file operations util contract.
- `Constants` — Represents the constants.
- `ConsoleHostedService` — Represents the console hosted service.

## API at a glance

| API | What it does | Result / important behavior |
| --- | --- | --- |
| `IFileOperationsUtil.ExtractDataFile(zipFilePath, cancellationToken)` | Extracts data File. | A task whose result is the text returned by extract Data File. |
| `ConsoleHostedService.StartAsync(cancellationToken)` | Starts the console hosted service and begins its background work. | A task that completes after the console hosted service has started. |
| `ConsoleHostedService.StopAsync(cancellationToken)` | Stops the console hosted service and waits for its background work to finish. | A task that completes after the console hosted service has stopped. |

## Practical notes

- Cancellation stops pending work; it does not undo work that has already completed.
