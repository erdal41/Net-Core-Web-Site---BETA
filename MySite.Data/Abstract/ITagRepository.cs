using System.Threading.Tasks;
using MySite.Entities.Concrete;
using MySite.Shared.Data.Abstract;

namespace MySite.Data.Abstract
{
    public interface ITagRepository : IEntityRepository<Tag>
    {
        Task<Tag> GetById(int tagId);
    }
}