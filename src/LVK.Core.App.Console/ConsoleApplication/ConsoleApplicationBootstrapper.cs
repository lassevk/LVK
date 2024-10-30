﻿using System.Text;

using LVK.Core.Bootstrapping;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LVK.Core.App.Console.ConsoleApplication;

internal class ConsoleApplicationBootstrapper : IApplicationBootstrapper<HostApplicationBuilder, IHost>
{
    public void Bootstrap(IHostBootstrapper<HostApplicationBuilder, IHost> bootstrapper, HostApplicationBuilder builder)
    {
        System.Console.OutputEncoding = Encoding.UTF8;

        bootstrapper.Bootstrap(new ModuleBootstrapper());

        builder.Services.AddHostedService<MainEntrypointRunner>();
    }
}