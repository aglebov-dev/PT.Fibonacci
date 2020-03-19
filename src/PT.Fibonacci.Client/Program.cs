using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PT.Fibonacci.Client.Clients;
using PT.Fibonacci.Client.Job;
using PT.Fibonacci.DataAccess.DI;
using PT.Fibonacci.Logic.DI;

namespace PT.Fibonacci.Client
{
    public class Program
    {
        private static CancellationTokenSource _cts;

        public static void Main(string[] args)
        {
            _cts = new CancellationTokenSource();

            var configuration = CreateConfiguration(args);
            var seviceCollection = ConfigureServices(configuration);

            var serviceProvider = seviceCollection.BuildServiceProvider();
            var job = serviceProvider.GetService<ProcessesJob>();

            job.StartAsync(CancellationToken.None)
                .GetAwaiter()
                .GetResult();
        }
        private static IServiceCollection ConfigureServices(IConfiguration configuration)
        {
            Console.CancelKeyPress += Cancel;

            var services = new ServiceCollection();

            return services
                .AddSingleton(_cts)
                .AddSingleton(configuration)
                .AddSingleton<ProcessesJob>()
                .AddSingleton<FibonacciApiClient>()
                .AddLogging(ConfigureLogging)
                .LogicConfigure()
                .DataAccessConfigure(configuration)
                ;
        }

        private static void Cancel(object sender, ConsoleCancelEventArgs e)
        {
            Console.CancelKeyPress -= Cancel;
            _cts.Cancel();
        }

        private static IConfiguration CreateConfiguration(string[] args)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "application.json");
            var builder = new ConfigurationBuilder()
                .AddJsonFile(path)
                .AddCommandLine(args);

            DebugConfiguration(builder);

            return builder.Build();
        }

        private static void ConfigureLogging(ILoggingBuilder builder)
        {
            builder.ClearProviders();
            builder.AddConsole();
        }

        [Conditional("DEBUG")]
        private static void DebugConfiguration(IConfigurationBuilder builder)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "application.Development.json");
            builder.AddJsonFile(path);
        }
    }
}
