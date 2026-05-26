using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Soenneker.GeoNames.Cities500.Runner.Utils.Abstract;
using Soenneker.Tests.HostedUnit;

namespace Soenneker.GeoNames.Cities500.Runner.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public sealed class GeonamesCities500RunnerTests : HostedUnitTest
{
    private readonly IFileOperationsUtil _fileOperationsUtil;

    public GeonamesCities500RunnerTests(Host host) : base(host)
    {
        _fileOperationsUtil = Resolve<IFileOperationsUtil>(true);
    }

    [Test]
    public async Task Extracts_cities500_data_file()
    {
        string zipFilePath = Path.Combine(Path.GetTempPath(), $"{nameof(Extracts_cities500_data_file)}.zip");

        if (File.Exists(zipFilePath))
            File.Delete(zipFilePath);

        await using (FileStream zipStream = File.Create(zipFilePath))
        {
            using var archive = new ZipArchive(zipStream, ZipArchiveMode.Create);
            ZipArchiveEntry entry = archive.CreateEntry(Constants.SourceFileName);

            await using Stream entryStream = entry.Open();
            await using var writer = new StreamWriter(entryStream);
            await writer.WriteLineAsync("5128581\tNew York City\tNew York City\tNew York,NYC\t40.71427\t-74.00597\tP\tPPL\tUS\t\tNY\t061\t\t\t8804190\t10\t57\tAmerica/New_York\t2024-11-12");
        }

        string resultPath = await _fileOperationsUtil.ExtractDataFile(zipFilePath);
        string result = (await File.ReadAllTextAsync(resultPath)).Replace("\r\n", "\n");

        await Assert.That(result.Trim()).IsEqualTo("5128581\tNew York City\tNew York City\tNew York,NYC\t40.71427\t-74.00597\tP\tPPL\tUS\t\tNY\t061\t\t\t8804190\t10\t57\tAmerica/New_York\t2024-11-12");
    }
}
