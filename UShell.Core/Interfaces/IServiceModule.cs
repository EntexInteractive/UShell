// Copyright 2026 Entex Interactive

using Microsoft.Extensions.DependencyInjection;

namespace UShell.Core.Interfaces
{
    /// <summary>
    /// Defines a module using <see cref="Microsoft.Extensions.Hosting"/>.
    /// </summary>
    public interface IServiceModule
    {
        void RegisterServices(IServiceCollection services);
    }
}