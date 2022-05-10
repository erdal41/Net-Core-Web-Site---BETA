using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using MySite.Entities.Dtos;
using MySite.Mvc.Helpers.Abstract;
using MySite.Shared.Utilities.Results.Abstract;
using MySite.Shared.Utilities.Results.ComplexTypes;
using MySite.Shared.Utilities.Results.Concrete;
using NToastNotify;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MySite.Mvc.Helpers.Concrete
{
    public class ImageHelper : IImageHelper
    {
        private readonly IWebHostEnvironment _env;
        private readonly IToastNotification _toastNotification;
        private readonly string _wwwroot;
        private const string imgFolder = "assets/img";

        public ImageHelper(IWebHostEnvironment env, IToastNotification toastNotification)
        {
            _env = env;
            _wwwroot = _env.WebRootPath;
            _toastNotification = toastNotification;
        }

        public async Task<IDataResult<FileUploadDto>> Upload(IFormFile uploadFile, string createdByName)
        {
            string dosyaYolu = $"{_wwwroot}/{imgFolder}";
            string contentType = uploadFile.ContentType;

            if (!Directory.Exists(dosyaYolu))
            {
                Directory.CreateDirectory($"{_wwwroot}/{imgFolder}");
            }

            /* Resimin yüklenme sırasındaki ilk adı oldFileName adlı değişkene atanır. */
            string oldFileName = Path.GetFileNameWithoutExtension(uploadFile.FileName);

            /* Resimin uzantısı fileExtension adlı değişkene atanır. */
            string fileExtension = Path.GetExtension(uploadFile.FileName);

            Regex regex = new("[*'\",._&#^@]");
            string newFileNameNotExtension = regex.Replace(oldFileName, string.Empty);
            /* Kendi parametrelerimiz ile sistemimize uygun yeni bir dosya yolu (path) oluşturulur. */

            string timeFormat = DateTime.Now.ToString("yyMMddHHmmss");

            string newFileNameAndExtension = newFileNameNotExtension + "-" + timeFormat + fileExtension;

            var path = Path.Combine($"{_wwwroot}/{imgFolder}", newFileNameAndExtension);

            /* Sistemimiz için oluşturulan yeni dosya yoluna resim kopyalanır. */
            await using (var stream = new FileStream(path, FileMode.Create))
            {
                await uploadFile.CopyToAsync(stream);
            }

            /* Resim tipine göre kullanıcı için bir mesaj oluşturulur. */
            string nameMessage = $"{newFileNameAndExtension} adlı medya dosyası başarıyla yüklenmiştir.";

            _toastNotification.AddSuccessToastMessage(nameMessage, new ToastrOptions
            {
                Title = "Başarılı İşlem!"
            });

            return new DataResult<FileUploadDto>(ResultStatus.Success, nameMessage, new FileUploadDto
            {
                FileFullName = newFileNameAndExtension,
                Path = path,
                ContentType = contentType,
                Extension = fileExtension,
                Size = uploadFile.Length
            });
        }

        public IDataResult<FileDeleteDto> Delete(string uploadFileName)
        {
            var fileToDelete = Path.Combine($"{_wwwroot}/{imgFolder}/", uploadFileName);
          
            if (File.Exists(fileToDelete))
            {
                var fileInfo = new FileInfo(fileToDelete);
                
                var imageDeletedDto = new FileDeleteDto
                {
                    FileFullName = uploadFileName,
                    Extension = fileInfo.Extension,
                    Path = fileInfo.FullName,
                    Size = fileInfo.Length,
                };
                File.Delete(fileToDelete);
                return new DataResult<FileDeleteDto>(ResultStatus.Success, imageDeletedDto);
            }
            else
            {
                return new DataResult<FileDeleteDto>(ResultStatus.Error, $"Böyle bir medya dosyası bulunamadı.", null);
            }
        }

        public IDataResult<FileDeleteDto> MultiDelete(string uploadFileName)
        {
            var fileToDelete = Path.Combine($"{_wwwroot}/{imgFolder}/", uploadFileName);
            if (File.Exists(fileToDelete))
            {
                var fileInfo = new FileInfo(fileToDelete);
                var imageDeletedDto = new FileDeleteDto
                {
                    FileFullName = uploadFileName,
                    Extension = fileInfo.Extension,
                    Path = fileInfo.FullName,
                    Size = fileInfo.Length
                };
                File.Delete(fileToDelete);
                return new DataResult<FileDeleteDto>(ResultStatus.Success, imageDeletedDto);
            }
            else
            {
                return new DataResult<FileDeleteDto>(ResultStatus.Error, $"Böyle bir medya dosyası bulunamadı.", null);
            }
        }
    }
}