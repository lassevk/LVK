using LVK.Tests.Nuget;

namespace LVK.Extensions.Bootstrapping.Tests;

[TestFixture]
[NugetProject("../../src/LVK.Extensions.Bootstrapping/LVK.Extensions.Bootstrapping.csproj")]
public class PackageTests : NugetTests<PackageTests>
{

}