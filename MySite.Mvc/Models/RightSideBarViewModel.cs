using MySite.Entities.Concrete;
using System.Collections.Generic;

namespace MySite.Mvc.Models
{
    public class RightSideBarViewModel
    {
        public IList<Category> Categories { get; set; }
        public IList<Article> Articles { get; set; }
        public IList<Comment> Comments { get; set; }
    }
}