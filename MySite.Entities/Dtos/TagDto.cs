using MySite.Entities.Concrete;
using MySite.Shared.Entities.Abstract;

namespace MySite.Entities.Dtos
{
    public class TagDto : DtoGetBase
    {
        public Tag Tag { get; set; }
    }
}