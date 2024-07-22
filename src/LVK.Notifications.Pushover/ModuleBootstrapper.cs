using LVK.Core.Bootstrapping;
using LVK.Events;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LVK.Notifications.Pushover;

public class ModuleBootstrapper : IModuleBootstrapper
{
    public void Bootstrap(IHostBootstrapper bootstrapper, IHostApplicationBuilder builder)
    {
        bootstrapper.Bootstrap(new Core.ModuleBootstrapper());
        bootstrapper.Bootstrap(new Networking.ModuleBootstrapper());
        bootstrapper.Bootstrap(new Events.ModuleBootstrapper());

        builder.Services.AddSingleton<IEventSubscriber<PushoverNotification>, PushoverNotificationSubscriber>();
        builder.Services.Configure<PushoverNotificationOptions>(builder.Configuration.GetSection("Notifications:Pushover"));
    }
}