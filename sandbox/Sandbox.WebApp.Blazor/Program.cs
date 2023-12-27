using LVK.Core.App.Web;
using LVK.Core.Bootstrapping;

using Sandbox.WebApp.Blazor;

await App.Instance.RunAsWebApplication(args, new ApplicationBootstrapper());