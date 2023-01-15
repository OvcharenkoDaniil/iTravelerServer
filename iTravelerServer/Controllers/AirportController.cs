using iTravelerServer.Domain.Entities;
using iTravelerServer.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iTravelerServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AirportController : Controller
{
    private readonly IAccountService _accountService;
    private readonly IConfiguration _configuration;
    private readonly IAirportService _airportService;

    public AirportController(IAccountService accountService, IConfiguration configuration, IFlightService flightService,
        IPlaneService planeService, IAirportService airportService)
    {
        //_authOptions = authOptions;
        _accountService = accountService;
        _configuration = configuration;
        _airportService = airportService;
        
    }

    [Authorize(Roles = "Admin")]
    [Route("GetAirports")]
    [HttpGet]
    public async Task<IEnumerable<Airport>> GetAirports()
    {
        //TicketSearchRequest filter = new TicketSearchRequest();

        var response = await _airportService.GetAirports();
        if (response.StatusCode == Domain.Enum.StatusCode.OK)
        {
            return response.Data;
        }

        return null;
    }
}