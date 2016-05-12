using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using TeamZ.CalendarService.Models;
using TeamZ.CalendarService.Services;

namespace TeamZ.CalendarService.Controllers
{
    public class CalendarController : Controller
    {
        private readonly IExchangeService _exchangeService;

        public CalendarController(IExchangeService exchangeService)
        {
            _exchangeService = exchangeService;
        }

        public async Task<IActionResult> Coming(string id)
        {
            var result = await _exchangeService.GetAppointments(id);
            return Json(result);
        }

        public async Task<IActionResult> Vacation()
        {
            var result = await _exchangeService.OnVacation(DateTime.Now);
            var model = new VacationStateModel
            {
                Entries = result.ToArray()
            };
            return Json(model);
        }
    }
}
