using System.CommandLine.Help;
using System.Reflection;
using Serilog.Events;
using UShell.Core;
using UShell.Core.Utils;

namespace UShell.Cli
{
    internal static class Program
    {
        private static Option<bool> _versionOption = new("--version", ["-v"]);
        private static Option<bool> _debugOption = new("-log");

        static async Task Main(string[] args)
        {
            RootCommand rootCommand = new("UShell - A cross-platform modular command line.");
            rootCommand.Options.RemoveAt(1);
            rootCommand.Add(_versionOption);
            rootCommand.Add(_debugOption);
            
            ParseResult parseResult = rootCommand.Parse(args);
            Terminal.IsDebugging = parseResult.GetValue(_debugOption);
            
            AssemblyName assembly = Assembly.GetExecutingAssembly().GetName();
            LoggerConfiguration config = new LoggerConfiguration().MinimumLevel.Is(Terminal.IsDebugging ? LogEventLevel.Debug : LogEventLevel.Information);
            if (Terminal.IsDebugging) config.WriteTo.Console();
            Log.Logger = config.CreateLogger();

            Log.Information("{AssemblyName} [Version {AssemblyVersion}]", assembly.Name, assembly.Version);
            Log.Debug("Running in: {GetCurrentDirectory}", Directory.GetCurrentDirectory());

            ModuleManager.RegisterCommands(rootCommand);
            rootCommand.SetAction(HandleRootCommand);

            try
            {
                await rootCommand.Parse(args).InvokeAsync();
            }
            catch (Exception ex)
            {
                Terminal.PrintError(ex);
                throw;
            }
        }

        private static void HandleRootCommand(ParseResult result)
        {
            if (result.GetValue(_versionOption))
            {
                AssemblyName[] modules = ModuleManager.Modules.Select(m => m.GetName()).ToArray();
                int width = modules.Max(e => e.Name?.Length ?? 0) + 1;

                foreach (AssemblyName module in modules)
                {
                    if (string.IsNullOrEmpty(module.Name)) continue;
                    Terminal.WriteLine($"{module.Name.PadRight(width)} v{module.Version}");
                }
                
                return;
            }

            HelpAction helpAction = new HelpAction();
            helpAction.Invoke(result);
        }
    }
}