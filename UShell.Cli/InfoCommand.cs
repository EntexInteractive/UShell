// Copyright 2026 Entex Interactive

using System.Reflection;
using Spectre.Console;

namespace UShell.Cli
{
    public sealed class InfoCommand : Command
    {
        public override int Execute(CommandContext context, CancellationToken cancellationToken)
        {
            Assembly? assembly = Assembly.GetEntryAssembly();
            AssemblyName? name = assembly?.GetName();

            string version = name?.Version?.ToString() ?? "Unknown";

            AnsiConsole.Write(new FigletText("UShell").Color(Color.Cyan1));
            AnsiConsole.MarkupLine("[grey]A cross-platform modular command line.[/]");
            AnsiConsole.WriteLine();

            Table table = new Table().Border(TableBorder.Rounded).AddColumn("Property").AddColumn("Value");

            table.AddRow("Version", Markup.Escape(version));
            table.AddRow(".NET", Markup.Escape(Environment.Version.ToString()));
            table.AddRow("Platform", Markup.Escape(Environment.OSVersion.ToString()));
            table.AddRow("Working directory", Markup.Escape(Environment.CurrentDirectory));

            AnsiConsole.Write(table);

            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[grey]Run '--help' to see available commands.[/]");
            return 0;
        }
    }
}