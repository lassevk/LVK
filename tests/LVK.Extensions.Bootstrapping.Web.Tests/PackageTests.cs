using LVK.Tests.Nuget;

namespace LVK.Extensions.Bootstrapping.Web.Tests;

[TestFixture]
[NugetProject("../../src/LVK.Extensions.Bootstrapping.Web/LVK.Extensions.Bootstrapping.Web.csproj")]
public class PackageTests : NugetTests<PackageTests>
{

}