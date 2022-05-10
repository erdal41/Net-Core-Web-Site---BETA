using MySite.Entities.Concrete;
using MySite.Shared.Entities.Abstract;

namespace MySite.Entities.Dtos
{
    public class CategoryDto : DtoGetBase
    {
        public Category Category { get; set; }
    }
}