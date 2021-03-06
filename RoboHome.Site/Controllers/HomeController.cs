using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RoboHome.Models;
using RoboHome.Services;
using Microsoft.Extensions.Options;

namespace RoboHome.Controllers
{
    public class HomeController: Controller
    {

        public HomeController() {
        }
        public ActionResult Index(HomeViewModel model = null)
        {
            if (model == null) model = new HomeViewModel();
            return View(model);
        }
    }
}