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
                .AddEnvironmentVariables("Redis")
                .Build();
            services.AddInstance<IConfiguration>(configuration);

            services.AddMvc();
            services.AddSingleton<IExchangeService, ExchangeService>();
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