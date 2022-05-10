using MySite.Entities.Concrete;
using MySite.Shared.Entities.Abstract;
using System.Collections.Generic;

namespace MySite.Entities.Dtos
{
    public class CategoryListDto : DtoGetBase
    {
        public IList<Category> Categories { get; set; }
    }
}