using System.Collections.Generic;
using System.Threading.Tasks;
using iTravelerServer.Domain.Entities;
using iTravelerServer.Domain.Response;
using iTravelerServer.Domain.ViewModels.FlightVM;
using iTravelerServer.Domain.ViewModels.OrderVM;


namespace iTravelerServer.Service.Interfaces
{
    public interface IOrderService
    {
        Task<BaseResponse<IEnumerable<Order>>> GetOrders();
        Task<BaseResponse<Order>> AddOrder(OrderVM orderVM);

        BaseResponse<List<TicketListVM>> GetOrdersList(List<FlightListVM> flightList, string email,
            ITicketService _ticketService);

        Task<BaseResponse<Order>> DeleteOrder(int orderId);
    }   
}