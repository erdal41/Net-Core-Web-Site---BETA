using MySite.Entities.Concrete;
using MySite.Shared.Entities.Abstract;
using System.Collections.Generic;

namespace MySite.Entities.Dtos
{
    public class UploadListDto : DtoGetBase
    {
        public IList<Upload> Uploads { get; set; }
    }
}