using LVK.Tests.Nuget;

namespace LVK.Core.Tests;

[TestFixture]
[NugetProject("../../src/LVK.Core/LVK.Core.csproj")]
public class PackageTests : NugetTests<PackageTests>
{

}