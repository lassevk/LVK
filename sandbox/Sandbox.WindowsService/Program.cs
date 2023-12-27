using LVK.Core.App.WindowsService;
using LVK.Core.Bootstrapping;

await App.Instance.RunAsWindowsService(args, new ApplicationBootstrapper());