using PT.Fibonacci.Logic.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace PT.Fibonacci.Logic.DI
{
    public static class LogicConfiguration
    {
        public static IServiceCollection LogicConfigure(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IFibonacciService, FibonacciService>();
        }
    }
}
