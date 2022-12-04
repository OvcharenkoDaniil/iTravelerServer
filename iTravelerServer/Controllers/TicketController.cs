using System.Security.Claims;
using iTravelerServer.DAL;
using iTravelerServer.Domain.Entities;
using iTravelerServer.Domain.Response;
using iTravelerServer.Domain.ViewModels.FlightVM;
using iTravelerServer.Service.Interfaces;
using iTravelerServer.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iTravelerServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TicketController : Controller
{
    private readonly IAccountService _accountService;
    private readonly IAirportService _airportService;
    private readonly IFlightService _flightService;
    private readonly IOrderService _orderService;
    private readonly IPlaneService _planeService;
    private readonly ITicketService _ticketService;
    private readonly ITransferService _transferService;
    private readonly ITicketListService _ticketListService;


    private readonly ApplicationDbContext _db;

    //private Guid Id => Guid.Parse(User.Claims.Single(i => i.Type == ClaimTypes.NameIdentifier).Value);
    public TicketController(
        ITicketService ticketService,
        ITicketListService ticketListService,
        ApplicationDbContext db, IFlightService flightService)
    {
        _db = db;
        _flightService = flightService;

        _ticketService = ticketService;

        _ticketListService = ticketListService;
    }


    //[Authorize (Roles = "User")]
    //[Authorize]
    // [HttpGet]
    // [Route("get-flights")]
    // public async Task<BaseResponse<IEnumerable<TicketListVM>>> GetTikets()
    // {
    //     var response = await _flightService.GetFlights();
    //    
    //     // if (response.StatusCode == Domain.Enum.StatusCode.OK)
    //     // {
    //         return response;
    //     // }
    //
    //     //return Redirect("Error");
    // }


    //[Authorize(Roles = "Admin")]
    [HttpGet]
    [Route("GetTikets")] //TicketSearchRequest filter
    public List<FlightListVM> GetTikets()
    {
        var response = _flightService.GetFlightList();
        if (response.StatusCode == Domain.Enum.StatusCode.OK)
        {
            return response.Data;
        }

        return null;
    }

    [Authorize]
    [HttpGet]
    [Route("GetTiketss")] //TicketSearchRequest filter
    public List<TicketListVM> GetTiketss()
    {
        var response = _ticketListService.GetFlightList();
        if (response.StatusCode == Domain.Enum.StatusCode.OK)
        {
            return response.Data;
        }

        return null;
    }

    //[Authorize]
    //[Route("GetFilteredTickets")] //TicketSearchRequest filter
    [HttpPost("GetFilteredTickets")]
    public List<TicketListVM> GetFilteredTickets(TicketSearchRequest filter)
    {
        var response = _flightService.GetFlightList();

        if (response.StatusCode == Domain.Enum.StatusCode.OK)
        {
            var listOfSortedTickets = _ticketService.GetTicketList(response.Data, filter);

            if (listOfSortedTickets.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return listOfSortedTickets.Data;
            }
        }
        return null;
    }
}