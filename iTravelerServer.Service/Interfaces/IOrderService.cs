using System.Collections.Generic;
using System.Threading.Tasks;
using iTravelerServer.Domain.Entities;
using iTravelerServer.Domain.Response;
using iTravelerServer.Domain.ViewModels.FlightVM;


namespace iTravelerServer.Service.Interfaces
{
    public interface IOrderService
    {
        Task<BaseResponse<IEnumerable<Order>>> GetOrders();
        Task<BaseResponse<Order>> AddOrders(TicketListVM ticket, Account account);
    }   
}