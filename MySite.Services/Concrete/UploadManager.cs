using AutoMapper;
using MySite.Data.Abstract;
using MySite.Entities.Concrete;
using MySite.Entities.Dtos;
using MySite.Services.Abstract;
using MySite.Services.Utilities;
using MySite.Shared.Utilities.Results.Concrete;
using MySite.Shared.Utilities.Results.Abstract;
using MySite.Shared.Utilities.Results.ComplexTypes;
using System.Threading.Tasks;

namespace MySite.Services.Concrete
{
    public class UploadManager : ManagerBase, IUploadService
    {
        public UploadManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task<IDataResult<UploadDto>> GetAsync(int uploadId)
        {
            var upload = await UnitOfWork.Uploads.GetAsync(u => u.Id == uploadId);
            if (upload != null)
            {
                return new DataResult<UploadDto>(ResultStatus.Success, new UploadDto
                {
                    Upload = upload,
                    ResultStatus = ResultStatus.Success
                });
            }
            return new DataResult<UploadDto>(ResultStatus.Error, Messages.Upload.NotFound(isPlural: false), null);
        }

        public async Task<IDataResult<UploadUpdateDto>> GetUploadUpdateDtoAsync(int uploadId)
        {
            var result = await UnitOfWork.Uploads.AnyAsync(u => u.Id == uploadId);
            if (result)
            {
                var upload = await UnitOfWork.Uploads.GetAsync(u => u.Id == uploadId);
                var uploadUpdateDto = Mapper.Map<UploadUpdateDto>(upload);
                return new DataResult<UploadUpdateDto>(ResultStatus.Success, uploadUpdateDto);
            }
            else
            {
                return new DataResult<UploadUpdateDto>(ResultStatus.Error, Messages.Upload.NotFound(isPlural: false), null);
            }
        }

        public async Task<IDataResult<UploadListDto>> GetAllAsync()
        {
            var uploads = await UnitOfWork.Uploads.GetAllAsync(null);
            if (uploads.Count > -1)
            {
                return new DataResult<UploadListDto>(ResultStatus.Success, new UploadListDto
                {
                    Uploads = uploads,
                    ResultStatus = ResultStatus.Success
                });
            }
            return new DataResult<UploadListDto>(ResultStatus.Error, Messages.Upload.NotFound(isPlural: true), new UploadListDto
            {
                Uploads = null,
                ResultStatus = ResultStatus.Error,
                Message = Messages.Upload.NotFound(isPlural: true)
            });
        }

        public async Task<IDataResult<UploadDto>> AddAsync(UploadAddDto uploadAddDto, string createdByName)
        {
            var upload = Mapper.Map<Upload>(uploadAddDto);
            upload.CreatedByName = createdByName;
            upload.ModifiedByName = createdByName;
            var addedUpload = await UnitOfWork.Uploads.AddAsync(upload);
            await UnitOfWork.SaveAsync();
            return new DataResult<UploadDto>(ResultStatus.Success, Messages.Upload.Add(addedUpload.FileName), new UploadDto
            {
                Upload = addedUpload,
                ResultStatus = ResultStatus.Success,
                Message = Messages.Upload.Add(addedUpload.FileName)
            });
        }

        public async Task<IDataResult<UploadDto>> UpdateAsync(UploadUpdateDto uploadUpdateDto, string modifiedByName)
        {
            var oldUpload = await UnitOfWork.Uploads.GetAsync(u => u.Id == uploadUpdateDto.Id);
            var upload = Mapper.Map<UploadUpdateDto, Upload>(uploadUpdateDto, oldUpload);
            upload.ModifiedByName = modifiedByName;
            var updateUpload = await UnitOfWork.Uploads.UpdateAsync(upload);
            await UnitOfWork.SaveAsync();
            return new DataResult<UploadDto>(ResultStatus.Success, Messages.Upload.Update(upload.FileName), new UploadDto
            {
                Upload = updateUpload,
                ResultStatus = ResultStatus.Success,
                Message = Messages.Upload.Update(updateUpload.FileName)
            });
        }

        public async Task<IResult> DeleteAsync(int uploadId)
        {
            var result = await UnitOfWork.Uploads.AnyAsync(u => u.Id == uploadId);
            if (result)
            {
                var upload = await UnitOfWork.Uploads.GetAsync(u => u.Id == uploadId);
                await UnitOfWork.Uploads.DeleteAsync(upload);
                await UnitOfWork.SaveAsync();
                return new Result(ResultStatus.Success, Messages.Upload.Delete(upload.FileName));
            }
            return new Result(ResultStatus.Error, Messages.Upload.NotFound(isPlural: false));
        }

        public async Task<IDataResult<int>> CountAsync()
        {
            var uploadsCount = await UnitOfWork.Uploads.CountAsync();
            if (uploadsCount > -1)
            {
                return new DataResult<int>(ResultStatus.Success, uploadsCount);
            }
            else
            {
                return new DataResult<int>(ResultStatus.Error, $"Beklenmeyen bir hata ile karşılaşıldı.", -1);
            }
        }
    }
}