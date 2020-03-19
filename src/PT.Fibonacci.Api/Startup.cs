using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PT.Fibonacci.Api.Controllers;
using PT.Fibonacci.DataAccess.DI;
using PT.Fibonacci.Logic.DI;

namespace PT.Fibonacci.Api
{
    internal class Startup
    {
        private readonly IConfiguration Configuration;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services
                .AddOptions()
                .LogicConfigure()
                .DataAccessConfigure(Configuration);
        }
    }
}
