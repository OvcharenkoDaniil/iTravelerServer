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
        Task<BaseResponse<List<Order>>> GetAllOrders();
        List<int> GetFwOrderDetail(int FlightId, string flightClass);
        List<int> GetBwOrderDetail(int FlightId,string flightClass);
        BaseResponse<OrderDetails> AddOrderDetails(OrderDetails orderDetail, List<int> listOfFreePlaces);

        Task<BaseResponse<Order>> AddOrder(OrderDetails fwOrderDetails, OrderDetails bwOrderDetails,
            OrderDetailsVM orderDetailsVM);
        void CheckBaggageExtraPayment(OrderDetails orderDetail, int NumberOfTickets, int flight_id,
            int extraBaggageWeight);

        BaseResponse<List<TicketListVM>> GetOrdersList(List<FlightListVM> flightList, string email);

        bool IsAvailable(OrderDetailsVM orderDetailsVM);
        //
        Task<BaseResponse<Order>> DeleteOrder(int orderId);
        // BaseResponse<List<TicketListVM>> ChangePrices(BaseResponse<List<TicketListVM>> listOfOrders,string email);
    }   
}