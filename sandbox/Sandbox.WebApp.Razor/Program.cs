using LVK.Core.App.Web;
using LVK.Core.Bootstrapping;

using Sandbox.WebApp.Razor;

await App.Instance.RunAsWebApplication(args, new ApplicationBootstrapper());