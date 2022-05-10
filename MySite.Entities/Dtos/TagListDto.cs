using MySite.Entities.Concrete;
using MySite.Shared.Entities.Abstract;
using System.Collections.Generic;

namespace MySite.Entities.Dtos
{
    public class TagListDto : DtoGetBase
    {
        public IList<Tag> Tags { get; set; }
    }
}