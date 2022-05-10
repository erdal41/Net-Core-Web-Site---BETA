using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySite.Entities.Concrete;
using MySite.Entities.Dtos;
using MySite.Mvc.Areas.Admin.Models;
using MySite.Mvc.Helpers.Abstract;
using MySite.Shared.Utilities.Results.ComplexTypes;
using NToastNotify;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MySite.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class KullaniciController : BaseController
    {
        private readonly SignInManager<User> _signInManager;
        private readonly IToastNotification _toastNotification;

        public KullaniciController(UserManager<User> userManager, IMapper mapper, SignInManager<User> signInManager, IImageHelper imageHelper, IToastNotification toastNotification) : base(userManager, mapper, imageHelper)
        {
            _signInManager = signInManager;
            _toastNotification = toastNotification;
        }

        [Authorize(Roles = "SuperAdmin,User.Read")]
        public async Task<IActionResult> Index()
        {
            var users = await UserManager.Users.ToListAsync();
            return View(new UserListDto
            {
                Users = users,
                ResultStatus = ResultStatus.Success
            });
        }

        [Authorize(Roles = "SuperAdmin,User.Read")]
        [HttpGet]
        public async Task<PartialViewResult> Detaylar(int userId)
        {
            var user = await UserManager.Users.SingleOrDefaultAsync(u => u.Id == userId);
            return PartialView("_GetDetailPartial", new UserDto { User = user });
        }

        [Authorize(Roles = "SuperAdmin,User.Read")]
        [HttpGet]
        public async Task<JsonResult> TumKullanicilar()
        {
            var users = await UserManager.Users.ToListAsync();
            var userListDto = JsonSerializer.Serialize(new UserListDto
            {
                Users = users,
                ResultStatus = ResultStatus.Success
            }, new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return Json(userListDto);
        }

        [Authorize(Roles = "SuperAdmin,User.Create")]
        [HttpGet]
        public IActionResult Ekle()
        {
            return View();
        }

        [Authorize(Roles = "SuperAdmin,User.Create")]
        [HttpPost]
        public async Task<IActionResult> Ekle(UserAddViewModel userAddViewModel)
        {
            if (ModelState.IsValid)
            {
                if (userAddViewModel.Picture == null)
                {
                    userAddViewModel.Picture = "default-user-image.png";
                }

                var user = Mapper.Map<User>(userAddViewModel);
                var result = await UserManager.CreateAsync(user, userAddViewModel.Password);
                if (result.Succeeded)
                {
                    _toastNotification.AddSuccessToastMessage("Kullanıcı başarılı bir şekilde eklendi.", new ToastrOptions
                    {
                        Title = "Başarılı İşlem!"
                    });
                    return RedirectToAction("Index", "Kullanici");
                }
                else
                {
                    ModelState.AddModelError("", "Kullanıcı eklenemedi.");
                }
            }
            return View(userAddViewModel);
        }

        [Authorize(Roles = "SuperAdmin,User.Delete")]
        [HttpPost]
        public async Task<JsonResult> Sil(int userId)
        {
            var user = await UserManager.FindByIdAsync(userId.ToString());
            var result = await UserManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                var deletedUser = JsonSerializer.Serialize(new UserDto
                {
                    ResultStatus = ResultStatus.Success,
                    Message = $"{user.UserName} adlı kullanıcı adına sahip kullanıcı başarıyla silinmiştir.",
                    User = user
                });
                return Json(deletedUser);
            }
            else
            {
                string errorMessages = String.Empty;
                foreach (var error in result.Errors)
                {
                    errorMessages = $"*{error.Description}\n";
                }

                var deletedUserErrorModel = JsonSerializer.Serialize(new UserDto
                {
                    ResultStatus = ResultStatus.Error,
                    Message =
                        $"{user.UserName} adlı kullanıcı adına sahip kullanıcı silinirken bazı hatalar oluştu.\n{errorMessages}",
                    User = user
                });
                return Json(deletedUserErrorModel);
            }
        }

        [Authorize(Roles = "SuperAdmin,User.Delete")]
        [HttpPost]
        public async Task<JsonResult> TopluSil(int[] userIds)
        {
            var jsonResult = string.Empty;

            foreach (var userId in userIds)
            {
                var user = await UserManager.FindByIdAsync(userId.ToString());
                var result = await UserManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    var deletedUser = JsonSerializer.Serialize(new UserDto
                    {
                        ResultStatus = ResultStatus.Success,
                        Message = $"{user.UserName} adlı kullanıcı adına sahip kullanıcı başarıyla silinmiştir.",
                        User = user
                    });
                    jsonResult = deletedUser;
                }
                else
                {
                    string errorMessages = String.Empty;
                    foreach (var error in result.Errors)
                    {
                        errorMessages = $"*{error.Description}\n";
                    }

                    var deletedUserErrorModel = JsonSerializer.Serialize(new UserDto
                    {
                        ResultStatus = ResultStatus.Error,
                        Message =
                            $"{user.UserName} adlı kullanıcı adına sahip kullanıcı silinirken bazı hatalar oluştu.\n{errorMessages}",
                        User = user
                    });
                    return Json(deletedUserErrorModel);
                }
            }
            return Json(jsonResult);
        }

        [Authorize(Roles = "SuperAdmin,User.Update")]
        [HttpGet]
        public async Task<IActionResult> Duzenle(int userId)
        {
            var user = await UserManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user != null)
            {
                var userUpdateDto = Mapper.Map<UserUpdateDto>(user);
                return View(userUpdateDto);
            }
            else
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "SuperAdmin,User.Update")]
        [HttpPost]
        public async Task<IActionResult> Duzenle(UserUpdateDto userUpdateDto)
        {
            if (ModelState.IsValid)
            {
                var oldUser = await UserManager.FindByIdAsync(userUpdateDto.Id.ToString());
                if (userUpdateDto.Picture == null)
                {
                    userUpdateDto.Picture = "default-user-image.png";
                }

                var updatedUserDto = Mapper.Map(userUpdateDto, oldUser);
                var result = await UserManager.UpdateAsync(updatedUserDto);
                if (result.Succeeded)
                {
                    _toastNotification.AddSuccessToastMessage("Kullanıcı başarılı bir şekilde güncellendi.",
                        new ToastrOptions
                        {
                            Title = "Başarılı İşlem!"
                        });
                    return RedirectToAction("Index", "Kullanici");
                }
                else
                {
                    ModelState.AddModelError("", "Kullanıcı Eklenemedi.");
                }
            }
            return View(userUpdateDto);
        }

        [Authorize]
        [HttpGet]
        public async Task<ViewResult> ProfiliDuzenle()
        {
            var user = await UserManager.GetUserAsync(HttpContext.User);
            var updateDto = Mapper.Map<UserUpdateDto>(user);
            return View(updateDto);
        }

        [Authorize]
        [HttpPost]
        public async Task<ViewResult> ProfiliDuzenle(UserUpdateDto userUpdateDto)
        {
            if (ModelState.IsValid)
            {
                var oldUser = await UserManager.GetUserAsync(HttpContext.User);
                if (userUpdateDto.Picture == null)
                {
                    userUpdateDto.Picture = "default-user-image.png";
                }

                var updatedUser = Mapper.Map(userUpdateDto, oldUser);
                var result = await UserManager.UpdateAsync(updatedUser);
                if (result.Succeeded)
                {
                    _toastNotification.AddSuccessToastMessage("Bilgileriniz başarıyla güncellenmiştir.",
                        new ToastrOptions
                        {
                            Title = "Başarılı İşlem!"
                        });
                    return View(userUpdateDto);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return View(userUpdateDto);
                }
            }
            else
            {
                return View(userUpdateDto);
            }
        }

        [Authorize]
        [HttpGet]
        public ViewResult SifreDegistir()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SifreDegistir(UserPasswordChangeDto userPasswordChangeDto)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.GetUserAsync(HttpContext.User);
                var isVerified = await UserManager.CheckPasswordAsync(user, userPasswordChangeDto.CurrentPassword);
                if (isVerified)
                {
                    var result = await UserManager.ChangePasswordAsync(user, userPasswordChangeDto.CurrentPassword,
                        userPasswordChangeDto.NewPassword);
                    if (result.Succeeded)
                    {
                        await UserManager.UpdateSecurityStampAsync(user);
                        await _signInManager.SignOutAsync();
                        await _signInManager.PasswordSignInAsync(user, userPasswordChangeDto.NewPassword, true, false);
                        _toastNotification.AddSuccessToastMessage("Şifreniz başarıyla değiştirilmiştir.",
                            new ToastrOptions
                            {
                                Title = "Başarılı İşlem!"
                            });
                        return View();
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }

                        return View(userPasswordChangeDto);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Lütfen, girmiş olduğunuz şu anki şifrenizi kontrol ediniz.");
                    return View(userPasswordChangeDto);
                }
            }
            else
            {
                return View(userPasswordChangeDto);
            }
        }
    }
}