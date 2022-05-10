using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MySite.Entities.Concrete;
using MySite.Entities.Dtos;
using MySite.Mvc.Areas.Admin.Models;
using MySite.Mvc.Helpers.Abstract;
using MySite.Services.Abstract;
using MySite.Shared.Utilities.Extensions;
using MySite.Shared.Utilities.Results.ComplexTypes;
using NToastNotify;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MySite.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MedyaController : BaseController
    {
        private readonly IUploadService _uploadService;

        public MedyaController(IUploadService uploadService, UserManager<User> userManager, IMapper mapper, IImageHelper imageHelper) : base(userManager, mapper, imageHelper)
        {
            _uploadService = uploadService;
        }

        [Authorize(Roles = "SuperAdmin,Article.Read")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _uploadService.GetAllAsync();
            if (result.ResultStatus == ResultStatus.Success) return View(result.Data);
            return NotFound();
        }

        public async Task<IActionResult> GetAllUploadsPartial()
        {
            var result = await _uploadService.GetAllAsync();
            if (result.ResultStatus == ResultStatus.Success)
            {
                return PartialView("_GetAllUploadsPartial", result.Data);
            }
            else
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "SuperAdmin,Article.Create")]
        [HttpGet]
        public IActionResult Ekle()
        {
            return View();
        }

        [Authorize(Roles = "SuperAdmin,Article.Create")]
        [HttpPost]
        public async Task<IActionResult> Ekle(IList<IFormFile> files)
        {
            foreach (IFormFile file in files)
            {
                var uploadAddDto = Mapper.Map<UploadAddDto>(file);
                var uploadResult = await ImageHelper.Upload(file, LoggedInUser.UserName);
                uploadAddDto.FileName = uploadResult.Data.FileFullName;
                uploadAddDto.AltText = uploadResult.Data.FileFullName;
                uploadAddDto.ContentType = uploadResult.Data.ContentType;
                uploadAddDto.Size = uploadResult.Data.Size;

                await _uploadService.AddAsync(uploadAddDto, LoggedInUser.UserName);
            }
            return Json(files);
        }

        [Authorize(Roles = "SuperAdmin,Article.Update")]
        [HttpGet]
        public async Task<IActionResult> Duzenle(int uploadId)
        {
            var result = await _uploadService.GetUploadUpdateDtoAsync(uploadId);

            if (result.ResultStatus == ResultStatus.Success)
            {
                return PartialView("_UploadUpdatePartial", result.Data);
            }
            else
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "SuperAdmin,Article.Update")]
        [HttpPost]
        public async Task<IActionResult> Duzenle(UploadUpdateDto uploadUpdateDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _uploadService.UpdateAsync(uploadUpdateDto, LoggedInUser.UserName);
                if (result.ResultStatus == ResultStatus.Success)
                {
                    var uploadUpdateAjaxViewModel = JsonSerializer.Serialize(new UploadUpdateAjaxViewModel
                    {
                        UploadDto = result.Data,
                        UploadUpdatePartial = await this.RenderViewToStringAsync("_UploadUpdatePartial", uploadUpdateDto)
                    });
                    return Json(uploadUpdateAjaxViewModel);
                }
            }
            var uploadUpdateAjaxErrorViewModel = JsonSerializer.Serialize(new UploadUpdateAjaxViewModel
            {
                UploadUpdatePartial = await this.RenderViewToStringAsync("_UploadUpdatePartial", uploadUpdateDto)
            });
            return Json(uploadUpdateAjaxErrorViewModel);
        }

        [Authorize(Roles = "SuperAdmin,Article.Update")]
        [HttpPost]
        public async Task<JsonResult> Sil(int uploadId)
        {
            var upload = await _uploadService.GetAsync(uploadId);
            ImageHelper.Delete(upload.Data.Upload.FileName);
            var result = await _uploadService.DeleteAsync(uploadId);
            var uploadResult = JsonSerializer.Serialize(result);
            return Json(uploadResult);
        }

        [Authorize(Roles = "SuperAdmin,Article.Update")]
        [HttpPost]
        public async Task<JsonResult> TopluSil(int[] uploadIds)
        {
            var jsonResult = string.Empty;
            foreach (var uploadId in uploadIds)
            {
                var upload = await _uploadService.GetAsync(uploadId);
                ImageHelper.Delete(upload.Data.Upload.FileName);
                var result = await _uploadService.DeleteAsync(uploadId);
                var uploadResult = JsonSerializer.Serialize(result);
                jsonResult = uploadResult;
            }
            return Json(jsonResult);
        }

        [Authorize(Roles = "SuperAdmin,Article.Read")]
        [HttpGet]
        public async Task<JsonResult> TumDosyalar()
        {
            var uploads = await _uploadService.GetAllAsync();
            var uploadResult = JsonSerializer.Serialize(uploads, new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return Json(uploadResult);
        }
    }
}