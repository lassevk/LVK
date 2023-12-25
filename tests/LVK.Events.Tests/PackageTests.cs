using LVK.Tests.Nuget;

namespace LVK.Events.Tests;

[TestFixture]
[NugetProject("../../src/LVK.Events/LVK.Events.csproj")]
public class PackageTests : NugetTests<PackageTests>
{

}

[TestFixture]
[NugetProject("../../src/LVK.Events.Bootstrapped/LVK.Events.Bootstrapped.csproj")]
public class BootstrappedPackageTests : NugetTests<BootstrappedPackageTests>
{

}