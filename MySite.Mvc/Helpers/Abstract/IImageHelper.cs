using Microsoft.AspNetCore.Http;
using MySite.Entities.Dtos;
using MySite.Shared.Utilities.Results.Abstract;
using System.Threading.Tasks;

namespace MySite.Mvc.Helpers.Abstract
{
    public interface IImageHelper
    {
        Task<IDataResult<FileUploadDto>> Upload(IFormFile uploadFile, string createdByName);

        IDataResult<FileDeleteDto> Delete(string uploadFileName);
    }
}