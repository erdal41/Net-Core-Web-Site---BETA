using AutoMapper;
using MySite.Data.Abstract;
using MySite.Entities.Concrete;
using MySite.Entities.Dtos;
using MySite.Services.Abstract;
using MySite.Services.Utilities;
using MySite.Shared.Utilities.Results.Concrete;
using MySite.Shared.Utilities.Results.Abstract;
using MySite.Shared.Utilities.Results.ComplexTypes;
using System;
using System.Threading.Tasks;

namespace MySite.Services.Concrete
{
    public class TagManager : ManagerBase, ITagService
    {
        public TagManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task<IDataResult<TagDto>> GetAsync(int tagId)
        {
            var tag = await UnitOfWork.Tags.GetAsync(c => c.Id == tagId);
            if (tag != null)
            {
                return new DataResult<TagDto>(ResultStatus.Success, new TagDto
                {
                    Tag = tag,
                    ResultStatus = ResultStatus.Success
                });
            }
            return new DataResult<TagDto>(ResultStatus.Error, Messages.Tag.NotFound(isPlural: false), new TagDto
            {
                Tag = null,
                ResultStatus = ResultStatus.Error,
                Message = Messages.Tag.NotFound(isPlural: false)
            });
        }

        /// <summary>
        /// Verilen ID parametresine ait kategorinin TagUpdateDto temsilini geriye döner.
        /// </summary>
        /// <param name="tagId">0'dan büyük integer bir ID değeri</param>
        /// <returns>Asenkron bir operasyon ile Task olarak işlem sonucu DataResult tipinde geriye döner.</returns>
        public async Task<IDataResult<TagUpdateDto>> GetTagUpdateDtoAsync(int tagId)
        {
            var result = await UnitOfWork.Tags.AnyAsync(t => t.Id == tagId);
            if (result)
            {
                var tag = await UnitOfWork.Tags.GetAsync(t => t.Id == tagId);
                var tagUpdateDto = Mapper.Map<TagUpdateDto>(tag);
                return new DataResult<TagUpdateDto>(ResultStatus.Success, tagUpdateDto);
            }
            else
            {
                return new DataResult<TagUpdateDto>(ResultStatus.Error, Messages.Tag.NotFound(isPlural: false), null);
            }
        }

        public async Task<IDataResult<TagListDto>> GetAllAsync()
        {
            var tags = await UnitOfWork.Tags.GetAllAsync(null);
            if (tags.Count > -1)
            {
                return new DataResult<TagListDto>(ResultStatus.Success, new TagListDto
                {
                    Tags = tags,
                    ResultStatus = ResultStatus.Success
                });
            }
            return new DataResult<TagListDto>(ResultStatus.Error, Messages.Tag.NotFound(isPlural: true), new TagListDto
            {
                Tags = null,
                ResultStatus = ResultStatus.Error,
                Message = Messages.Tag.NotFound(isPlural: true)
            });
        }

        public async Task<IDataResult<TagListDto>> GetAllByNonDeletedAsync()
        {
            var tags = await UnitOfWork.Tags.GetAllAsync(c => !c.IsDeleted);
            if (tags.Count > -1)
            {
                return new DataResult<TagListDto>(ResultStatus.Success, new TagListDto
                {
                    Tags = tags,
                    ResultStatus = ResultStatus.Success
                });
            }
            return new DataResult<TagListDto>(ResultStatus.Error, Messages.Tag.NotFound(isPlural: true), new TagListDto
            {
                Tags = null,
                ResultStatus = ResultStatus.Error,
                Message = Messages.Tag.NotFound(isPlural: true)
            });
        }

        public async Task<IDataResult<TagListDto>> GetAllByNonDeletedAndActiveAsync()
        {
            var tags = await UnitOfWork.Tags.GetAllAsync(c => !c.IsDeleted && c.IsActive);
            if (tags.Count > -1)
            {
                return new DataResult<TagListDto>(ResultStatus.Success, new TagListDto
                {
                    Tags = tags,
                    ResultStatus = ResultStatus.Success
                });
            }
            return new DataResult<TagListDto>(ResultStatus.Error, Messages.Tag.NotFound(isPlural: true), null);
        }

        public async Task<IDataResult<TagListDto>> GetAllByDeletedAsync()
        {
            var tags = await UnitOfWork.Tags.GetAllAsync(c => c.IsDeleted);
            if (tags.Count > -1)
            {
                return new DataResult<TagListDto>(ResultStatus.Success, new TagListDto
                {
                    Tags = tags,
                    ResultStatus = ResultStatus.Success
                });
            }
            return new DataResult<TagListDto>(ResultStatus.Error, Messages.Tag.NotFound(isPlural: true), null);
        }

        /// <summary>
        /// Verilen TagAddDto ve CreatedByName parametrelerine ait bilgiler ile yeni bir Tag ekler.
        /// </summary>
        /// <param name="tagAddDto">tagAddDto tipinde eklenecek kategori bilgileri</param>
        /// <param name="createdByName">string tipinde kullanıcının kullanıcı adı</param>
        /// <returns>Asenkron bir operasyon ile Task olarak bizlere ekleme işleminin sonucunu DataResult tipinde döner.</returns>
        public async Task<IDataResult<TagDto>> AddAsync(TagAddDto tagAddDto, string createdByName)
        {
            var tag = Mapper.Map<Tag>(tagAddDto);
            tag.CreatedByName = createdByName;
            tag.ModifiedByName = createdByName;
            var addedTag = await UnitOfWork.Tags.AddAsync(tag);
            await UnitOfWork.SaveAsync();
            return new DataResult<TagDto>(ResultStatus.Success, Messages.Tag.Add(addedTag.Name), new TagDto
            {
                Tag = addedTag,
                ResultStatus = ResultStatus.Success,
                Message = Messages.Tag.Add(addedTag.Name)
            });
        }

        public async Task<IDataResult<TagDto>> UpdateAsync(TagUpdateDto tagUpdateDto, string modifiedByName)
        {
            var oldTag = await UnitOfWork.Tags.GetAsync(c => c.Id == tagUpdateDto.Id);
            var tag = Mapper.Map<TagUpdateDto, Tag>(tagUpdateDto, oldTag);
            tag.ModifiedByName = modifiedByName;
            var updatedTag = await UnitOfWork.Tags.UpdateAsync(tag);
            await UnitOfWork.SaveAsync();
            return new DataResult<TagDto>(ResultStatus.Success, Messages.Tag.Update(updatedTag.Name), new TagDto
            {
                Tag = updatedTag,
                ResultStatus = ResultStatus.Success,
                Message = Messages.Tag.Update(updatedTag.Name)
            });
        }

        public async Task<IDataResult<TagDto>> DeleteAsync(int tagId, string modifiedByName)
        {
            var tag = await UnitOfWork.Tags.GetAsync(t => t.Id == tagId);
            if (tag != null)
            {
                tag.IsDeleted = true;
                tag.IsActive = false;
                tag.ModifiedByName = modifiedByName;
                tag.ModifiedDate = DateTime.Now;
                var deletedTag = await UnitOfWork.Tags.UpdateAsync(tag);
                await UnitOfWork.SaveAsync();
                return new DataResult<TagDto>(ResultStatus.Success, Messages.Tag.Delete(deletedTag.Name), new TagDto
                {
                    Tag = deletedTag,
                    ResultStatus = ResultStatus.Success,
                    Message = Messages.Tag.Delete(deletedTag.Name)
                });
            }
            return new DataResult<TagDto>(ResultStatus.Error, Messages.Tag.NotFound(isPlural: false), new TagDto
            {
                Tag = null,
                ResultStatus = ResultStatus.Error,
                Message = Messages.Tag.NotFound(isPlural: false)
            });
        }

        public async Task<IDataResult<TagDto>> UndoDeleteAsync(int tagId, string modifiedByName)
        {
            var tag = await UnitOfWork.Tags.GetAsync(t => t.Id == tagId);
            if (tag != null)
            {
                tag.IsDeleted = false;
                tag.IsActive = true;
                tag.ModifiedByName = modifiedByName;
                tag.ModifiedDate = DateTime.Now;
                var deletedTag = await UnitOfWork.Tags.UpdateAsync(tag);
                await UnitOfWork.SaveAsync();
                return new DataResult<TagDto>(ResultStatus.Success, Messages.Tag.UndoDelete(deletedTag.Name), new TagDto
                {
                    Tag = deletedTag,
                    ResultStatus = ResultStatus.Success,
                    Message = Messages.Tag.UndoDelete(deletedTag.Name)
                });
            }
            return new DataResult<TagDto>(ResultStatus.Error, Messages.Tag.NotFound(isPlural: false), new TagDto
            {
                Tag = null,
                ResultStatus = ResultStatus.Error,
                Message = Messages.Tag.NotFound(isPlural: false)
            });
        }

        public async Task<IResult> HardDeleteAsync(int tagId)
        {
            var tag = await UnitOfWork.Tags.GetAsync(t => t.Id == tagId);
            if (tag != null)
            {
                await UnitOfWork.Tags.DeleteAsync(tag);
                await UnitOfWork.SaveAsync();
                return new Result(ResultStatus.Success, Messages.Tag.HardDelete(tag.Name));
            }
            return new Result(ResultStatus.Error, Messages.Tag.NotFound(isPlural: false));
        }

        public async Task<IDataResult<int>> CountAsync()
        {
            var tagsCount = await UnitOfWork.Tags.CountAsync();
            if (tagsCount > -1)
            {
                return new DataResult<int>(ResultStatus.Success, tagsCount);
            }
            else
            {
                return new DataResult<int>(ResultStatus.Error, $"Beklenmeyen bir hata ile karşılaşıldı.", -1);
            }
        }

        public async Task<IDataResult<int>> CountByNonDeletedAsync()
        {
            var tagsCount = await UnitOfWork.Tags.CountAsync(c => !c.IsDeleted);
            if (tagsCount > -1)
            {
                return new DataResult<int>(ResultStatus.Success, tagsCount);
            }
            else
            {
                return new DataResult<int>(ResultStatus.Error, $"Beklenmeyen bir hata ile karşılaşıldı.", -1);
            }
        }
    }
}