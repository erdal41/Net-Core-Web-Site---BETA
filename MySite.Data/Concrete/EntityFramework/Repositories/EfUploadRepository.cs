using Microsoft.EntityFrameworkCore;
using MySite.Data.Abstract;
using MySite.Entities.Concrete;
using MySite.Shared.Data.Concrete.EntityFramework;

namespace MySite.Data.Concrete.EntityFramework.Repositories
{
    public class EfUploadRepository : EfEntityRepositoryBase<Upload>, IUploadRepository
    {
        public EfUploadRepository(DbContext context) : base(context)
        {
        }
    }
}