using MySite.Entities.Dtos;

namespace MySite.Mvc.Areas.Admin.Models
{
    public class TagAddAjaxViewModel
    {
        public TagAddDto TagAddDto { get; set; }
        public string TagAddPartial { get; set; }
        public TagDto TagDto { get; set; }
    }
}