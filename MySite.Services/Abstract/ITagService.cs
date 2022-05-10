using MySite.Entities.Dtos;
using MySite.Shared.Utilities.Results.Abstract;
using System.Threading.Tasks;

namespace MySite.Services.Abstract
{
    public interface ITagService
    {
        Task<IDataResult<TagDto>> GetAsync(int tagId);

        /// <summary>
        /// Verilen ID parametresine ait kategorinin CategoryUpdateDto temsilini geriye döner.
        /// </summary>
        /// <param name="tagId">0'dan büyük integer bir ID değeri</param>
        /// <returns>Asenkron bir operasyon ile Task olarak işlem sonucu DataResult tipinde geriye döner.</returns>
        Task<IDataResult<TagUpdateDto>> GetTagUpdateDtoAsync(int tagId);

        Task<IDataResult<TagListDto>> GetAllAsync();

        Task<IDataResult<TagListDto>> GetAllByNonDeletedAsync();

        Task<IDataResult<TagListDto>> GetAllByNonDeletedAndActiveAsync();

        Task<IDataResult<TagListDto>> GetAllByDeletedAsync();

        /// <summary>
        /// Verilen CategoryAddDto ve CreatedByName parametrelerine ait bilgiler ile yeni bir Category ekler.
        /// </summary>
        /// <param name="tagAddDto">categoryAddDto tipinde eklenecek kategori bilgileri</param>
        /// <param name="createdByName">string tipinde kullanıcının kullanıcı adı</param>
        /// <returns>Asenkron bir operasyon ile Task olarak bizlere ekleme işleminin sonucunu DataResult tipinde döner.</returns>
        Task<IDataResult<TagDto>> AddAsync(TagAddDto tagAddDto, string createdByName);

        Task<IDataResult<TagDto>> UpdateAsync(TagUpdateDto tagUpdateDto, string modifiedByName);

        Task<IDataResult<TagDto>> DeleteAsync(int tagId, string modifiedByName);

        Task<IDataResult<TagDto>> UndoDeleteAsync(int tagId, string modifiedByName);

        Task<IResult> HardDeleteAsync(int tagId);

        Task<IDataResult<int>> CountAsync();

        Task<IDataResult<int>> CountByNonDeletedAsync();
    }
}