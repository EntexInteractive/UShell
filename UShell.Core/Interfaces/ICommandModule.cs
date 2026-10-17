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
        /// <param name="rootCommand">The <see cref="System.CommandLine"/> root command.</param>
        void Register(Command rootCommand);
    }
}
