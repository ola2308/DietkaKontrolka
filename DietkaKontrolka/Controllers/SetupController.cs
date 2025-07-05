using Microsoft.AspNetCore.Mvc;

namespace DietkaKontrolka.Controllers
{
    public class SetupController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
