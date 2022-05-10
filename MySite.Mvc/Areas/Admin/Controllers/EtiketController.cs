using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MySite.Entities.Concrete;
using MySite.Entities.Dtos;
using MySite.Mvc.Areas.Admin.Models;
using MySite.Services.Abstract;
using MySite.Shared.Utilities.Extensions;
using MySite.Shared.Utilities.Results.ComplexTypes;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MySite.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EtiketController : BaseController
    {
        private readonly ITagService _tagService;

        public EtiketController(ITagService tagService, UserManager<User> userManager, IMapper mapper) : base(userManager, mapper)
        {
            _tagService = tagService;
        }

        [Authorize(Roles = "SuperAdmin,Tag.Read")]
        public async Task<IActionResult> Index()
        {
            var result = await _tagService.GetAllAsync();
            return View(result.Data);
        }

        [Authorize(Roles = "SuperAdmin,Tag.Create")]
        [HttpGet]
        public IActionResult Ekle()
        {
            return PartialView("_TagAddPartial");
        }

        [Authorize(Roles = "SuperAdmin,Tag.Create")]
        [HttpPost]
        public async Task<IActionResult> Ekle(TagAddDto tagAddDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _tagService.AddAsync(tagAddDto, LoggedInUser.UserName);
                if (result.ResultStatus == ResultStatus.Success)
                {
                    var tagAddAjaxModel = JsonSerializer.Serialize(new TagAddAjaxViewModel
                    {
                        TagDto = result.Data,
                        TagAddPartial = await this.RenderViewToStringAsync("_TagAddPartial", tagAddDto)
                    });
                    return Json(tagAddAjaxModel);
                }
            }
            var tagAddAjaxErrorModel = JsonSerializer.Serialize(new TagAddAjaxViewModel
            {
                TagAddPartial = await this.RenderViewToStringAsync("_TagAddPartial", tagAddDto)
            });
            return Json(tagAddAjaxErrorModel);
        }

        [Authorize(Roles = "SuperAdmin,Tag.Update")]
        [HttpGet]
        public async Task<IActionResult> Duzenle(int tagId)
        {
            var result = await _tagService.GetTagUpdateDtoAsync(tagId);
            if (result.ResultStatus == ResultStatus.Success)
            {
                return PartialView("_TagUpdatePartial", result.Data);
            }
            else
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "SuperAdmin,Tag.Update")]
        [HttpPost]
        public async Task<IActionResult> Duzenle(TagUpdateDto tagUpdateDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _tagService.UpdateAsync(tagUpdateDto, LoggedInUser.UserName);
                if (result.ResultStatus == ResultStatus.Success)
                {
                    var tagUpdateAjaxModel = JsonSerializer.Serialize(new TagUpdateAjaxViewModel
                    {
                        TagDto = result.Data,
                        TagUpdatePartial = await this.RenderViewToStringAsync("_TagUpdatePartial", tagUpdateDto)
                    });
                    return Json(tagUpdateAjaxModel);
                }
            }
            var tagUpdateAjaxErrorModel = JsonSerializer.Serialize(new TagUpdateAjaxViewModel
            {
                TagUpdatePartial = await this.RenderViewToStringAsync("_TagUpdatePartial", tagUpdateDto)
            });
            return Json(tagUpdateAjaxErrorModel);
        }

        [Authorize(Roles = "SuperAdmin,Tag.Read")]
        [HttpGet]
        public async Task<JsonResult> TumEtiketler()
        {
            var result = await _tagService.GetAllByNonDeletedAsync();
            var tags = JsonSerializer.Serialize(result.Data, new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return Json(tags);
        }

        [Authorize(Roles = "SuperAdmin,Tag.Delete")]
        [HttpPost]
        public async Task<JsonResult> Sil(int tagId)
        {
            var result = await _tagService.DeleteAsync(tagId, LoggedInUser.UserName);
            var deletedTag = JsonSerializer.Serialize(result.Data);
            return Json(deletedTag);
        }

        [Authorize(Roles = "SuperAdmin,Tag.Delete")]
        [HttpPost]
        public async Task<JsonResult> TopluSil(int[] tagIds)
        {
            var jsonResult = string.Empty;
            foreach (var tagId in tagIds)
            {
                var result = await _tagService.DeleteAsync(tagId, LoggedInUser.UserName);
                var deletedTag = JsonSerializer.Serialize(result.Data);
                jsonResult = deletedTag;
            }
            return Json(jsonResult);
        }

        [Authorize(Roles = "SuperAdmin,Tag.Read")]
        [HttpGet]
        public async Task<IActionResult> SilinmisEtiketler()
        {
            var result = await _tagService.GetAllByDeletedAsync();
            return View(result.Data);
        }

        [Authorize(Roles = "SuperAdmin,Tag.Read")]
        [HttpGet]
        public async Task<JsonResult> TumSilinmisEtiketler()
        {
            var result = await _tagService.GetAllByDeletedAsync();
            var tags = JsonSerializer.Serialize(result.Data, new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return Json(tags);
        }

        [Authorize(Roles = "SuperAdmin,Tag.Update")]
        [HttpPost]
        public async Task<JsonResult> GeriAl(int tagId)
        {
            var result = await _tagService.UndoDeleteAsync(tagId, LoggedInUser.UserName);
            var undoDeletedTag = JsonSerializer.Serialize(result.Data);
            return Json(undoDeletedTag);
        }

        [Authorize(Roles = "SuperAdmin,Tag.Update")]
        [HttpPost]
        public async Task<JsonResult> TopluGeriAl(int[] tagIds)
        {
            var jsonResult = string.Empty;
            foreach (var tagId in tagIds)
            {
                var result = await _tagService.UndoDeleteAsync(tagId, LoggedInUser.UserName);
                var undoDeletedTag = JsonSerializer.Serialize(result.Data);
                jsonResult = undoDeletedTag;
            }
            return Json(jsonResult);
        }

        [Authorize(Roles = "SuperAdmin,Tag.Delete")]
        [HttpPost]
        public async Task<JsonResult> KaliciSil(int tagId)
        {
            var result = await _tagService.HardDeleteAsync(tagId);
            var deletedTag = JsonSerializer.Serialize(result);
            return Json(deletedTag);
        }

        [Authorize(Roles = "SuperAdmin,Tag.Delete")]
        [HttpPost]
        public async Task<JsonResult> TopluKaliciSil(int[] tagIds)
        {
            var jsonResult = string.Empty;
            foreach (var tagId in tagIds)
            {
                var result = await _tagService.HardDeleteAsync(tagId);
                var deletedTag = JsonSerializer.Serialize(result);
                jsonResult = deletedTag;
            }
            return Json(jsonResult);
        }
    }
}