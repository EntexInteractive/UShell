// Copyright 2026 Entex Interactive

using System.Reflection;
using Spectre.Console.Cli;
using UShell.Core.Interfaces;

namespace UShell.Cli
{
    public sealed class SystemModule : ICommandModule
    {
        public void Register(IConfigurator config)
        {
            config.AddCommand<ClearCommand>("clear").WithDescription("Clears the console");
            config.AddCommand<ClearCommand>("cls").WithDescription("Clears the console");
            
            config.AddCommand<ListCommand>("list").WithDescription("Lists all loaded modules.");
            config.AddCommand<InfoCommand>("info").WithDescription("Displays the application info.");
            
            config.AddCommand<RunCommand>("run");
        }
    }
}