using System.CommandLine.Help;
using System.Reflection;
using UShell.Core.Modules;
using UShell.Core.Utils;

namespace UShell.Cli
{
    internal static class Program
    {
        private static Option<bool> _versionOption = new Option<bool>("--version", ["-v"]);

        static async Task Main(string[] args)
        {
            AssemblyName assembly = Assembly.GetExecutingAssembly().GetName();
            Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().CreateLogger();

            Log.Information($"{assembly.Name} [Version {assembly.Version}]");
            RootCommand rootCommand = new("UShell - A universal cross-platform system shell with expandable functionality.");
            rootCommand.Options.RemoveAt(1);

            ModuleManager.RegisterModules(rootCommand);
            rootCommand.SetAction(HandleRootCommand);
            rootCommand.Add(_versionOption);

            try
            {
                await rootCommand.Parse(args).InvokeAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
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