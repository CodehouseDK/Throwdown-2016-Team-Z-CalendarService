using Microsoft.AspNet.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TeamZ.CalendarService.Services;

namespace TeamZ.CalendarService
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables("Exchange")
                .Build();
            services.AddInstance<IConfiguration>(configuration);

            services.AddMvc();
            services.AddSingleton<IExchangeService, ExchangeService>();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            
            app.UseIISPlatformHandler();
            
            app.UseDeveloperExceptionPage();

            app.UseMvcWithDefaultRoute();
            
            app.UseStaticFiles();
            
        }
    }
}