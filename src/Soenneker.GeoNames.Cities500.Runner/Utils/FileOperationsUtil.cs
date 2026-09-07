using System;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Soenneker.Utils.Directory.Abstract;
using Soenneker.Utils.File.Abstract;
using Soenneker.GeoNames.Cities500.Runner.Utils.Abstract;

namespace Soenneker.GeoNames.Cities500.Runner.Utils;

/// <inheritdoc cref="IFileOperationsUtil" />
public sealed class FileOperationsUtil : IFileOperationsUtil
{
    private const string _usCountryCode = "US";

    private readonly ILogger<FileOperationsUtil> _logger;
    private readonly IFileUtil _fileUtil;
    private readonly IDirectoryUtil _directoryUtil;

    public FileOperationsUtil(ILogger<FileOperationsUtil> logger, IFileUtil fileUtil, IDirectoryUtil directoryUtil)
    {
        _logger = logger;
        _fileUtil = fileUtil;
        _directoryUtil = directoryUtil;
    }

    public async ValueTask<string> ExtractDataFile(string zipFilePath, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Extracting {SourceFileName} from {ZipFilePath}...", Constants.SourceFileName, zipFilePath);

        string workingDirectory = await _directoryUtil.CreateTempDirectory(cancellationToken);
        string resultFilePath = Path.Combine(workingDirectory, Constants.FileName);

        await using FileStream zipStream = _fileUtil.OpenRead(zipFilePath);
        await using var archive = new ZipArchive(zipStream, ZipArchiveMode.Read);

        ZipArchiveEntry? sourceEntry = archive.GetEntry(Constants.SourceFileName);

        if (sourceEntry == null)
            throw new FileNotFoundException($"Could not find {Constants.SourceFileName} in archive", Constants.SourceFileName);

        await using Stream sourceStream = await sourceEntry.OpenAsync(cancellationToken);
        using var reader = new StreamReader(sourceStream);
        await using FileStream resultStream = _fileUtil.OpenWrite(resultFilePath);
        await using var writer = new StreamWriter(resultStream);

        var totalRows = 0;
        var writtenRows = 0;

        while (await reader.ReadLineAsync(cancellationToken) is { } line)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            totalRows++;

            string? output = FormatUsRow(line);
            if (output is null)
                continue;

            await writer.WriteLineAsync(output);
            writtenRows++;
        }

        _logger.LogInformation("Extracted {WrittenRows} US rows from {TotalRows} {SourceFileName} rows to {ResultFilePath}.", writtenRows, totalRows,
            Constants.SourceFileName, resultFilePath);

        return resultFilePath;
    }

    private static string? FormatUsRow(string line)
    {
        Span<Range> columns = stackalloc Range[12];
        ReadOnlySpan<char> span = line;
        int count = span.Split(columns, '\t');
        if (count < 11 || !span[columns[8]].SequenceEqual(_usCountryCode))
            return null;

        return $"{span[columns[1]]}\t{span[columns[10]]}\t{span[columns[4]]}\t{span[columns[5]]}";
    }

}
