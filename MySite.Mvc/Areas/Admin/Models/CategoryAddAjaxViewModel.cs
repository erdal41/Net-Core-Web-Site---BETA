using MySite.Entities.Dtos;

namespace MySite.Mvc.Areas.Admin.Models
{
    public class CategoryAddAjaxViewModel
    {
        public CategoryAddDto CategoryAddDto { get; set; }
        public CategoryDto CategoryDto { get; set; }
    }
}