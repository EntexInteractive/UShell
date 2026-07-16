// Copyright 2026 Entex Interactive

using System.Reflection;
using UShell.Core.Interfaces;

namespace UShell.Cli
{
    public sealed class SystemModule : ICommandModule
    {
        public void Register(Command rootCommand)
        {
            IEnumerable<Type> types = Assembly.GetExecutingAssembly().GetTypes().Where(t => typeof(Command).IsAssignableFrom(t) && !t.IsAbstract && t != typeof(Command));
            foreach (Type type in types) rootCommand.Add((Command)Activator.CreateInstance(type)!);
        }
    }
}