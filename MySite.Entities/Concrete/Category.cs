using System.Collections.Generic;
using MySite.Shared.Entities.Abstract;

namespace MySite.Entities.Concrete
{
    public class Category : EntityUserAndStatusBase, IEntity
    {
        public string Name { get; set; }
        public string SeoAuthor { get; set; }
        public string SeoTags { get; set; }
        public string SeoDescription { get; set; }
        public ICollection<Article> Articles { get; set; }
    }
}