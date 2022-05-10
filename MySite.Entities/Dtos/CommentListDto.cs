using MySite.Entities.Concrete;
using System.Collections.Generic;

namespace MySite.Entities.Dtos
{
    public class CommentListDto
    {
        public IList<Comment> Comments { get; set; }
    }
}