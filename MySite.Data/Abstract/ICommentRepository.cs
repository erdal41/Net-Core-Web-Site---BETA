using MySite.Entities.Concrete;
using MySite.Shared.Data.Abstract;

namespace MySite.Data.Abstract
{
    public interface ICommentRepository : IEntityRepository<Comment> { }
}