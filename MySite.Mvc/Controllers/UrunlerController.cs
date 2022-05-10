using Microsoft.AspNetCore.Mvc;

namespace MySite.Mvc.Controllers
{
    public class UrunlerController : Controller
    {
        [Route("urunler/cam-balkon")]
        public IActionResult CamBalkon()
        {
            return View();
        }

        [Route("urunler/sineklik")]
        public IActionResult Sineklik()
        {
            return View();
        }

        [Route("urunler/stor-sineklik")]
        public IActionResult StorSineklik()
        {
            return View();
        }

        [Route("urunler/pvc-dograma")]
        public IActionResult PvcDograma()
        {
            return View();
        }

        [Route("urunler/dusakabin")]
        public IActionResult Dusakabin()
        {
            return View();
        }

        [Route("urunler/kupeste")]
        public IActionResult Kupeste()
        {
            return View();
        }
    }
}
