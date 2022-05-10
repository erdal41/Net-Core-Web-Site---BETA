using Microsoft.Extensions.Options;
using MySite.Entities.Concrete;
using MySite.Entities.Dtos;
using MySite.Services.Abstract;
using MySite.Shared.Utilities.Results.Concrete;
using MySite.Shared.Utilities.Results.Abstract;
using MySite.Shared.Utilities.Results.ComplexTypes;
using System.Net;
using System.Net.Mail;

namespace MySite.Services.Concrete
{
    public class MailManager : IMailService
    {
        private readonly SmtpSettings _smtpSettings;

        public MailManager(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        public IResult Send(EmailSendDto emailSendDto)
        {
            MailMessage message = new MailMessage
            {
                From = new MailAddress(_smtpSettings.SenderEmail),
                To = { new MailAddress(emailSendDto.Email) },
                Subject = emailSendDto.Subject,
                IsBodyHtml = true,
                Body = emailSendDto.Message,
            };
            SmtpClient smtpClient = new SmtpClient
            {
                Host = _smtpSettings.Server,
                Port = _smtpSettings.Port,
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
            smtpClient.Send(message);
            return new Result(ResultStatus.Success, "E-Postanız başarılı bir şekilde gönderilmiştir.");
        }

        public IResult SendContactEmail(EmailSendDto emailSendDto)
        {
            MailMessage message = new MailMessage
            {
                From = new MailAddress(_smtpSettings.SenderEmail),
                To = { new MailAddress("erdaldemiral41@gmail.com") },
                Subject = emailSendDto.Subject,
                IsBodyHtml = true,
                Body = $"Gönderen Kişi: {emailSendDto.Name}<br/>Gönderen E-Posta Adresi: {emailSendDto.Email}<br/><br/>{emailSendDto.Message}",
            };
            SmtpClient smtpClient = new SmtpClient
            {
                Host = _smtpSettings.Server,
                Port = _smtpSettings.Port,
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
            smtpClient.Send(message);

            return new Result(ResultStatus.Success, "E-Postanız başarılı bir şekilde gönderilmiştir.");
        }
    }
}