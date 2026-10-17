using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Spectre.Console.Cli;
using UShell.Core;

public sealed class RunCommand : AsyncCommand<RunSettings>
{
    public override async Task<int> ExecuteAsync(CommandContext context, RunSettings settings, CancellationToken cancellationToken)
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();
        if (OperatingSystem.IsWindows()) builder.Services.AddWindowsService();
        if (OperatingSystem.IsLinux()) builder.Services.AddSystemd();

        ModuleManager.RegisterServices(builder.Services);

        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog();

        using IHost host = builder.Build();
        await host.RunAsync(cancellationToken);
        return 0;
    }
}

public sealed class RunSettings : CommandSettings
{
}