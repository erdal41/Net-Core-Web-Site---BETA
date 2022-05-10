using MySite.Entities.Dtos;

namespace MySite.Mvc.Areas.Admin.Models
{
    public class UploadUpdateAjaxViewModel
    {
        public UploadUpdateDto UploadUpdateDto { get; set; }
        public string UploadUpdatePartial { get; set; }
        public UploadDto UploadDto { get; set; }
    }
}