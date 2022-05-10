using Microsoft.EntityFrameworkCore;
using MySite.Data.Abstract;
using MySite.Data.Concrete.EntityFramework.Contexts;
using MySite.Entities.Concrete;
using MySite.Shared.Data.Concrete.EntityFramework;
using System.Threading.Tasks;

namespace MySite.Data.Concrete.EntityFramework.Repositories
{
    public class EfTagRepository : EfEntityRepositoryBase<Tag>, ITagRepository
    {
        public EfTagRepository(DbContext context) : base(context)
        {
        }

        public async Task<Tag> GetById(int tagId)
        {
            return await MyBlogContext.Tags.SingleOrDefaultAsync(t => t.Id == tagId);
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