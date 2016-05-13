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
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables("Exchange")
                .AddEnvironmentVariables("REDIS_")
                .Build();
            services.AddInstance<IConfiguration>(configuration);

            services.AddLogging();
            services.AddMvc();

            if (string.IsNullOrEmpty(configuration["ExchangeProxy"]))
            {
                services.AddSingleton<IExchangeService, ExchangeService>();
            }
            else
            {
                services.AddSingleton<IExchangeService, ExchangeProxyService>();
            }

            services.AddSingleton<INotificationService, WebSocketsService>();
            services.AddSingleton<IPersonUpdateService, PersonUpdateService>();
            services.AddSingleton<IScheduedUpdateService, ScheduledUpdateService>();
            services.AddSingleton<RedisSubscriberService>();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            app.UseIISPlatformHandler()
                .UseDeveloperExceptionPage()
                .UseWebSockets()
                .UseWebSocketsServer()
                .UseMvcWithDefaultRoute()
                .UseStaticFiles()
                ;

            app.ApplicationServices.GetService<IScheduedUpdateService>().Start();
            app.ApplicationServices.GetService<RedisSubscriberService>()
                .Subscribe(msg => app.ApplicationServices.GetService<IPersonUpdateService>().Show(msg));
        }
    }
}