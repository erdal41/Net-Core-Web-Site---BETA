using MySite.Data.Abstract;
using MySite.Data.Concrete.EntityFramework.Contexts;
using MySite.Data.Concrete.EntityFramework.Repositories;
using System.Threading.Tasks;

namespace MySite.Data.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyBlogContext _context;
        private readonly EfArticleRepository _articleRepository;
        private readonly EfCategoryRepository _categoryRepository;
        private readonly EfTagRepository _tagRepository;
        private readonly EfCommentRepository _commentRepository;
        private readonly EfUploadRepository _uploadRepository;


        public UnitOfWork(MyBlogContext context)
        {
            _context = context;
        }

        public IArticleRepository Articles => _articleRepository ?? new EfArticleRepository(_context);
        public ICategoryRepository Categories => _categoryRepository ?? new EfCategoryRepository(_context);
        public ITagRepository Tags => _tagRepository ?? new EfTagRepository(_context);
        public ICommentRepository Comments => _commentRepository ?? new EfCommentRepository(_context);
        public IUploadRepository Uploads => _uploadRepository ?? new EfUploadRepository(_context);

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }
    }
}