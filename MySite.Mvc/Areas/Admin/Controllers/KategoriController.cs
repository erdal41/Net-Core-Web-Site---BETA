using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MySite.Entities.Concrete;
using MySite.Entities.Dtos;
using MySite.Mvc.Areas.Admin.Models;
using MySite.Mvc.Helpers.Abstract;
using MySite.Services.Abstract;
using MySite.Shared.Utilities.Extensions;
using MySite.Shared.Utilities.Results.ComplexTypes;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MySite.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class KategoriController : BaseController
    {
        private readonly ICategoryService _categoryService;

        public KategoriController(ICategoryService categoryService, UserManager<User> userManager, IMapper mapper, IImageHelper imageHelper) : base(userManager, mapper, imageHelper)
        {
            _categoryService = categoryService;
        }

        [Authorize(Roles = "SuperAdmin,Category.Read")]
        public async Task<IActionResult> Index()
        {
            var result = await _categoryService.GetAllAsync();
            if (result.ResultStatus == ResultStatus.Success) return View(result.Data);
            return NotFound();
        }

        [Authorize(Roles = "SuperAdmin,Category.Create")]
        [HttpGet]
        public IActionResult Ekle()
        {
            return PartialView("_CategoryAddPartial");
        }

        [Authorize(Roles = "SuperAdmin,Category.Create")]
        [HttpPost]
        public async Task<IActionResult> Ekle(CategoryAddDto categoryAddDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _categoryService.AddAsync(categoryAddDto, LoggedInUser.UserName);
                if (result.ResultStatus == ResultStatus.Success)
                {
                    var categoryAddAjaxModel = JsonSerializer.Serialize(new CategoryAddAjaxViewModel
                    {
                        CategoryDto = result.Data,
                    });
                    return Json(categoryAddAjaxModel);
                }
            }
            var categoryAddAjaxErrorModel = JsonSerializer.Serialize(new CategoryAddAjaxViewModel
            {
                CategoryDto = null,
            });
            return Json(categoryAddAjaxErrorModel);
        }

        [Authorize(Roles = "SuperAdmin,Category.Update")]
        [HttpGet]
        public async Task<IActionResult> Duzenle(int categoryId)
        {
            var result = await _categoryService.GetCategoryUpdateDtoAsync(categoryId);
            if (result.ResultStatus == ResultStatus.Success)
            {
                return PartialView("_CategoryUpdatePartial", result.Data);
            }
            else
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "SuperAdmin,Category.Update")]
        [HttpPost]
        public async Task<IActionResult> Duzenle(CategoryUpdateDto categoryUpdateDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _categoryService.UpdateAsync(categoryUpdateDto, LoggedInUser.UserName);
                if (result.ResultStatus == ResultStatus.Success)
                {
                    var categoryUpdateAjaxModel = JsonSerializer.Serialize(new CategoryUpdateAjaxViewModel
                    {
                        CategoryDto = result.Data,
                        CategoryUpdatePartial = await this.RenderViewToStringAsync("_CategoryUpdatePartial", categoryUpdateDto)
                    });
                    return Json(categoryUpdateAjaxModel);
                }
            }
            var categoryUpdateAjaxErrorModel = JsonSerializer.Serialize(new CategoryUpdateAjaxViewModel
            {
                CategoryUpdatePartial = await this.RenderViewToStringAsync("_CategoryUpdatePartial", categoryUpdateDto)
            });
            return Json(categoryUpdateAjaxErrorModel);
        }

        [Authorize(Roles = "SuperAdmin,Category.Read")]
        [HttpGet]
        public async Task<JsonResult> TumKategoriler()
        {
            var result = await _categoryService.GetAllByNonDeletedAsync();
            var categories = JsonSerializer.Serialize(result.Data, new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return Json(categories);
        }

        [Authorize(Roles = "SuperAdmin,Category.Delete")]
        [HttpPost]
        public async Task<JsonResult> Sil(int categoryId)
        {
            var result = await _categoryService.DeleteAsync(categoryId, LoggedInUser.UserName);
            var deletedCategory = JsonSerializer.Serialize(result.Data);
            return Json(deletedCategory);
        }

        [Authorize(Roles = "SuperAdmin,Tag.Delete")]
        [HttpPost]
        public async Task<JsonResult> TopluSil(int[] categoryIds)
        {
            var jsonResult = string.Empty;
            foreach (var categoryId in categoryIds)
            {
                var result = await _categoryService.DeleteAsync(categoryId, LoggedInUser.UserName);
                var deletedCategory = JsonSerializer.Serialize(result.Data);
                jsonResult = deletedCategory;
            }
            return Json(jsonResult);
        }

        [Authorize(Roles = "SuperAdmin,Category.Read")]
        [HttpGet]
        public async Task<IActionResult> SilinmisKategoriler()
        {
            var result = await _categoryService.GetAllByDeletedAsync();
            return View(result.Data);
        }

        [Authorize(Roles = "SuperAdmin,Category.Read")]
        [HttpGet]
        public async Task<JsonResult> TumSilinmisKategoriler()
        {
            var result = await _categoryService.GetAllByDeletedAsync();
            var categories = JsonSerializer.Serialize(result.Data, new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return Json(categories);
        }

        [Authorize(Roles = "SuperAdmin,Category.Update")]
        [HttpPost]
        public async Task<JsonResult> GeriAl(int categoryId)
        {
            var result = await _categoryService.UndoDeleteAsync(categoryId, LoggedInUser.UserName);
            var undoDeletedCategory = JsonSerializer.Serialize(result.Data);
            return Json(undoDeletedCategory);
        }

        [Authorize(Roles = "SuperAdmin,Category.Update")]
        [HttpPost]
        public async Task<JsonResult> TopluGeriAl(int[] categoryIds)
        {
            var jsonResult = string.Empty;
            foreach (var categoryId in categoryIds)
            {
                var result = await _categoryService.UndoDeleteAsync(categoryId, LoggedInUser.UserName);
                var undoDeletedCategory = JsonSerializer.Serialize(result.Data);
                jsonResult = undoDeletedCategory;
            }
            return Json(jsonResult);
        }

        [Authorize(Roles = "SuperAdmin,Category.Delete")]
        [HttpPost]
        public async Task<JsonResult> KaliciSil(int categoryId)
        {
            var result = await _categoryService.HardDeleteAsync(categoryId);
            var deletedCategory = JsonSerializer.Serialize(result);
            return Json(deletedCategory);
        }

        [Authorize(Roles = "SuperAdmin,Category.Delete")]
        [HttpPost]
        public async Task<JsonResult> TopluKaliciSil(int[] categoryIds)
        {
            var jsonResult = string.Empty;
            foreach (var categoryId in categoryIds)
            {
                var result = await _categoryService.HardDeleteAsync(categoryId);
                var deletedCategory = JsonSerializer.Serialize(result);
                jsonResult = deletedCategory;
            }
            return Json(jsonResult);
        }
    }
}