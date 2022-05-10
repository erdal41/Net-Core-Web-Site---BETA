using System.Threading.Tasks;
using MySite.Entities.Concrete;
using MySite.Shared.Data.Abstract;

namespace MySite.Data.Abstract
{
    public interface ICategoryRepository : IEntityRepository<Category>
    {
        Task<Category> GetById(int categoryId);
    }
}