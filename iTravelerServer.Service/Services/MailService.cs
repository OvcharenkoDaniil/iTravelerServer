using iTravelerServer.DAL.Interfaces;
using iTravelerServer.Domain.Entities;
using iTravelerServer.Domain.Enum;
using iTravelerServer.Domain.Response;
using iTravelerServer.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;

namespace iTravelerServer.Service.Services;

public class MailService : IMailService
{
    private readonly IBaseRepository<Transfer> _transferRepository;

    public MailService(IBaseRepository<Transfer> transferRepository)
    {
        _transferRepository = transferRepository;
    }


    private string mailCode;

    public string MailCode { get; set; }
    

    public void MessageSender(string email,string messageText)
    {
        try
        {
            MailAddress from = new MailAddress("itraveler.oficial@gmail.com", "Администрация iTraveler");
            // кому отправляем
            MailAddress to = new MailAddress(email);
            // создаем объект сообщения
            MailMessage message = new MailMessage(from, to);
            // тема письма
            message.Subject = "Бронирование";
            // текст письма
            message.Body = messageText;
            // письмо представляет код html
            message.IsBodyHtml = true;
            // адрес smtp-сервера и порт, с которого будем отправлять письмо
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            // логин и пароль
            smtp.Credentials = new NetworkCredential("itraveler.oficial@gmail.com", "xxxxziiyykqskamh");
            smtp.EnableSsl = true;
            smtp.Send(message);
        }
        catch
        {
        }
    }
}