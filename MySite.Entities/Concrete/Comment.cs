using MySite.Shared.Data.Abstract;
using MySite.Shared.Entities.Abstract;

namespace MySite.Entities.Concrete
{
    public class Comment : EntityUserAndStatusBase, IEntity
    {
        public string Text { get; set; }
        public int ArticleId { get; set; }
        public Article Article { get; set; }
    }
}