using LVK.Core.Bootstrapping;
using LVK.Core.App.Console;

using Sandbox.ConsoleApp;

await App.Instance.RunAsConsole(args, new ModuleBootstrapper());