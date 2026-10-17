// Copyright 2026 Entex Interactive

using System.ComponentModel;
using System.Diagnostics;
using Spectre.Console;
using Spectre.Console.Cli;
using UShell.Core.Utils;

namespace UShell.IO.Files
{
    public sealed class ProtectCommand : Command<ProtectSettings>
    {
        public override int Execute(CommandContext context, ProtectSettings settings, CancellationToken cancellationToken)
        {
            string path = settings.Path ?? Directory.GetCurrentDirectory();

            if (!File.Exists(path) && !Directory.Exists(path))
            {
                AnsiConsole.MarkupLine($"[red]Path does not exist:[/] {Markup.Escape(path)}");
                return 1;
            }

            Stopwatch sw = Stopwatch.StartNew();
            string[] files = File.Exists(path) ? [path] : Directory.GetFiles(path, "*", settings.Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

            for (int i = 0; i < files.Length; i++)
            {
                Terminal.Progress("Processing", i, files.Length, sw.Elapsed);

                FileAttributes attributes = File.GetAttributes(files[i]);

                if (settings.Writeable)
                    attributes &= ~FileAttributes.ReadOnly;
                else
                    attributes |= FileAttributes.ReadOnly;

                if (settings.Hidden)
                    attributes |= FileAttributes.Hidden;

                File.SetAttributes(files[i], attributes);
            }

            sw.Stop();

            Terminal.PrintSuccess($"Successfully made {files.Length:N0} files {(settings.Writeable ? "writeable" : "read-only")}.");
            return 0;
        }
    }

    public sealed class ProtectSettings : CommandSettings
    {
        [CommandOption("-p|--path <PATH>")]
        [Description("Path to a file or directory.")]
        public string? Path { get; set; }

        [CommandOption("-R|--recursive")]
        [Description("Process directories recursively.")]
        public bool Recursive { get; set; }

        [CommandOption("--writeable")]
        [Description("Marks files as writeable.")]
        public bool Writeable { get; set; }

        [CommandOption("--hidden")]
        [Description("Marks files as hidden.")]
        public bool Hidden { get; set; }
    }   
}
