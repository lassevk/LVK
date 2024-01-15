using System.ComponentModel;
using System.Reflection;

namespace LVK.Core.App.Console.CommandLineInterface.Commands;

[Description("Shows version information for the application")]
internal class VersionCommand : ICommand
{
    public Task<int> RunAsync(CancellationToken cancellationToken)
    {
        var assembly = Assembly.GetEntryAssembly();
        if (assembly == null)
            throw new InvalidOperationException("No entry assembly registered, unable to evaluate application version");

        if (!ReportAssemblyVersion(assembly))
            System.Console.Error.WriteLine("Unable to identify version for this application");

        if (AllDependencies)
        {
            System.Console.WriteLine();
            ReportDependencies(assembly);
        }

        return Task.FromResult(0);
    }

    private void ReportDependencies(Assembly entryAssembly)
    {
        System.Console.WriteLine("Depends on:");
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            if (assembly == entryAssembly)
                continue;

            ReportAssemblyVersion(assembly);
        }
    }

    private static bool ReportAssemblyVersion(Assembly assembly)
    {
        AssemblyFileVersionAttribute? fileVersionAttribute = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>();
        if (fileVersionAttribute != null)
        {
            System.Console.WriteLine($"{assembly.GetName().Name}: {fileVersionAttribute.Version}");
            return true;
        }

        AssemblyVersionAttribute? versionAttribute = assembly.GetCustomAttribute<AssemblyVersionAttribute>();
        if (versionAttribute != null)
        {
            System.Console.WriteLine($"{assembly.GetName().Name}: {versionAttribute.Version}");
            return true;
        }

        return false;
    }

    [CommandLineOption("dependencies")]
    [CommandLineOption("d")]
    [Description("Also include all dependencies")]
    public bool AllDependencies { get; set; }
}