using MySite.Entities.Dtos;
using MySite.Shared.Utilities.Results.Abstract;
using System.Threading.Tasks;

namespace MySite.Services.Abstract
{
    public interface IUploadService
    {
        Task<IDataResult<UploadDto>> GetAsync(int uploadId);
        Task<IDataResult<UploadUpdateDto>> GetUploadUpdateDtoAsync(int uploadId);
        Task<IDataResult<UploadListDto>> GetAllAsync();
        Task<IDataResult<UploadDto>> AddAsync(UploadAddDto uploadAddDto, string createdByName);
        Task<IDataResult<UploadDto>> UpdateAsync(UploadUpdateDto uploadUpdateDto, string modifiedByName);
        Task<IResult> DeleteAsync(int uploadId);
        Task<IDataResult<int>> CountAsync();
    }
}