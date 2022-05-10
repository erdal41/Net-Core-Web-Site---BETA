using MySite.Entities.Concrete;
using MySite.Shared.Entities.Abstract;

namespace MySite.Entities.Dtos
{
    public class UploadDto : DtoGetBase
    {
        public Upload Upload { get; set; }
    }
}