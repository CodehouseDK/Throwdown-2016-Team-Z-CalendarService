using System;
using System.Linq;
using Microsoft.AspNet.Mvc;
using StackExchange.Redis;
using TeamZ.CalendarService.Services;

namespace TeamZ.CalendarService.Controllers
{
    public class FakerController : Controller
    {
        private readonly IPersonUpdateService _personUpdateService;
        private readonly IScheduedUpdateService _scheduler;

        public FakerController(IPersonUpdateService personUpdateService, IScheduedUpdateService scheduler)
        {
            _personUpdateService = personUpdateService;
            _scheduler = scheduler;
        }

        public IActionResult Index()
        {
            var nextUser = CalendarFolderMap.Ids.Keys.ToList().OrderBy(key => Guid.NewGuid()).First();
            _personUpdateService.Show(nextUser);
            _scheduler.Trigger();

            return Content("Now showing " + nextUser);
        }
    }
}
