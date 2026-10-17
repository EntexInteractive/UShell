// Copyright 2026 Entex Interactive

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UShell.Core;

namespace UShell.Cli
{
    public class RunCommand : Command
    {
        public RunCommand() : base("run", "Runs a background service.")
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder();
            if (OperatingSystem.IsWindows()) builder.Services.AddWindowsService();
            if (OperatingSystem.IsLinux()) builder.Services.AddSystemd();
            
            ModuleManager.RegisterServices(builder.Services);
            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog();
            
            IHost host = builder.Build();
            SetAction((ParseResult _, CancellationToken token) => host.RunAsync(token));
        }
    }
}