using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.GeoNames.Cities500.Runner.Utils.Abstract;

/// <summary>
/// Defines the file operations util contract.
/// </summary>
public interface IFileOperationsUtil
{
    /// <summary>
    /// Executes the extract data file operation.
    /// </summary>
    /// <param name="zipFilePath">The zip file path.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task containing the result of the operation.</returns>
    ValueTask<string> ExtractDataFile(string zipFilePath, CancellationToken cancellationToken = default);
}
