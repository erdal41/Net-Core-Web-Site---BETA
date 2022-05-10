using System.Collections.Generic;
using MySite.Shared.Entities.Abstract;

namespace MySite.Entities.Concrete
{
    public class Article : EntityUserAndStatusBase, IEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Picture { get; set; }
        public int ViewCount { get; set; } = 0;
        public int CommentCount { get; set; } = 0;
        public int CategoryId { get; set; }
        public int UserId { get; set; }
        public string SeoAuthor { get; set; }
        public string SeoTags { get; set; }
        public string SeoDescription { get; set; }
        public Category Category { get; set; }
        public User User { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}