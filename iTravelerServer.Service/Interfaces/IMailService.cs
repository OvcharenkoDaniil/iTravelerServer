using System.Collections.Generic;
using System.Threading.Tasks;
using iTravelerServer.Domain.Entities;
using iTravelerServer.Domain.Response;


namespace iTravelerServer.Service.Interfaces
{
    public interface IMailService
    {
        void MessageSender(string email, string messageText);

    }   
}