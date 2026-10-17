// Copyright 2026 Entex Interactive

using Spectre.Console;

namespace UShell.Cli
{
    public class ClearCommand : Command
    {
        public override int Execute(CommandContext context, CancellationToken cancellationToken)
        {
            AnsiConsole.Clear();
            return 0;
        }
    }
}