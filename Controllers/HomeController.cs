using Microsoft.AspNetCore.Mvc;
using RoboHome.Models;
using RoboHome.Services;

namespace RoboHome.Controllers
{
    public class HomeController: Controller
    {
        private readonly IMqClient _messenger;
        public HomeController(IMqClient messenger)
        {
            this._messenger = messenger;
        }
        public IActionResult Index(HomeViewModel model = null)
        {
            if (model == null) model = new HomeViewModel();
            return View(model);
        }

        public IActionResult SendMessage(HomeViewModel model)
        {
            this._messenger.SendMessage(0,model.Message);
            return RedirectToAction("Index", model);
        }
    }
}