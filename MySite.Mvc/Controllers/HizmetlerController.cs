using Microsoft.AspNetCore.Mvc;

namespace MySite.Mvc.Controllers
{
    public class HizmetlerController : Controller
    {
        [Route("hizmetler/ozel-proje-tasarim")]
        public IActionResult OzelProjeTasarim()
        {
            return View();
        }

        [Route("hizmetler/montaj-de-montaj")]
        public IActionResult MontajDeMontaj()
        {
            return View();
        }

        [Route("hizmetler/tadilat-tamirat")]
        public IActionResult TadilatTamirat()
        {
            return View();
        }       
    }
}
