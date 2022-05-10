using MySite.Entities.Concrete;
using MySite.Shared.Entities.Abstract;
using System.Collections.Generic;

namespace MySite.Entities.Dtos
{
    public class ArticleListDto : DtoGetBase
    {
        public IList<Article> Articles { get; set; }
        public int? CategoryId { get; set; }
    }
}