using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PT.Fibonacci.DataAccess.Contracts;

namespace PT.Fibonacci.DataAccess.DI
{
    public static class DataAccessConfiguration
    {
        public static IServiceCollection DataAccessConfigure(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            return serviceCollection
                .Configure<RepositorySettings>(configuration.GetSection("Repository"), x => x.BindNonPublicProperties = true)
                .AddSingleton<IRepository, NatsRepository>();
        }
    }
}
