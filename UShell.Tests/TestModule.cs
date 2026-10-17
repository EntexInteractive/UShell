using Microsoft.Extensions.DependencyInjection;
using UShell.Core.Interfaces;

namespace UShell.Tests
{
    public class TestModule : IServiceModule
    {
        public void RegisterServices(IServiceCollection services)
        {
            services.AddHostedService<TestService>();
        }
    }
}