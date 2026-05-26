using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.GeoNames.Cities500.Runner.Utils.Abstract;

public interface IFileOperationsUtil
{
    ValueTask<string> ExtractDataFile(string zipFilePath, CancellationToken cancellationToken = default);
}
