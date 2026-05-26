using Soenneker.Tests.HostedUnit;

namespace Soenneker.GeoNames.Cities500.Runner.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public sealed class GeonamesCities500RunnerTests : HostedUnitTest
{
    public GeonamesCities500RunnerTests(Host host) : base(host)
    {
    }

    [Test]
    public void Default()
    {

    }
}
