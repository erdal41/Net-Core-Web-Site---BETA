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
    public class YorumController : BaseController
    {
        private readonly ICommentService _commentService;

        public YorumController(UserManager<User> userManager, IMapper mapper, IImageHelper imageHelper, ICommentService commentService) : base(userManager, mapper, imageHelper)
        {
            _commentService = commentService;
        }

        [Authorize(Roles = "SuperAdmin,Comment.Read")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _commentService.GetAllAsync();
            return View(result.Data);
        }

        [Authorize(Roles = "SuperAdmin,Comment.Read")]
        [HttpGet]
        public async Task<IActionResult> TumYorumlar()
        {
            var result = await _commentService.GetAllByNonDeletedAsync();
            var commentsResult = JsonSerializer.Serialize(result, new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
            });
            return Json(commentsResult);
        }

        [Authorize(Roles = "SuperAdmin,Comment.Read")]
        [HttpGet]
        public async Task<IActionResult> YorumDetayi(int commentId)
        {
            var result = await _commentService.GetAsync(commentId);
            if (result.ResultStatus == ResultStatus.Success)
            {
                return PartialView("_CommentDetailPartial", result.Data);
            }
            else
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "SuperAdmin,Comment.Delete")]
        [HttpPost]
        public async Task<IActionResult> Sil(int commentId)
        {
            var result = await _commentService.DeleteAsync(commentId, LoggedInUser.UserName);
            var commentResult = JsonSerializer.Serialize(result, new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return Json(commentResult);
        }

        [Authorize(Roles = "SuperAdmin,Tag.Delete")]
        [HttpPost]
        public async Task<JsonResult> TopluSil(int[] commentIds)
        {
            var jsonResult = string.Empty;
            foreach (var commentId in commentIds)
            {
                var result = await _commentService.DeleteAsync(commentId, LoggedInUser.UserName);
                var commentResult = JsonSerializer.Serialize(result, new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve
                });
                jsonResult = commentResult;
            }
            return Json(jsonResult);
        }

        [Authorize(Roles = "SuperAdmin,Comment.Update")]
        [HttpPost]
        public async Task<IActionResult> Onay(int commentId)
        {
            var result = await _commentService.ApproveAsync(commentId, LoggedInUser.UserName);
            var commentResult = JsonSerializer.Serialize(result, new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return Json(commentResult);
        }

        [Authorize(Roles = "SuperAdmin,Comment.Update")]
        [HttpGet]
        public async Task<IActionResult> Duzenle(int commentId)
        {
            var result = await _commentService.GetCommentUpdateDtoAsync(commentId);
            if (result.ResultStatus == ResultStatus.Success)
            {
                return PartialView("_CommentUpdatePartial", result.Data);
            }
            else
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "SuperAdmin,Comment.Update")]
        [HttpPost]
        public async Task<IActionResult> Duzenle(CommentUpdateDto commentUpdateDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _commentService.UpdateAsync(commentUpdateDto, LoggedInUser.UserName);
                if (result.ResultStatus == ResultStatus.Success)
                {
                    var commentUpdateAjaxModel = JsonSerializer.Serialize(new CommentUpdateAjaxViewModel
                    {
                        CommentDto = result.Data,
                        CommentUpdatePartial = await this.RenderViewToStringAsync("_CommentUpdatePartial", commentUpdateDto)
                    }, new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve
                    });
                    return Json(commentUpdateAjaxModel);
                }
            }
            var commentUpdateAjaxErrorModel = JsonSerializer.Serialize(new CommentUpdateAjaxViewModel
            {
                CommentUpdatePartial = await this.RenderViewToStringAsync("_CommentUpdatePartial", commentUpdateDto)
            });
            return Json(commentUpdateAjaxErrorModel);
        }

        [Authorize(Roles = "SuperAdmin,Comment.Read")]
        [HttpGet]
        public async Task<IActionResult> SilinmisYorumlar()
        {
            var result = await _commentService.GetAllByDeletedAsync();
            return View(result.Data);
        }

        [Authorize(Roles = "SuperAdmin,Comment.Read")]
        [HttpGet]
        public async Task<JsonResult> TumSilinmisYorumlar()
        {
            var result = await _commentService.GetAllByDeletedAsync();
            var comments = JsonSerializer.Serialize(result, new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return Json(comments);
        }

        [Authorize(Roles = "SuperAdmin,Comment.Update")]
        [HttpPost]
        public async Task<JsonResult> GeriAl(int commentId)
        {
            var result = await _commentService.UndoDeleteAsync(commentId, LoggedInUser.UserName);
            var undoDeleteCommentResult = JsonSerializer.Serialize(result, new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return Json(undoDeleteCommentResult);
        }

        [Authorize(Roles = "SuperAdmin,Comment.Update")]
        [HttpPost]
        public async Task<JsonResult> TopluGeriAl(int[] commentIds)
        {
            string jsonResult = string.Empty;
            foreach (var commentId in commentIds)
            {
                var result = await _commentService.UndoDeleteAsync(commentId, LoggedInUser.UserName);
                var undoDeleteCommentResult = JsonSerializer.Serialize(result, new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve
                });
                jsonResult = undoDeleteCommentResult;
            }
            return Json(jsonResult);
        }

        [Authorize(Roles = "SuperAdmin,Comment.Delete")]
        [HttpPost]
        public async Task<JsonResult> KaliciSil(int commentId)
        {
            var result = await _commentService.HardDeleteAsync(commentId);
            var hardDeletedCommentResult = JsonSerializer.Serialize(result, new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return Json(hardDeletedCommentResult);
        }

        [Authorize(Roles = "SuperAdmin,Comment.Delete")]
        [HttpPost]
        public async Task<JsonResult> TopluKaliciSil(int[] commentIds)
        {
            string jsonResult = string.Empty;
            foreach (var commentId in commentIds)
            {
                var result = await _commentService.HardDeleteAsync(commentId);
                var hardDeletedCommentResult = JsonSerializer.Serialize(result, new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve
                });
                jsonResult = hardDeletedCommentResult;
            }
            return Json(jsonResult);
        }
    }
}