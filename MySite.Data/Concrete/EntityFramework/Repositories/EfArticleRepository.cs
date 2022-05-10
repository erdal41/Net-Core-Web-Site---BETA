using Microsoft.EntityFrameworkCore;
using MySite.Data.Abstract;
using MySite.Entities.Concrete;
using MySite.Shared.Data.Concrete.EntityFramework;

namespace MySite.Data.Concrete.EntityFramework.Repositories
{
    public class EfArticleRepository : EfEntityRepositoryBase<Article>, IArticleRepository
    {
        public EfArticleRepository(DbContext context) : base(context)
        {
        }
    }
}