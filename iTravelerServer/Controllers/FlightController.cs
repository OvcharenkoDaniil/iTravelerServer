using System.Collections;
using iTravelerServer.DAL;
using iTravelerServer.DAL.Interfaces;
using iTravelerServer.DAL.Repositories;
using iTravelerServer.Domain.Entities;
using iTravelerServer.Domain.Response;
using iTravelerServer.Domain.ViewModels.FlightVM;
using iTravelerServer.Domain.ViewModels.OrderVM;
using iTravelerServer.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iTravelerServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FlightController : Controller
{
    private readonly IAccountService _accountService;
    private readonly IOrderService _orderService;
    private readonly IConfiguration _configuration;
    private readonly IFlightService _flightService;
    private readonly ApplicationDbContext _db;
    private readonly IBaseRepository<FlightDetails> _flightDetailsRepository;

    public FlightController(IAccountService accountService,IOrderService orderService, IConfiguration configuration,
        IFlightService flightService, ApplicationDbContext db, IBaseRepository<FlightDetails> flightDetailsRepository)
    {
        _accountService = accountService;
        _orderService = orderService;
        _configuration = configuration;
        _flightService = flightService;
        _flightDetailsRepository = flightDetailsRepository;
        _db = db;
    }
    
    [HttpGet]
    public List<Flight> FillData()
    {
        return _flightService.FlightGenerator(2,2);
    }
    
    // [HttpPost("AddOrder")]
    // [Authorize]
    // public async Task<int> AddOrder(OrderDetailsVM orderDetailsVM)
    // {
    //     var listOfFreeFwPlaces = _flightService.GetFreePlaces(orderDetailsVM.FwFlight_id, orderDetailsVM.FlightClass,
    //         _flightService.GetFwOrderDetail(orderDetailsVM.FwFlight_id));
    //     var orderdetail = new OrderDetails()
    //     {
    //         SeatNumbers = orderDetailsVM.FwSeatNumbers,
    //         Direction = "Forward",
    //         Order_id = 0,
    //         NumberOfBaggagePlaces = orderDetailsVM.FwNumberOfBaggagePlaces,
    //         Price = orderDetailsVM.FwPrice,
    //         FlightClass = orderDetailsVM.FlightClass
    //     };
    //     _flightService.CheckBaggageExtraPayment(orderdetail, orderDetailsVM.NumberOfTickets,
    //         orderDetailsVM.FwFlight_id, orderDetailsVM.FwTotalExtraBaggageWeight);
    //     var fwOrderDetails = _flightService.AddOrderDetails(orderdetail, listOfFreeFwPlaces);
    //     var bwOrderDetails = new BaseResponse<OrderDetails>();
    //     bwOrderDetails = fwOrderDetails;
    //
    //     if (!orderDetailsVM.IsOneSide)
    //     {
    //         var listOfFreeBwPlaces = _flightService.GetFreePlaces(orderDetailsVM.BwFlight_id,
    //             orderDetailsVM.FlightClass,
    //             _flightService.GetBwOrderDetail(orderDetailsVM.BwFlight_id));
    //         orderdetail = new OrderDetails()
    //         {
    //             SeatNumbers = orderDetailsVM.BwSeatNumbers,
    //             Direction = "Backward",
    //             Order_id = 0,
    //             NumberOfBaggagePlaces = orderDetailsVM.BwNumberOfBaggagePlaces,
    //             Price = orderDetailsVM.BwPrice,
    //             FlightClass = orderDetailsVM.FlightClass
    //         };
    //         _flightService.CheckBaggageExtraPayment(orderdetail, orderDetailsVM.NumberOfTickets,
    //             orderDetailsVM.BwFlight_id, orderDetailsVM.BwTotalExtraBaggageWeight);
    //         bwOrderDetails = _flightService.AddOrderDetails(orderdetail, listOfFreeBwPlaces);
    //     }
    //
    //     var response = await _flightService.AddOrder(fwOrderDetails.Data, bwOrderDetails.Data, orderDetailsVM);
    //
    //
    //     if (
    //         response.StatusCode == Domain.Enum.StatusCode.OK
    //     )
    //     {
    //         return response.Data.Order_id;
    //     }
    //
    //     return 0;
    // }


    [HttpPost("FreePlaces")]
    public FreePlaces FreePlaces(TicketFreePlacesVM ticketVm)
    {
        FreePlaces dataVm = new FreePlaces();
        var fwPlaces = _flightService.GetFreePlaces(ticketVm.FwFlight_id, ticketVm.FlightClass,
            _orderService.GetFwOrderDetail(ticketVm.FwFlight_id,ticketVm.FlightClass));
        var bwPlaces = _flightService.GetFreePlaces(ticketVm.BwFlight_id, ticketVm.FlightClass,
            _orderService.GetBwOrderDetail(ticketVm.BwFlight_id,ticketVm.FlightClass));

        dataVm.fwPlaces = _flightService.FreePlacesToString(fwPlaces);
        dataVm.bwPlaces = _flightService.FreePlacesToString(bwPlaces);
        return dataVm;
    }


    //[Authorize]
    [HttpPost("GetFilteredTickets")]
    public List<TicketListVM> GetFilteredTickets(SearchRequest filter)
    {
        //TicketSearchRequest filter = new TicketSearchRequest();
       
            var response = _flightService.GetFlightList();
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                BaseResponse<List<TicketListVM>> listOfSortedTickets;
                if (filter.IsOneSide)
                {
                    listOfSortedTickets = _flightService.GetOneSideTicketList(response.Data, filter);
                }
                else
                    listOfSortedTickets = _flightService.GetTicketList(response.Data, filter);

                if (listOfSortedTickets.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    return listOfSortedTickets.Data;
                }
            }
        

        return null;
    }
    // [HttpPost("GetOneSideFilteredTickets")]
    // public List<TicketListVM> GetOneSideFilteredTickets(SearchRequest filter)
    // {
    //     //TicketSearchRequest filter = new TicketSearchRequest();
    //
    //     var response = _flightService.GetFlightList();
    //     if (response.StatusCode == Domain.Enum.StatusCode.OK)
    //     {
    //         var listOfSortedTickets = _flightService.GetOneSideTicketList(response.Data, filter);
    //
    //         if (listOfSortedTickets.StatusCode == Domain.Enum.StatusCode.OK)
    //         {
    //             return listOfSortedTickets.Data;
    //         }
    //     }
    //
    //     return null;
    // }


    [Authorize(Roles = "Admin")]
    [HttpGet("GetFlights")]
    public async Task<IEnumerable<Flight>> GetFlights()
    {
        var response = await _flightService.GetFlights();
        if (response.StatusCode == Domain.Enum.StatusCode.OK)
        {
            return response.Data;
        }

        return null;
    }

    // [Authorize(Roles = "Admin")]
    // [HttpPost("AddFlight")]
    // public async Task<bool> AddFlight(Flight flightData)
    // {
    //     var response = await _flightService.AddFlight(flightData);
    //     if (response.StatusCode == Domain.Enum.StatusCode.OK)
    //     {
    //         return true;
    //     }
    //
    //     return false;
    // }
}