using MySite.Entities.Dtos;

namespace MySite.Mvc.Areas.Admin.Models
{
    public class TagUpdateAjaxViewModel
    {
        public TagUpdateDto TagUpdateDto { get; set; }
        public string TagUpdatePartial { get; set; }
        public TagDto TagDto { get; set; }
    }
}