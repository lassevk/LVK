using LVK.Tests;

namespace LVK.Core.App.WindowsService.Tests;

[TestFixture]
[NugetProject("../../src/LVK.Core.App.WindowsService/LVK.Core.App.WindowsService.csproj")]
public class PackageTests : NugetTests<PackageTests>;