using LVK.Tests.Nuget;

namespace LVK.Extensions.Bootstrapping.Console.Tests;

[TestFixture]
[NugetProject("../../src/LVK.Extensions.Bootstrapping.Console/LVK.Extensions.Bootstrapping.Console.csproj")]
public class PackageTests : NugetTests<PackageTests>
{

}