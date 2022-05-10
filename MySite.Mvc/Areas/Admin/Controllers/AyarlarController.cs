using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MySite.Entities.Concrete;
using MySite.Mvc.Areas.Admin.Models;
using MySite.Services.Abstract;
using MySite.Shared.Utilities.Helpers.Abstract;
using NToastNotify;
using System.Threading.Tasks;

namespace MySite.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AyarlarController : Controller
    {
        private readonly WebsiteInfo _websiteInfo;
        private readonly IWritableOptions<WebsiteInfo> _websiteInfoWriter;
        private readonly AboutUsPageInfo _aboutUsPageInfo;
        private readonly IWritableOptions<AboutUsPageInfo> _aboutUsPageInfoWriter;
        private readonly ReferencesPageInfo _referencesPageInfo;
        private readonly IWritableOptions<ReferencesPageInfo> _referencesPageInfoWriter;
        private readonly ContactPageInfo _contactPageInfo;
        private readonly IWritableOptions<ContactPageInfo> _contactPageInfoWriter;
        private readonly SmtpSettings _smtpSettings;
        private readonly IWritableOptions<SmtpSettings> _smtpSettingsWriter;
        private readonly ArticleRightSideBarWidgetOptions _articleRightSideBarWidgetOptions;
        private readonly IWritableOptions<ArticleRightSideBarWidgetOptions> _articleRightSideBarWidgetOptionsWriter;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly IToastNotification _toastNotification;

        public AyarlarController(IOptionsSnapshot<WebsiteInfo> websiteInfo, IWritableOptions<WebsiteInfo> websiteInfoWriter, IOptionsSnapshot<AboutUsPageInfo> aboutUsPageInfo, IWritableOptions<AboutUsPageInfo> aboutUsPageInfoWriter, IOptionsSnapshot<ReferencesPageInfo> referencesPageInfo, IWritableOptions<ReferencesPageInfo> referencesPageInfoWriter, IToastNotification toastNotification, IOptionsSnapshot<SmtpSettings> smtpSettings, IWritableOptions<SmtpSettings> smtpSettingsWriter, IOptionsSnapshot<ContactPageInfo> contactPageInfo, IWritableOptions<ContactPageInfo> contactPageInfoWriter, IOptionsSnapshot<ArticleRightSideBarWidgetOptions> articleRightSideBarWidgetOptions, IWritableOptions<ArticleRightSideBarWidgetOptions> articleRightSideBarWidgetOptionsWriter, ICategoryService categoryService, IMapper mapper)
        {
            _websiteInfo = websiteInfo.Value;
            _websiteInfoWriter = websiteInfoWriter;
            _aboutUsPageInfo = aboutUsPageInfo.Value;
            _aboutUsPageInfoWriter = aboutUsPageInfoWriter;
            _referencesPageInfo = referencesPageInfo.Value;
            _referencesPageInfoWriter = referencesPageInfoWriter;
            _contactPageInfo = contactPageInfo.Value;
            _contactPageInfoWriter = contactPageInfoWriter;
            _toastNotification = toastNotification;
            _smtpSettings = smtpSettings.Value;
            _smtpSettingsWriter = smtpSettingsWriter;
            _articleRightSideBarWidgetOptions = articleRightSideBarWidgetOptions.Value;
            _articleRightSideBarWidgetOptionsWriter = articleRightSideBarWidgetOptionsWriter;
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Anasayfa()
        {
            return View(_websiteInfo);
        }

        [HttpPost]
        public IActionResult Anasayfa(WebsiteInfo websiteInfo)
        {
            if (ModelState.IsValid)
            {
                _websiteInfoWriter.Update(x =>
                {
                    x.Title = websiteInfo.Title;
                    x.MenuTitle = websiteInfo.MenuTitle;
                    x.SeoTitle = websiteInfo.SeoTitle;
                    x.SeoAuthor = websiteInfo.SeoAuthor;
                    x.SeoDescription = websiteInfo.SeoDescription;
                    x.SeoTags = websiteInfo.SeoTags;
                });
                _toastNotification.AddSuccessToastMessage("Sitenizin genel ayarları başarıyla güncellenmiştir.", new ToastrOptions
                {
                    Title = "İşlem Başarılı!"
                });
                return View(websiteInfo);
            }
            return View(websiteInfo);
        }

        [HttpGet]
        public IActionResult Hakkimizda()
        {
            return View(_aboutUsPageInfo);
        }

        [HttpPost]
        public IActionResult Hakkimizda(AboutUsPageInfo aboutUsPageInfo)
        {
            if (ModelState.IsValid)
            {
                _aboutUsPageInfoWriter.Update(x =>
                {
                    x.Title = aboutUsPageInfo.Title;
                    x.SeoTitle = aboutUsPageInfo.SeoTitle;
                    x.SeoAuthor = aboutUsPageInfo.SeoAuthor;
                    x.SeoDescription = aboutUsPageInfo.SeoDescription;
                    x.SeoTags = aboutUsPageInfo.SeoTags;
                });
                _toastNotification.AddSuccessToastMessage("Hakkımızda sayfasının ayarları başarıyla güncellenmiştir.", new ToastrOptions
                {
                    Title = "İşlem Başarılı!"
                });
                return View(aboutUsPageInfo);
            }
            return View(aboutUsPageInfo);
        }

        [HttpGet]
        public IActionResult Referanslar()
        {
            return View(_referencesPageInfo);
        }

        [HttpPost]
        public IActionResult Referanslar(ReferencesPageInfo referencesPageInfo)
        {
            if (ModelState.IsValid)
            {
                _referencesPageInfoWriter.Update(x =>
                {
                    x.Title = referencesPageInfo.Title;
                    x.SeoTitle = referencesPageInfo.SeoTitle;
                    x.SeoAuthor = referencesPageInfo.SeoAuthor;
                    x.SeoDescription = referencesPageInfo.SeoDescription;
                    x.SeoTags = referencesPageInfo.SeoTags;
                });
                _toastNotification.AddSuccessToastMessage("Referanslar sayfasının ayarları başarıyla güncellenmiştir.", new ToastrOptions
                {
                    Title = "İşlem Başarılı!"
                });
                return View(referencesPageInfo);
            }
            return View(referencesPageInfo);
        }

        [HttpGet]
        public IActionResult Iletisim()
        {
            return View(_contactPageInfo);
        }

        [HttpPost]
        public IActionResult Iletisim(ContactPageInfo contactPageInfo)
        {
            if (ModelState.IsValid)
            {
                _contactPageInfoWriter.Update(x =>
                {
                    x.Title = contactPageInfo.Title;
                    x.SeoTitle = contactPageInfo.SeoTitle;
                    x.SeoAuthor = contactPageInfo.SeoAuthor;
                    x.SeoDescription = contactPageInfo.SeoDescription;
                    x.SeoTags = contactPageInfo.SeoTags;
                });
                _toastNotification.AddSuccessToastMessage("İletişim sayfasının ayarları başarıyla güncellenmiştir.", new ToastrOptions
                {
                    Title = "İşlem Başarılı!"
                });
                return View(contactPageInfo);
            }
            return View(contactPageInfo);
        }

        [HttpGet]
        public IActionResult EmailAyarlari()
        {
            return View(_smtpSettings);
        }

        [HttpPost]
        public IActionResult EmailAyarlari(SmtpSettings smtpSettings)
        {
            if (ModelState.IsValid)
            {
                _smtpSettingsWriter.Update(x =>
                {
                    x.Server = smtpSettings.Server;
                    x.Port = smtpSettings.Port;
                    x.SenderName = smtpSettings.SenderName;
                    x.SenderEmail = smtpSettings.SenderEmail;
                    x.Username = smtpSettings.Username;
                    x.Password = smtpSettings.Password;
                });
                _toastNotification.AddSuccessToastMessage("E-Mail ayarları başarıyla güncellenmiştir.", new ToastrOptions
                {
                    Title = "İşlem Başarılı!"
                });
                return View(smtpSettings);
            }
            return View(smtpSettings);
        }

        [HttpGet]
        public async Task<IActionResult> ArticleRightSideBarWidgetSettings()
        {
            var categoriesResult = await _categoryService.GetAllByNonDeletedAndActiveAsync();
            var articleRightSideBarWidgetOptionsViewModel = _mapper.Map<ArticleRightSideBarWidgetOptionsViewModel>(_articleRightSideBarWidgetOptions);
            articleRightSideBarWidgetOptionsViewModel.Categories = categoriesResult.Data.Categories;
            return View(articleRightSideBarWidgetOptionsViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ArticleRightSideBarWidgetSettings(ArticleRightSideBarWidgetOptionsViewModel articleRightSideBarWidgetOptionsViewModel)
        {
            var categoriesResult = await _categoryService.GetAllByNonDeletedAndActiveAsync();
            articleRightSideBarWidgetOptionsViewModel.Categories = categoriesResult.Data.Categories;
            if (ModelState.IsValid)
            {
                _articleRightSideBarWidgetOptionsWriter.Update(x =>
                {
                    x.Header = articleRightSideBarWidgetOptionsViewModel.Header;
                    x.TakeSize = articleRightSideBarWidgetOptionsViewModel.TakeSize;
                    x.CategoryId = articleRightSideBarWidgetOptionsViewModel.CategoryId;
                    x.FilterBy = articleRightSideBarWidgetOptionsViewModel.FilterBy;
                    x.OrderBy = articleRightSideBarWidgetOptionsViewModel.OrderBy;
                    x.IsAscending = articleRightSideBarWidgetOptionsViewModel.IsAscending;
                    x.StartAt = articleRightSideBarWidgetOptionsViewModel.StartAt;
                    x.EndAt = articleRightSideBarWidgetOptionsViewModel.EndAt;
                    x.MaxViewCount = articleRightSideBarWidgetOptionsViewModel.MaxViewCount;
                    x.MinViewCount = articleRightSideBarWidgetOptionsViewModel.MinViewCount;
                    x.MaxCommentCount = articleRightSideBarWidgetOptionsViewModel.MaxCommentCount;
                    x.MinCommentCount = articleRightSideBarWidgetOptionsViewModel.MinCommentCount;
                });
                _toastNotification.AddSuccessToastMessage("Makale sayfalarınızın widget ayarları başarıyla güncellenmiştir.", new ToastrOptions
                {
                    Title = "İşlem Başarılı!"
                });
                return View(articleRightSideBarWidgetOptionsViewModel);
            }
            return View(articleRightSideBarWidgetOptionsViewModel);
        }
    }
}
