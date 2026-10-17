// Copyright 2026 Entex Interactive

using System.Reflection;
using UShell.Core;
using UShell.Core.Utils;

namespace UShell.Cli
{
    public class ListCommand : Command
    {
        public override int Execute(CommandContext context, CancellationToken cancellationToken)
        {
            AssemblyName[] modules = ModuleManager.Modules.Select(m => m.GetName()).ToArray();
            int width = modules.Max(e => e.Name?.Length ?? 0) + 1;

            foreach (AssemblyName module in modules)
            {
                if (string.IsNullOrEmpty(module.Name)) continue;
                Terminal.WriteLine($"{module.Name.PadRight(width)} v{module.Version}");
            }
            
            return 0;
        }
    }
}