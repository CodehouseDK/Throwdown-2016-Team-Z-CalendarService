using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TeamZ.CalendarService.Models;

namespace TeamZ.CalendarService.Services
{
    public interface IScheduedUpdateService
    {
        void Start();
    }

    public class ScheduledUpdateService : IScheduedUpdateService
    {
        private readonly IExchangeService _exchangeService;
        private readonly INotificationService _notificationService;

        private Timer _timer;

        public ScheduledUpdateService(IExchangeService exchangeService, INotificationService notificationService)
        {
            _exchangeService = exchangeService;
            _notificationService = notificationService;
        }

        public void Start()
        {
            _timer = new Timer(TimerExpired, null, new TimeSpan(0, 10, 0), TimeSpan.Zero);
        }

        private void TimerExpired(object state)
        {
            var task = CreateModel();
            Task.WaitAll(task);
            var model = task.Result;
            _notificationService.Send(model);
        }

        private async Task<VacationStateModel> CreateModel()
        {
            var entities = await _exchangeService.OnVacation(DateTime.Now);
            var model = new VacationStateModel
            {
                Entries = entities.ToArray()
            };

            return model;
        }
    }
}
