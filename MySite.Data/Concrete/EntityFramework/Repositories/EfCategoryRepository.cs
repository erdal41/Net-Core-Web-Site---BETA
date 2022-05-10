using Microsoft.EntityFrameworkCore;
using MySite.Data.Abstract;
using MySite.Data.Concrete.EntityFramework.Contexts;
using MySite.Entities.Concrete;
using MySite.Shared.Data.Concrete.EntityFramework;
using System.Threading.Tasks;

namespace MySite.Data.Concrete.EntityFramework.Repositories
{
    public class EfCategoryRepository : EfEntityRepositoryBase<Category>, ICategoryRepository
    {
        public EfCategoryRepository(DbContext context) : base(context)
        {
        }

        public async Task<Category> GetById(int categoryId)
        {
            return await MyBlogContext.Categories.SingleOrDefaultAsync(c => c.Id == categoryId);
        }

        private MyBlogContext MyBlogContext
        {
            get
            {
                return _context as MyBlogContext;
            }
        }
    }
}