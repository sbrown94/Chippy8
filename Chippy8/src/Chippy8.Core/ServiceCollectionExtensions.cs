using Microsoft.Extensions.DependencyInjection;

namespace Chippy8.Core
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddChippy8Dependencies(this IServiceCollection services)
        {
            services.AddSingleton<ICounter, Counter>();
            services.AddSingleton<IInput, Input>();
            services.AddSingleton<IStack, Stack>();
            services.AddSingleton<ITimers, Timers>();
            services.AddSingleton<IMemory, Memory>();
            services.AddSingleton<IRegisters, Registers>();
            services.AddSingleton<IScreen, Screen>();
            services.AddSingleton<Chip8, Chip8>();

            return services;
        }
    }
}
