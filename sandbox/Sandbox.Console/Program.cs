using LVK.Extensions.Bootstrapping.Console;

using Microsoft.Extensions.Hosting;

await HostEx.CreateApplication<Sandbox.Console.ModuleBootstrapper>(args).RunAsync();