using Microsoft.AspNetCore.Mvc;

namespace FlyLib.API.Controllers
{
    public class ProvincesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
