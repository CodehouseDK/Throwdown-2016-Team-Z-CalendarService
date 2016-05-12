using System;
using System.Linq;
using Microsoft.AspNet.Mvc;
using TeamZ.CalendarService.Services;

namespace TeamZ.CalendarService.Controllers
{
    public class FakerController : Controller
    {
        private readonly IPersonUpdateService _personUpdateService;

        public FakerController(IPersonUpdateService personUpdateService)
        {
            _personUpdateService = personUpdateService;
        }

        public IActionResult Index()
        {
            var nextUser = CalendarFolderMap.Ids.Keys.ToList().OrderBy(key => Guid.NewGuid()).First();
            _personUpdateService.Show(nextUser);

            return Content("Now showing " + nextUser);
        }
    }
}
