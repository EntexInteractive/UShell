// Copyright 2026 Entex Interactive

using System.Reflection;
using System.Text.RegularExpressions;
using Entexinteractive.Extensions;
using Serilog;
using Serilog.Events;
using Spectre.Console;
using Spectre.Console.Cli;
using UShell.Core;
using UShell.Core.Utils;

namespace UShell.CLi
{
    internal static class Program
    {
        private static async Task<int> Main(string[] args)
        {
            AssemblyName assembly = Assembly.GetExecutingAssembly().GetName();

            // Bootstrap debug detection before configuring logging.
            Terminal.IsDebugging = args.Contains("-log", StringComparer.OrdinalIgnoreCase);
            LoggerConfiguration loggerConfig = new LoggerConfiguration().MinimumLevel.Is(Terminal.IsDebugging ? LogEventLevel.Debug : LogEventLevel.Information);
            if (Terminal.IsDebugging) loggerConfig.WriteTo.Console();

            Log.Logger = loggerConfig.CreateLogger();
            Log.Information("{AssemblyName} [Version {AssemblyVersion}]", assembly.Name, assembly.Version);
            Log.Debug("Running in: {CurrentDirectory}", Directory.GetCurrentDirectory());

            CommandApp app = new CommandApp();
            app.Configure(config =>
            {
                config.SetApplicationName("UShell");

                config.SetExceptionHandler((ex, resolver) =>
                {
                    Terminal.PrintError(ex);
                    Log.Error(ex, "Command execution failed");
                });

                ModuleManager.RegisterCommands(config);
            });

            try
            {
                if (args.Length > 0) return await app.RunAsync(args);

                while (true)
                {
                    AnsiConsole.Markup("[blue]UShell[/] [grey]>[/] ");

                    string? input = Console.ReadLine();

                    if (input is null || input.Equals("exit", StringComparison.OrdinalIgnoreCase)) break;
                    if (string.IsNullOrWhiteSpace(input)) continue;
                    
                    string[] commandArgs = SplitArguments(input);
                    
                    try
                    {
                        await app.RunAsync(commandArgs);
                    }
                    catch (OperationCanceledException)
                    {
                        AnsiConsole.MarkupLine("\n[yellow]Command cancelled.[/]");
                    }
                }

                return 0;
            }
            catch (Exception ex)
            {
                Terminal.PrintError(ex);
                throw;
            }
            finally
            {
                await Log.CloseAndFlushAsync();
            }
        }

        private static string[] SplitArguments(string input)
        {
            return Regex.Matches(input, @"[\""].+?[\""]|[^ ]+").Cast<Match>().Select(m => m.Value.Replace("\"", "")).ToArray();
        }
    }
}