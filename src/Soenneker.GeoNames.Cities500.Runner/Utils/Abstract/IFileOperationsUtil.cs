using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.GeoNames.Cities500.Runner.Utils.Abstract;

/// <summary>
/// Defines the file operations util contract.
/// </summary>
public interface IFileOperationsUtil
{
    /// <summary>
    /// Extracts data File.
    /// </summary>
    /// <param name="zipFilePath">Path of the zip file to use.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task whose result is the text returned by extract Data File.</returns>
    ValueTask<string> ExtractDataFile(string zipFilePath, CancellationToken cancellationToken = default);
}
