using LVK.Core.App.WindowsService;
using LVK.Core.Bootstrapping;

using Sandbox.WindowsService;

await App.Instance.RunAsWindowsService(args, new ApplicationBootstrapper());