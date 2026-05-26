using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Soenneker.Utils.Directory.Abstract;
using Soenneker.Utils.File.Abstract;
using Soenneker.GeoNames.Cities500.Runner.Utils.Abstract;

namespace Soenneker.GeoNames.Cities500.Runner.Utils;

///<inheritdoc cref="IFileOperationsUtil"/>
public sealed class FileOperationsUtil : IFileOperationsUtil
{
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
        await using FileStream resultStream = _fileUtil.OpenWrite(resultFilePath);
        await sourceStream.CopyToAsync(resultStream, cancellationToken);

        _logger.LogInformation("Extracted {SourceFileName} to {ResultFilePath}.", Constants.SourceFileName, resultFilePath);

        return resultFilePath;
    }
}
