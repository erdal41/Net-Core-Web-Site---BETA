using Microsoft.AspNetCore.Http;
using MySite.Entities.Dtos;
using System.Collections.Generic;

namespace MySite.Mvc.Areas.Admin.Models
{
    public class UploadAddAjaxViewModel
    {
        public UploadAddDto UploadAddDto { get; set; }
        public UploadDto UploadDto { get; set; }
        public IList<IFormFile> FormFiles { get; set; }
    }
}