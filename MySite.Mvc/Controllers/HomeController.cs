using Microsoft.AspNetCore.Mvc;

namespace MySite.Mvc.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Route("hakkimizda")]
        public IActionResult Hakkimizda()
        {
            return View();
        }

        [Route("referanslar")]
        public IActionResult Referanslar()
        {
            return View();
        }

        [Route("iletisim")]
        public IActionResult Iletisim()
        {
            return View();
        }
    }
}
