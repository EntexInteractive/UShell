using System.CommandLine;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UShell.Core.Interfaces;

namespace UShell.IO
{
    /// <summary>
    /// Represents an IO module.
    /// </summary>
    public class IOModule : ICommandModule, IServiceModule
    {
        public void Register(Command rootCommand)
        {
            IEnumerable<Type> types = Assembly.GetExecutingAssembly().GetTypes().Where(t => typeof(Command).IsAssignableFrom(t) && !t.IsAbstract && t != typeof(Command));
            foreach (Type type in types) rootCommand.Add((Command)Activator.CreateInstance(type)!);
        }

        public void RegisterServices(IServiceCollection services)
        {
            IEnumerable<Type> types = Assembly.GetExecutingAssembly().GetTypes().Where(t => typeof(IHostedService).IsAssignableFrom(t) && !t.IsAbstract && t != typeof(IHostedService));
            foreach (Type type in types) services.AddSingleton(typeof(IHostedService), type);
        }
    }
}