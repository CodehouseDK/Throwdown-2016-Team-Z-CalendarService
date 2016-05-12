using Microsoft.AspNet.Mvc;
using TeamZ.Models;
using TeamZ.Services;

namespace TeamZ.Controllers
{
    public class HomeController : Controller
    {
        INameService _nameService;

        public HomeController(INameService nameService)
        {
            _nameService = nameService;
        }

        public IActionResult Index()
        {
            var model = new HomeModel(_nameService.GetName());
            return View(model);
        }
    }
}