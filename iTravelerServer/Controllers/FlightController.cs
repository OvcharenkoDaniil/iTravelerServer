using System.Collections;
using iTravelerServer.Domain.Entities;
using iTravelerServer.Domain.ViewModels.FlightVM;
using iTravelerServer.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iTravelerServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FlightController : Controller
{
    private readonly IAccountService _accountService;
    private readonly IConfiguration _configuration;
    private readonly IFlightService _flightService;


    public FlightController(IAccountService accountService, IConfiguration configuration, IFlightService flightService)
    {
        _accountService = accountService;
        _configuration = configuration;
        _flightService = flightService;
    }


    //[Authorize]
    [HttpPost("GetFilteredTickets")]
    public List<FlightListVM> GetFilteredTickets(TicketSearchRequest filter)
    {
        var response = _flightService.GetFlightList();
        if (response.StatusCode == Domain.Enum.StatusCode.OK)
        {
            return response.Data;
        }
        return null;
    }
    
    
    
    [Authorize(Roles = "Admin")]
    [HttpGet("GetFlights")]
    public async Task<IEnumerable<Flight>> GetFlights()
    {
        var response = await  _flightService.GetFlights();
        if (response.StatusCode == Domain.Enum.StatusCode.OK)
        {
            return response.Data;
        }
        return null;
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPost("AddFlight")]
    public async Task<bool> AddFlight(Flight flightData)
    {
        var response = await _flightService.AddFlight(flightData);
        if (response.StatusCode == Domain.Enum.StatusCode.OK)
        {
            return true;
        }
        return false;
    }
}