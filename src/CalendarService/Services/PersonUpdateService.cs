using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TeamZ.CalendarService.Models;

namespace TeamZ.CalendarService.Services
{
    public interface IPersonUpdateService
    {
        Task Show(string username);
    }
    public class PersonUpdateService : IPersonUpdateService
    {
        private readonly IExchangeService _exchangeService;
        private readonly INotificationService _notificationService;
        private readonly ILogger _logger;

        private Timer _timer;

        public PersonUpdateService(IExchangeService exchangeService, INotificationService notificationService, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("PersonUpdateService");
            _exchangeService = exchangeService;
            _notificationService = notificationService;
        }

        public async Task Show(string username)
        {
            _logger.Log(LogLevel.Information, 0, username, null, (state,exc) => "Will now show: " + string.IsNullOrEmpty(username));

            if (_timer != null)
            {
                _timer.Dispose();
                _timer = null;
            }

            var data = await _exchangeService.GetAppointments(username);
            var model = new PersonStateModel
            {
                CurrentUsername = username,
                Entries = data.ToArray()
            };
            await _notificationService.Send(model);

            _timer = new Timer(TimerExpired, null, new TimeSpan(0, 0, 15), TimeSpan.Zero);
        }

        private void TimerExpired(object state)
        {
            _timer.Dispose();
            _timer = null;
            _notificationService.Send(string.Empty);
        }

    }
}
