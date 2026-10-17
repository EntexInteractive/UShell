// Copyright 2026 Entex Interactive

using Microsoft.Extensions.Hosting;
using UShell.Core.Utils;

namespace UShell.Tests
{
    public class TestService : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Terminal.PrintInfo($"Current time: {DateTime.Now.ToLongTimeString()}");
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}