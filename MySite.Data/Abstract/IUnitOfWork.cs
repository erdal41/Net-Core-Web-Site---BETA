using System;
using System.Threading.Tasks;

namespace MySite.Data.Abstract
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IArticleRepository Articles { get; }
        ICategoryRepository Categories { get; }
        ITagRepository Tags { get; }
        ICommentRepository Comments { get; }
        IUploadRepository Uploads { get; }
        Task<int> SaveAsync();
    }
}