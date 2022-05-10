using Microsoft.EntityFrameworkCore;
using MySite.Data.Abstract;
using MySite.Entities.Concrete;
using MySite.Shared.Data.Concrete.EntityFramework;

namespace MySite.Data.Concrete.EntityFramework.Repositories
{
    public class EfCommentRepository : EfEntityRepositoryBase<Comment>, ICommentRepository
    {
        public EfCommentRepository(DbContext context) : base(context)
        {
        }
    }
}