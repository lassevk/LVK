using LVK.Core.Bootstrapping;
using LVK.ObjectDumper.Rules;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LVK.ObjectDumper;

public class ModuleBootstrapper : IModuleBootstrapper
{
    public void Bootstrap(IHostBootstrapper bootstrapper, IHostApplicationBuilder builder)
    {
        bootstrapper.Bootstrap(new LVK.Core.ModuleBootstrapper());
        bootstrapper.Bootstrap(new LVK.Typed.ModuleBootstrapper());

        builder.Services.AddSingleton<IObjectDumper, ObjectDumper>();

        builder.Services.AddSingleton<IObjectDumperRule, StringObjectDumperRule>();
        builder.Services.AddSingleton<IObjectDumperRule, IntegerObjectDumperRule>();
        builder.Services.AddSingleton<IObjectDumperRule, BooleanObjectDumperRule>();
        builder.Services.AddSingleton<IObjectDumperRule, NumericObjectDumperRule>();
        builder.Services.AddSingleton<IObjectDumperRule, IntPtrObjectDumperRule>();
        builder.Services.AddSingleton<IObjectDumperRule, PointerObjectDumperRule>();
        builder.Services.AddSingleton<IObjectDumperRule, NonRecursiveTypesObjectDumperRule>();
        builder.Services.AddSingleton<IObjectDumperRule, TypeObjectDumperRule>();
        builder.Services.AddSingleton<IObjectDumperRule, AssemblyObjectDumperRule>();
        builder.Services.AddSingleton<IObjectDumperRule, GuidObjectDumperRule>();
        builder.Services.AddSingleton<IObjectDumperRule, EnumerableObjectDumperRule>();
        builder.Services.AddSingleton<IObjectDumperRule, DateOrTimeTypesObjectDumperRule>();
        builder.Services.AddSingleton<IObjectDumperRule, EnumerableObjectDumperRule>();
        builder.Services.AddSingleton<IObjectDumperRule, EnumObjectDumperRule>();
        builder.Services.AddSingleton<IObjectDumperRule, ByteArrayObjectDumperRule>();
        builder.Services.AddSingleton<IObjectDumperRule, ExceptionObjectDumperRule>();
    }
}