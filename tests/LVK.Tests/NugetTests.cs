using System.Reflection;
using System.Text.RegularExpressions;

namespace LVK.Tests;

public abstract class NugetTests<T>
    where T : NugetTests<T>
{
    private static readonly Lazy<string?> _projectFilePath;
    private static readonly Lazy<Dictionary<string, string>> _properties;
    private static readonly Lazy<List<string>> _projectFile;

    static NugetTests()
    {
        _projectFilePath = new Lazy<string?>(GetProjectFilePath);
        _properties = new Lazy<Dictionary<string, string>>(ReadProjectFileAsDictionary);
        _projectFile = new Lazy<List<string>>(ReadProjectFileAsLines);
    }

    private static List<string> ReadProjectFileAsLines()
    {
        string? filePath = _projectFilePath.Value;
        if (filePath == null)
            return new List<string>();

        if (!File.Exists(filePath))
            return new List<string>();

        var result = new List<string>();

        using StreamReader streamReader = File.OpenText(filePath);
        while (streamReader.ReadLine() is { } line)
        {
            result.Add(line);
        }

        return result;
    }

    private static string? GetProjectFilePath()
    {
        NugetProjectAttribute? attr = typeof(T).GetCustomAttribute<NugetProjectAttribute>();
        if (attr == null)
            return null;

        string? location = Path.GetDirectoryName(typeof(NugetTests<T>).Assembly.Location);
        if (location == null)
            return null;

        while (true)
        {
            string parentPath = Path.GetFullPath(Path.Combine(location, ".."));
            if (StringComparer.InvariantCultureIgnoreCase.Equals(location, parentPath))
                return null;

            if (Directory.GetFiles(location, "*.csproj").Length > 0)
            {
                return Path.GetFullPath(Path.Combine(location, attr.RelativePath));
            }

            location = parentPath;
        }
    }

    private static Dictionary<string, string> ReadProjectFileAsDictionary()
    {
        var result = new Dictionary<string, string>();

        var re = new Regex("""^\s*<(?<name>[^>]+)>(?<value>[^<]+)</\1>""");
        foreach (string line in _projectFile.Value)
        {
            Match ma = re.Match(line);
            if (ma.Success)
                result[ma.Groups["name"].Value] = ma.Groups["value"].Value;
        }

        return result;
    }

    [Test]
    public void Csproj_CanLocateFile()
    {
        string? projectFilePath = _projectFilePath.Value;

        Assert.That(projectFilePath, Is.Not.Null);
        Assert.That(projectFilePath, Does.Exist);
    }

    private static IEnumerable<TestCaseData> RequiredPropertyValues()
    {
        TestCaseData prop(string name, string value) => new TestCaseData(name, value).SetName($"{name} = {value}");

        yield return prop("TargetFramework", "net8.0");
        yield return prop("Nullable", "enable");
        yield return prop("Title", Path.GetFileNameWithoutExtension(_projectFilePath.Value)!);
        yield return prop("Authors", "Lasse V\u00e5gs\u00e6ther Karlsen");
        yield return prop("Copyright", "Lasse Vågsæther Karlsen $([System.DateTime]::Today.ToString('yyyy')), All rights reserved");
        yield return prop("PackageReadmeFile", "README.md");
        yield return prop("PackageLicenseFile", "LICENSE.md");
        yield return prop("PublishRepositoryUrl", "true");
        yield return prop("EmbedUntrackedSource", "true");
        yield return prop("DebugType", "embedded");
    }

    [Test]
    [TestCaseSource(nameof(RequiredPropertyValues))]
    public void Csproj_ContainsTheRightPropertyValues(string propertyName, string value)
    {
        Assert.That(_properties.Value[propertyName], Is.EqualTo(value));
    }

    private static IEnumerable<TestCaseData> RequiredSections()
    {
        string? projectName = Path.GetFileNameWithoutExtension(_projectFilePath.Value);
        yield return new TestCaseData("""
                                      <PackageReference\s*Include="Microsoft.SourceLink.GitHub"\s*Version="8\.\d+\.\d+">
                                          <PrivateAssets>all</PrivateAssets>
                                          <IncludeAssets>runtime;\s*build;\s*native;\s*contentfiles;\s*analyzers;\s*buildtransitive</IncludeAssets>
                                      </PackageReference>
                                      """).SetName("Reference SourceLink");

        yield return new TestCaseData("""
                                      <PackageReference\s*Include="MinVer"\s*Version="\d+(\.\d+)+">
                                          <PrivateAssets>all</PrivateAssets>
                                          <IncludeAssets>runtime;\s*build;\s*native;\s*contentfiles;\s*analyzers;\s*buildtransitive</IncludeAssets>
                                      </PackageReference>
                                      """).SetName("Reference MinVer");

        yield return new TestCaseData("""
                                      <ItemGroup\s+Condition="'\$\(Configuration\)'\s*==\s*'Release'"\s*>
                                          <None\s+Include="(\.\.\\[a-zA-Z\.]+\\)?README\.md"\s+Pack="true"\s+PackagePath="\$\(PackageReadmeFile\)"\s*/>
                                          <None\s+Include="\.\.\\\.\.\\LICENSE\.md"\s+Pack="true"\s+PackagePath="\$\(PackageLicenseFile\)"\s*/>
                                      </ItemGroup>
                                      """).SetName("Include README.md and LICENSE.md");
    }

    private static List<string> StringToLines(string input)
    {
        var result = new List<string>();
        using var reader = new StringReader(input.Trim('\t', '\n'));
        while (reader.ReadLine() is { } line)
            result.Add(line);

        return result;
    }

    [Test]
    [TestCaseSource(nameof(RequiredSections))]
    public void ProjectFile_ContentsSection(string section)
    {
        List<string> lines = StringToLines(section);

        List<string> expected = lines[1..^1];
        Outdent(expected);

        for (var sectionStart = 0; sectionStart < _projectFile.Value.Count; sectionStart++)
        {
            if (!Regex.IsMatch(_projectFile.Value[sectionStart], lines[0]))
                continue;

            int sectionEnd = _projectFile.Value.FindIndex(sectionStart + 1, line => Regex.IsMatch(line, lines[^1]));
            if (sectionEnd < 0)
                break;

            if (sectionEnd - sectionStart + 1 != lines.Count)
                continue;

            var contents = _projectFile.Value.Skip(sectionStart + 1).Take(sectionEnd - sectionStart - 1).ToList();
            Outdent(contents);

            var isMatch = true;
            for (var index = 0; index < contents.Count; index++)
                isMatch = isMatch && Regex.IsMatch(contents[index], expected[index], RegexOptions.IgnoreCase);

            if (isMatch)
                Assert.Pass();
        }

        Assert.Fail();
    }

    private static void Outdent(List<string> lines)
    {
        while (lines.All(l => l.Length > 1 && l.StartsWith(" ")))
        {
            for (var index = 0; index < lines.Count; index++)
                lines[index] = lines[index][1..];
        }
    }
}