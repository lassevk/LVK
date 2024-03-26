using LVK.Core.Bootstrapping;
using LVK.Typed.Rules;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LVK.Typed;

public class ModuleBootstrapper : IModuleBootstrapper
{
    public void Bootstrap(IHostBootstrapper bootstrapper, IHostApplicationBuilder builder)
    {
        bootstrapper.Bootstrap(new LVK.Core.ModuleBootstrapper());

        builder.Services.AddSingleton<ITypeHelper, TypeHelper>();

        builder.Services.AddSingleton<ITypeNameRule, CSharpKeywordTypeNameRule>();
        builder.Services.AddSingleton<ITypeNameRule, GenericTypeNameRule>();
        builder.Services.AddSingleton<ITypeNameRule, NormalTypeNameRule>();
        builder.Services.AddSingleton<ITypeNameRule, NullableTypeNameRule>();
    }
}