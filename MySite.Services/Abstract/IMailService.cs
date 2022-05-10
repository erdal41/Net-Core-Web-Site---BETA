using MySite.Entities.Dtos;
using MySite.Shared.Utilities.Results.Abstract;

namespace MySite.Services.Abstract
{
    public interface IMailService
    {
        IResult Send(EmailSendDto emailSendDto);

        IResult SendContactEmail(EmailSendDto emailSendDto);
    }
}