using Microsoft.AspNetCore.Mvc;
namespace RoboHome.Controllers
{
    public class HomeController: Controller
    {
        public HomeController()
        {

        }
        public IActionResult Index()
        {
            return View();
        }
    }
}