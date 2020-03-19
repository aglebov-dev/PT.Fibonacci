using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System.Diagnostics;
using System.IO;

namespace PT.Fibonacci.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = CreateWebHostBuilder(args);
            builder.Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var configuration = CreateConfiguration(args);

            var url = configuration.GetValue<string>("Url");

            return WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging(ConfigureLogging)
                .UseConfiguration(configuration)
                .UseUrls(url)
                .UseStartup<Startup>();
        }

        private static void ConfigureLogging(ILoggingBuilder builder)
        {
            builder.ClearProviders();
            builder.AddNLog();
        }

        private static IConfiguration CreateConfiguration(string[] args)
        {
            var configurationPath = Path.Combine(Directory.GetCurrentDirectory(), "application.json");
            var builder = new ConfigurationBuilder()
                .AddCommandLine(args)
                .AddJsonFile(configurationPath)
                .AddEnvironmentVariables();
                

            DebugConfiguration(builder);

            return builder.Build();
        }

        [Conditional("DEBUG")]
        private static void DebugConfiguration(IConfigurationBuilder builder)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "application.Development.json");
            builder.AddJsonFile(path);
        }
    }
}
