using Spectre.Console.Cli;

namespace UShell.Core.Interfaces
{
    /// <summary>
    /// Defines a module using <see cref="System.CommandLine"/>.
    /// </summary>
    public interface ICommandModule
    {
        /// <summary>
        /// Registers the module with UShell.
        /// </summary>
        /// <param name="config">The <see cref="IConfigurator"/> config.</param>
        void Register(IConfigurator config);
    }
}
