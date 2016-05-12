using System;
using System.Linq;
using System.Threading.Tasks;
using CalendarService.Services;
using Microsoft.AspNet.Mvc;

namespace CalendarService.Controllers
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
            var text = string.Join(Environment.NewLine, result.Select(x => x.ToString()));
            return Content(text);

        }
    }
}
