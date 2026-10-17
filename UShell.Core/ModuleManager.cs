using System.Reflection;
using System.Runtime.Loader;
using Microsoft.Extensions.DependencyInjection;
using UShell.Core.Interfaces;

namespace UShell.Core
{
    /// <summary>
    /// Represents module management. This class cannot be inherited.
    /// </summary>
    public static class ModuleManager
    {
        private static HashSet<Assembly> _loaded = new HashSet<Assembly>();

        /// <summary>
        /// Gets the currently active modules.
        /// </summary>
        public static Assembly[] Modules => _loaded.OrderBy(e => e.GetName().Name).ToArray();

        /// <summary>
        /// Scans the application directory and loads any available modules.
        /// </summary>
        /// <returns>An array of currently active modules.</returns>
        public static Assembly[] LoadModules()
        {
            foreach (string dll in Directory.GetFiles(AppContext.BaseDirectory, "UShell*.dll", SearchOption.AllDirectories))
            {
                try
                {
                    Assembly assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(dll);
                    _loaded.Add(assembly);
                }
                catch (BadImageFormatException)
                {
                    Log.Warning("Failed to load assembly: {TypeName}", dll);
                }
            }

            return _loaded.ToArray();
        }

        /// <summary>
        /// Registers the currently loaded modules with <see cref="System.CommandLine"/>.
        /// </summary>
        /// <param name="root"></param>
        public static void RegisterCommands(RootCommand root)
        {
            foreach (Assembly assembly in LoadModules())
            {
                IEnumerable<Type> types = assembly.GetTypes().Where(t => typeof(ICommandModule).IsAssignableFrom(t) && !t.IsAbstract);
                foreach (Type type in types)
                {
                    Log.Information("Registered module: {TypeName}", type.Name);
                    ICommandModule commandModule = (ICommandModule)Activator.CreateInstance(type)!;
                    commandModule.Register(root);
                }
            }
        }

        public static void RegisterServices(IServiceCollection services)
        {
            foreach (Assembly assembly in LoadModules())
            {
                IEnumerable<Type> types = assembly.GetTypes().Where(t => typeof(IServiceModule).IsAssignableFrom(t) && !t.IsAbstract);
                foreach (Type type in types)
                {
                    Log.Information("Registered module: {TypeName}", type.Name);
                    IServiceModule serviceModule = (IServiceModule)Activator.CreateInstance(type)!;
                    serviceModule.RegisterServices(services);
                }
            }
        }
    }
}
