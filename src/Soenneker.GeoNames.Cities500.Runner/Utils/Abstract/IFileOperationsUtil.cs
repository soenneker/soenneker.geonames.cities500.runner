using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.GeoNames.Cities500.Runner.Utils.Abstract;

/// <summary>
/// Produces the packaged US city extract from a GeoNames cities500 archive.
/// </summary>
public interface IFileOperationsUtil
{
    /// <summary>
    /// Extracts US city, state, latitude, and longitude columns from the archive.
    /// </summary>
    /// <param name="zipFilePath">Path of the zip file to use.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The path to the extracted data file in a temporary directory.</returns>
    ValueTask<string> ExtractDataFile(string zipFilePath, CancellationToken cancellationToken = default);
}
