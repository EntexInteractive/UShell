// Copyright 2026 Entex Interactive

using System.CommandLine;

namespace UShell.IO.Files
{
    public class ZipCommand : Command
    {
        public ZipCommand() : base("zip", "Zips files in a directory.")
        {
        }
    }
}