using iTravelerServer.Domain.Entities;
using iTravelerServer.Domain.Response;
using iTravelerServer.Domain.ViewModels.AccountVM;
using iTravelerServer.Domain.ViewModels.FlightVM;
using iTravelerServer.Domain.ViewModels.OrderVM;
using iTravelerServer.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iTravelerServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : Controller
{
    private readonly IOrderService _orderService;
    private readonly IFlightService _flightService;
    private readonly IMailService _mailService;
    

    //[Authorize(Roles = "User")]
    public OrderController(IOrderService orderService, IFlightService flightService,IMailService mailService)
    {
        _mailService = mailService;
        _orderService = orderService;
        _flightService = flightService;
        
    }

    
    
    [HttpPost]
    public void SendMessage(string email)
    {
        _mailService.MessageSender(email,"");
    }
    
    
    [Authorize(Roles = "User")]
    [Route("AddOrder")]
    [HttpPost]
    public async Task<int> AddOrder(OrderDetailsVM orderDetailsVM)
    {
        if (_orderService.IsAvailable(orderDetailsVM))
        {
            var listOfFreeFwPlaces = _flightService.GetFreePlaces(orderDetailsVM.FwFlight_id,
                orderDetailsVM.FlightClass,
                _orderService.GetFwOrderDetail(orderDetailsVM.FwFlight_id, orderDetailsVM.FlightClass));
            var orderdetail = new OrderDetails()
            {
                SeatNumbers = orderDetailsVM.FwSeatNumbers,
                Direction = "Forward",
                Order_id = 0,
                NumberOfBaggagePlaces = orderDetailsVM.FwNumberOfBaggagePlaces,
                Price = orderDetailsVM.FwPrice,
                TotalExtraBaggageWeight = orderDetailsVM.FwTotalExtraBaggageWeight,
                FlightClass = orderDetailsVM.FlightClass
            };
            _orderService.CheckBaggageExtraPayment(orderdetail, orderDetailsVM.NumberOfTickets,
                orderDetailsVM.FwFlight_id, orderDetailsVM.FwTotalExtraBaggageWeight);
            var fwOrderDetails = _orderService.AddOrderDetails(orderdetail, listOfFreeFwPlaces);
            var bwOrderDetails = new BaseResponse<OrderDetails>();
            bwOrderDetails = fwOrderDetails;

            if (!orderDetailsVM.IsOneSide)
            {
                var listOfFreeBwPlaces = _flightService.GetFreePlaces(orderDetailsVM.BwFlight_id,
                    orderDetailsVM.FlightClass,
                    _orderService.GetBwOrderDetail(orderDetailsVM.BwFlight_id, orderDetailsVM.FlightClass));
                orderdetail = new OrderDetails()
                {
                    SeatNumbers = orderDetailsVM.BwSeatNumbers,
                    Direction = "Backward",
                    Order_id = 0,
                    NumberOfBaggagePlaces = orderDetailsVM.BwNumberOfBaggagePlaces,
                    Price = orderDetailsVM.BwPrice,
                    TotalExtraBaggageWeight = orderDetailsVM.BwTotalExtraBaggageWeight,
                    FlightClass = orderDetailsVM.FlightClass
                };
                _orderService.CheckBaggageExtraPayment(orderdetail, orderDetailsVM.NumberOfTickets,
                    orderDetailsVM.BwFlight_id, orderDetailsVM.BwTotalExtraBaggageWeight);
                bwOrderDetails = _orderService.AddOrderDetails(orderdetail, listOfFreeBwPlaces);
            }

            var response = await _orderService.AddOrder(fwOrderDetails.Data, bwOrderDetails.Data, orderDetailsVM);


            if (
                response.StatusCode == Domain.Enum.StatusCode.OK
            )
            {
                
                return response.Data.Order_id;
            }
        }

        return 0;
    }
    

    
    [Route("DeleteOrder")]
    [HttpPost]
    public async Task<bool> DeleteOrder(OrderDataVM order)
    {
        var response = await _orderService.DeleteOrder(order.orderId);
        if (response.StatusCode == Domain.Enum.StatusCode.OK)
        {
            
            return true;
        }
    
        return false;
    }

    [Authorize]
    [Route("GetOrders")]
    [HttpPost]
    public List<TicketListVM> GetOrders(AccountVM accountVm)
    {
        //TicketSearchRequest filter = new TicketSearchRequest();

        var response = _flightService.GetFlightList();
        if (response.StatusCode == Domain.Enum.StatusCode.OK)
        {
            var listOfOrders = _orderService.GetOrdersList(response.Data, accountVm.email);
            if (listOfOrders.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return listOfOrders.Data;
            }
        }
        return null;
    }


    
    [Route("GetAllOrders")]
    [HttpGet]
    public async Task<List<Order>> GetAllOrders()
    {
        var response = await _orderService.GetAllOrders();
        if (response.StatusCode == Domain.Enum.StatusCode.OK)
        {
            return response.Data;
        }
        return null;
    }
}