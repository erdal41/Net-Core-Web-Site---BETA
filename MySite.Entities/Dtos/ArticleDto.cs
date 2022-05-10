using MySite.Entities.Concrete;
using MySite.Shared.Entities.Abstract;

namespace MySite.Entities.Dtos
{
    public class ArticleDto : DtoGetBase
    {
        public Article Article { get; set; }
    }
}