using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MySite.Entities.Concrete;
using MySite.Entities.Dtos;
using MySite.Mvc.Areas.Admin.Models;
using MySite.Mvc.Helpers.Abstract;
using MySite.Services.Abstract;
using MySite.Shared.Utilities.Results.ComplexTypes;
using NToastNotify;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MySite.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MakaleController : BaseController
    {
        private readonly IArticleService _articleService;
        private readonly ICategoryService _categoryService;
        private readonly IToastNotification _toastNotification;

        public MakaleController(IArticleService articleService, ICategoryService categoryService, UserManager<User> userManager, IMapper mapper, IImageHelper imageHelper, IToastNotification toastNotification) : base(userManager, mapper, imageHelper)
        {
            _articleService = articleService;
            _categoryService = categoryService;
            _toastNotification = toastNotification;
        }

        [Authorize(Roles = "SuperAdmin,Article.Read")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _articleService.GetAllAsync();
            if (result.ResultStatus == ResultStatus.Success) return View(result.Data);
            return NotFound();
        }

        [Authorize(Roles = "SuperAdmin,Article.Create")]
        [HttpGet]
        public async Task<IActionResult> Ekle()
        {
            var result = await _categoryService.GetAllByNonDeletedAndActiveAsync();
            if (result.ResultStatus == ResultStatus.Success)
            {
                return View(new ArticleAddViewModel
                {
                    Categories = result.Data.Categories
                });
            }
            return NotFound();
        }

        [Authorize(Roles = "SuperAdmin,Article.Create")]
        [HttpPost]
        public async Task<IActionResult> Ekle(ArticleAddViewModel articleAddViewModel)
        {
            if (ModelState.IsValid)
            {
                if (articleAddViewModel.Picture == null)
                {
                    articleAddViewModel.Picture = "default-post-image.jpg";
                }

                var articleAddDto = Mapper.Map<ArticleAddDto>(articleAddViewModel);
                var result = await _articleService.AddAsync(articleAddDto, LoggedInUser.UserName, LoggedInUser.Id);
                if (result.ResultStatus == ResultStatus.Success)
                {
                    _toastNotification.AddSuccessToastMessage(result.Message, new ToastrOptions
                    {
                        Title = "Başarılı İşlem!"
                    });
                    return RedirectToAction("Index", "Makale");
                }
                else
                {
                    ModelState.AddModelError("", result.Message);
                }
            }

            var categories = await _categoryService.GetAllByNonDeletedAndActiveAsync();
            articleAddViewModel.Categories = categories.Data.Categories;
            return View(articleAddViewModel);
        }

        [Authorize(Roles = "SuperAdmin,Article.Update")]
        [HttpGet]
        public async Task<IActionResult> Duzenle(int articleId)
        {
            var articleResult = await _articleService.GetArticleUpdateDtoAsync(articleId);
            var categoriesResult = await _categoryService.GetAllByNonDeletedAndActiveAsync();
            if (articleResult.ResultStatus == ResultStatus.Success && categoriesResult.ResultStatus == ResultStatus.Success)
            {
                var articleUpdateViewModel = Mapper.Map<ArticleUpdateViewModel>(articleResult.Data);
                articleUpdateViewModel.Categories = categoriesResult.Data.Categories;
                return View(articleUpdateViewModel);
            }
            else
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "SuperAdmin,Article.Update")]
        [HttpPost]
        public async Task<IActionResult> Duzenle(ArticleUpdateViewModel articleUpdateViewModel)
        {
            if (ModelState.IsValid)
            {
                if (articleUpdateViewModel.Picture == null)
                {
                    articleUpdateViewModel.Picture = "default-post-image.jpg";
                }

                var articleUpdateDto = Mapper.Map<ArticleUpdateDto>(articleUpdateViewModel);
                var result = await _articleService.UpdateAsync(articleUpdateDto, LoggedInUser.UserName);
                if (result.ResultStatus == ResultStatus.Success)
                {
                    _toastNotification.AddSuccessToastMessage(result.Message, new ToastrOptions
                    {
                        Title = "Başarılı İşlem!"
                    });
                    return RedirectToAction("Index", "Makale");
                }
                else
                {
                    ModelState.AddModelError("", result.Message);
                }
            }

            var categories = await _categoryService.GetAllByNonDeletedAndActiveAsync();
            articleUpdateViewModel.Categories = categories.Data.Categories;
            return View(articleUpdateViewModel);
        }

        [Authorize(Roles = "SuperAdmin,Article.Delete")]
        [HttpPost]
        public async Task<JsonResult> Sil(int articleId)
        {
            var result = await _articleService.DeleteAsync(articleId, LoggedInUser.UserName);
            var articleResult = JsonSerializer.Serialize(result);
            return Json(articleResult);
        }

        [Authorize(Roles = "SuperAdmin,Article.Delete")]
        [HttpPost]
        public async Task<JsonResult> TopluSil(int[] articleIds)
        {
            var jsonResult = string.Empty;
            foreach (var articleId in articleIds)
            {
                var result = await _articleService.DeleteAsync(articleId, LoggedInUser.UserName);
                var articleResult = JsonSerializer.Serialize(result);
                jsonResult = articleResult;
            }
            return Json(jsonResult);
        }

        [Authorize(Roles = "SuperAdmin,Article.Read")]
        [HttpGet]
        public async Task<JsonResult> TumMakaleler()
        {
            var articles = await _articleService.GetAllByNonDeletedAndActiveAsync();
            var articleResult = JsonSerializer.Serialize(articles, new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return Json(articleResult);
        }

        [Authorize(Roles = "SuperAdmin,Article.Read")]
        [HttpGet]
        public async Task<IActionResult> SilinmisMakaleler()
        {
            var result = await _articleService.GetAllByDeletedAsync();
            return View(result.Data);
        }

        [Authorize(Roles = "SuperAdmin,Article.Read")]
        [HttpGet]
        public async Task<JsonResult> TumSilinmisMakaleler()
        {
            var result = await _articleService.GetAllByDeletedAsync();
            var articles = JsonSerializer.Serialize(result, new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return Json(articles);
        }

        [Authorize(Roles = "SuperAdmin,Article.Update")]
        [HttpPost]
        public async Task<JsonResult> GeriAl(int articleId)
        {
            var result = await _articleService.UndoDeleteAsync(articleId, LoggedInUser.UserName);
            var undoDeleteArticleResult = JsonSerializer.Serialize(result);
            return Json(undoDeleteArticleResult);
        }

        [Authorize(Roles = "SuperAdmin,Article.Update")]
        [HttpPost]
        public async Task<JsonResult> TopluGeriAl(int[] articleIds)
        {
            var jsonresult = string.Empty;
            foreach (var articleId in articleIds)
            {
                var result = await _articleService.UndoDeleteAsync(articleId, LoggedInUser.UserName);
                var undoDeleteArticleResult = JsonSerializer.Serialize(result);
                jsonresult = undoDeleteArticleResult;
            }
            return Json(jsonresult);
        }

        [Authorize(Roles = "SuperAdmin,Article.Delete")]
        [HttpPost]
        public async Task<JsonResult> KaliciSil(int articleId)
        {
            var result = await _articleService.HardDeleteAsync(articleId);
            var hardDeletedArticleResult = JsonSerializer.Serialize(result);
            return Json(hardDeletedArticleResult);
        }

        [Authorize(Roles = "SuperAdmin,Article.Delete")]
        [HttpPost]
        public async Task<JsonResult> TopluKaliciSil(int[] articleIds)
        {
            var jsonResult = string.Empty;
            foreach (var articleId in articleIds)
            {
                var result = await _articleService.HardDeleteAsync(articleId);
                var hardDeletedArticleResult = JsonSerializer.Serialize(result);
                jsonResult = hardDeletedArticleResult;
            }
            return Json(jsonResult);
        }
    }
}