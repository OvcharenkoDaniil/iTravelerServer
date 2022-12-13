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


    private readonly ApplicationDbContext _db;

    public TicketController(
        ITicketService ticketService,
        ITransferService transferService,
        ApplicationDbContext db, IFlightService flightService)
    {
        _db = db;
        _flightService = flightService;
        _transferService = transferService;

        _ticketService = ticketService;
    }

    
    
    [Authorize]
    [HttpPost("GetFilteredTickets")]
    public List<TicketListVM> GetFilteredTickets(TicketSearchRequest filter)
    {
        //TicketSearchRequest filter = new TicketSearchRequest();

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


    [HttpPost("UpdateTiket")]
    [Authorize(Roles = "Admin")]
    public bool UpdateTiket(TicketListVM ticket)
    {
        var fwFlight = new Flight()
        {
            Flight_id = ticket.FwFlight_id,
            DepartureDate = ticket.FwDepartureDate,
            ArrivalDate = ticket.FwArrivalDate,
            DepartureTime = ticket.FwDepartureTime,
            ArrivalTime = ticket.FwArrivalTime,
            FlightDuration = ticket.FwFlightDuration,
            Price = ticket.FwPrice
        };

        var bwFlight = new Flight()
        {
            // Flight_id = ticket.FwFlight_id,
            // DepartureDate = ticket.FwDepartureDate,
            // ArrivalDate = ticket.FwArrivalDate,
            // DepartureTime = ticket.FwDepartureTime,
            // ArrivalTime = ticket.FwArrivalTime,
            // Price = ticket.FwPrice
            Flight_id = ticket.BwFlight_id,
            FlightDuration = ticket.BwFlightDuration,
            DepartureDate = ticket.BwDepartureDate,
            ArrivalDate = ticket.BwArrivalDate,
            DepartureTime = ticket.BwDepartureTime,
            ArrivalTime = ticket.BwArrivalTime,
            Price = ticket.BwPrice
        };
        var fwFlightObj = _flightService.GetFlight(ticket.FwFlight_id);
        var bwFlightObj = _flightService.GetFlight(ticket.BwFlight_id);
        var fwTransfer = _transferService.GetTransfer(fwFlightObj.Data.Transfer_id);
        var bwTransfer = _transferService.GetTransfer(bwFlightObj.Data.Transfer_id);
        var fwFlightResponse = _flightService.UpdateFlight(ticket.FwFlight_id, fwFlight);
        var bwFlightResponse = _flightService.UpdateFlight(ticket.BwFlight_id, bwFlight);
        var fwTransferResponse = _transferService.UpdateTransfer(fwTransfer.Data.Transfer_id,fwTransfer.Data);
        var bwTransferResponse = _transferService.UpdateTransfer(bwTransfer.Data.Transfer_id,bwTransfer.Data);
        
        if (fwFlightResponse.StatusCode == Domain.Enum.StatusCode.OK &&
            bwFlightResponse.StatusCode == Domain.Enum.StatusCode.OK &&
            fwTransferResponse.StatusCode == Domain.Enum.StatusCode.OK &&
            bwTransferResponse.StatusCode == Domain.Enum.StatusCode.OK 
            )
        {
            return true;
        }

        return false;
    }
    
    
    [HttpPost("AddTiket")]
    [Authorize]
    public int AddTiket(TicketListVM ticketVM)
    {
        var ticket = new Ticket()
        {
            Price = ticketVM.TotalPrice,
            FwFlight_id = ticketVM.FwFlight_id,
            BwFlight_id = ticketVM.BwFlight_id
        };

        var response = _ticketService.AddTicket(ticket);
        
        
        if (
            response.StatusCode == Domain.Enum.StatusCode.OK 
            )
        {
            return response.Data.Ticket_id;
        }

        return 0;
    }
}