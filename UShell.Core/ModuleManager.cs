using System.Reflection;
using System.Runtime.Loader;
using UShell.Core.Interfaces;

namespace UShell.Core.Modules
{
    public static class ModuleManager
    {
        private static List<Assembly> _loaded = new List<Assembly>();

        public static Assembly[] Modules => _loaded.OrderBy(e => e.GetName().Name).ToArray();

        public static Assembly[] LoadModules()
        {
            foreach (string dll in Directory.GetFiles(AppContext.BaseDirectory, "UShell*.dll"))
            {
                Assembly assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(dll);
                Log.Debug($"Loaded module: {assembly.GetName().Name}");
                _loaded.Add(assembly);
            }

            return _loaded.ToArray();
        }

        public static void RegisterModules(RootCommand root)
        {
            foreach (Assembly assembly in LoadModules())
            {
                IEnumerable<Type> types = assembly.GetTypes().Where(t => typeof(ICommandModule).IsAssignableFrom(t) && !t.IsAbstract);
                foreach (var type in types)
                {
                    Log.Information($"Registered module: {type.Name}");
                    ICommandModule commandModule = (ICommandModule)Activator.CreateInstance(type)!;
                    commandModule.Register(root);
                }
            }
        }
    }
}
