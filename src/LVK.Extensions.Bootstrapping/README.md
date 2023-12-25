# LVK.Extensions.Bootstrapping

This project is an opinionated extension to [Microsoft.Extensions.Hosting][meh].

The aim is to separate out host building and host initialization into separate files from the typical Program.cs wall
of code, and handle class libraries with services in a consistent manner.

The concept is that every project in your solution can have a "Module Bootstrapper" that is responsible for
registering services, and optionally initialize the host after it has been built.

This allows you to make all your service interfaces public, and all the service implementations internal in those
class libraries, which makes for easier separation of concerns. No code can reach into your class libraries and
yank out an implementation of a service, simply because they are internal and off limits.

## Installing the library

Use your favorite nuget package manager and install the package `LVK.Extensions.Bootstrapping`.

## Using the library in your application project

The typical line of code you would have in your Program.cs file will look like this:

    SomeHostBuilder
        .Create()
        .BootstrapAndBuild(new ModuleBootstrapper(), b => b.Build())
        .Run();

The exact lines of code varies with different types of hosts and application projects, so specific examples follows

### Console Applications

    await Host
        .CreateApplicationBuilder(args)
        .BootstrapAndBuild(new ModuleBootstrapper(), b => b.Build())
        .RunAsync();

### Web Application (Razor pages and Blazor server)

    await WebApplication
        .CreateBuilder(args)
        .BootstrapAndBuild(new ModuleBootstrapper(), b => b.Build())
        .RunAsync();

## ModuleBootstrapper

The `ModuleBootstrapper` classes mentioned is a class that implements one of the two the `IModuleBootstrapper`
interfaces. There are two because there is a generic one intended to be used in the main application project, and any
class libraries that is built specifically for that type of application, and a generic one that is to be used in all
class libraries that are general purpose.

This also means that you can publish class libraries internally using this library, without tying them directly to
any particular type of application.

The generic interface looks like this:

    public interface IModuleBootstrapper<TBuilder, THost>
    {
        void Bootstrap(IHostBootstrapper<TBuilder, THost> bootstrapper, TBuilder builder);
    }

and the non-generic interface looks like this:

    public interface IModuleBootstrapper
    {
        void Bootstrap(IHostBootstrapper bootstrapper, IHostApplicationBuilder builder);
    }

## Implementing IModuleBootstapper

Here is a typical module bootstrapper for an application:

    public class ModuleBootstrapper : IModuleBootstrapper<HostApplicationBuilder,IHost>
    {
        public void Bootstrap(IHostBootstrapper<HostApplicationBuilder, IHost> bootstrapper, HostApplicationBuilder builder)
        {
            bootstrapper.Bootstrap(new Sandbox.ClassLibrary.ModuleBootstrapper());
        }
    }

The call to `bootstrapper.Bootstrap` is used to handle dependencies. Here we indicate that the application is
depending on the module bootstrapper in the `Sandbox.ClassLibrary` project to bootstrap itself, by registering
services and whatnot.

The bootstrapper in that class library can look like this:

    public class ModuleBootstrapper : IModuleBootstrapper, IModuleInitializer<IHost>
    {
        public void Bootstrap(IHostBootstrapper bootstrapper, IHostApplicationBuilder builder)
        {
            builder.Services.AddSingleton<ISandboxService, SandboxService>();
        }
    
        public void Initialize(IHost host)
        {
            host.Services.GetRequiredService<ISandboxService>().SetName("Service has been initialized");
        }
    }

Here you can see that the bootstrapper registers one service. This service interface is public, but the actual
implementation is internal, and none of the application projects can thus access that class directly.

## IModuleInitializer

The `IModuleInitializer<THost>` interface is an optional interface that can be implemented on the module bootstrapper
type. If it is implemented, then after the host is built, but before `BootstrapAndBuild` returns the host back to the
main program code, all such initializers are invoked.

This allows you to take all the code normally written in projects such as web applications, code that typically looks
like `host.UseRouting();` and similar, can be put into a method in the bootstrapper itself.

## Auxiliary nuget packages

There are two auxiliary nuget packages available to make the experience of
writing Program.cs even simpler: `LVK.Extensions.Bootstrapping.Console`
and `LVK.Extensions.Bootstrapping.Web`.

They add `...Ex` versions of the application builder classes, that make the code
simpler and take away some of the code that will always be the same.

Here's two examples, one for a console application and one for a web application:

Console:

    await HostEx.CreateApplication<ModuleBootstrapper>(args).RunAsync();

Web:

    await WebApplicationEx.Create<ModuleBootstrapper>(args).RunAsync();

here you can see that the module bootstrapper is passed generically, and
the delegate has been handled internally. There are overloads for all
the variants of `Create...Builder´ methods on the corresponding `Host`
and `WebApplication` classes.

If you are required to pass parameters to the module bootstrappers,
there are non-generic overloads as well.

## Examples

This project has a few examples, a class library, a console application, a blazor application, and a razor page
application.

Take a look at their files to see how all this is used.

* Class Library
    * [ModuleBootstrapper](Sandbox.ClassLibrary/ModuleBootstrapper.cs)
* Console Application
    * [Program.cs](Sandbox.Console/Program.cs)
    * [ModuleBootstrapper.cs](Sandbox.Console/ModuleBootstrapper.cs)
* Blazor Application
    * [Program.cs](Sandbox.BlazorInteractiveServer/Program.cs)
    * [ModuleBootstrapper.cs](Sandbox.BlazorInteractiveServer/ModuleBootstrapper.cs)
* Razor Pages Application
    * [Program.cs](Sandbox.WebAppRazorPages/Program.cs)
    * [ModuleBootstrapper.cs](Sandbox.WebAppRazorPages/ModuleBootstrapper.cs)

  [meh]: https://www.nuget.org/packages/Microsoft.Extensions.Hosting
