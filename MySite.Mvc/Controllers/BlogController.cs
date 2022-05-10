using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MySite.Entities.Concrete;
using MySite.Entities.Dtos;
using MySite.Services.Abstract;
using MySite.Shared.Utilities.Helpers.Abstract;
using NToastNotify;
using System.Threading.Tasks;

namespace MySite.Mvc.Controllers
{
    public class BlogController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly AboutUsPageInfo _aboutUsPageInfo;
        private readonly IMailService _mailService;
        private readonly IToastNotification _toastNotification;
        private readonly IWritableOptions<AboutUsPageInfo> _aboutUsPageInfoWriter;

        public BlogController(IArticleService articleService, IOptionsSnapshot<AboutUsPageInfo> aboutUsPageInfo, IMailService mailService, IToastNotification toastNotification, IWritableOptions<AboutUsPageInfo> aboutUsPageInfoWriter)
        {
            _articleService = articleService;
            _mailService = mailService;
            _toastNotification = toastNotification;
            _aboutUsPageInfo = aboutUsPageInfo.Value;
            _aboutUsPageInfoWriter = aboutUsPageInfoWriter;
        }

        [Route("blog")]
        [Route("kategori/{categoryTitle}")]
        [HttpGet]
        public async Task<IActionResult> Index(int currentPage = 1, int pageSize = 5, bool isAscending = false)
        {
            var articlesResult = await _articleService.GetAllByPagingAsync(currentPage, pageSize, isAscending);
            return View(articlesResult.Data);
        }

        [HttpGet]
        public IActionResult About()
        {
            return View(_aboutUsPageInfo);
        }

        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(EmailSendDto emailSendDto)
        {
            if (ModelState.IsValid)
            {
                var result = _mailService.SendContactEmail(emailSendDto);
                _toastNotification.AddSuccessToastMessage(result.Message, new ToastrOptions
                {
                    Title = "Başarılı İşlem"
                });
                return View();
            }
            return View(emailSendDto);
        }
    }
}