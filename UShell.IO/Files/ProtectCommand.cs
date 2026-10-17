// Copyright 2026 Entex Interactive

using System.CommandLine;
using System.Diagnostics;
using UShell.Core.Utils;

namespace UShell.IO.Files
{
    public class ProtectCommand : Command
    {
        private readonly Option<string> _pathOption = new("path", ["-p"]) { Description = "Path to a file or directory." };
        private readonly Option<bool> _recursiveOption = new("--recursive", ["-R"]) { Description = "Recursively" };
        private readonly Option<bool> _writeableOption = new("--writeable") { Description = "Marks files as writeable." };
        private readonly Option<bool> _hiddenOption = new("--hidden") { Description = "Marks files as hidden." };
        
        public ProtectCommand() : base("protect", "Protects files by marking them as read-only.")
        {
            this.Add(_pathOption);
            this.Add(_recursiveOption);
            this.Add(_writeableOption);
            //this.Add(_hiddenOption);
            this.SetAction(HandleCommand);
        }

        private void HandleCommand(ParseResult args)
        {
            string path = args.GetValue(_pathOption) ?? Directory.GetCurrentDirectory();
            bool recursive = args.GetValue(_recursiveOption);
            bool writeable = args.GetValue(_writeableOption);
            bool hidden = args.GetValue(_hiddenOption);
            
            Stopwatch sw = Stopwatch.StartNew();
            string[] files = Directory.GetFiles(path, "*.*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            for (int i = 0; i < files.Length; i++)
            {
                Terminal.Progress("Processing", i, files.Length, sw.Elapsed);
                FileAttributes attributes = File.GetAttributes(files[i]);
                if (writeable)
                {
                    attributes &= ~FileAttributes.ReadOnly;
                }
                else
                {
                    attributes |= FileAttributes.ReadOnly;
                }
                
                File.SetAttributes(files[i], attributes);
            }
            
            Terminal.PrintSuccess($"Successfully marked {files.Length:N0} files as {(writeable ? "writeable" : "readable")}.");
        }
    }
}