using iTravelerServer.Domain.Entities;
using iTravelerServer.Domain.Response;
using iTravelerServer.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iTravelerServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlaneController : Controller
{
    private readonly IAccountService _accountService;
    private readonly IConfiguration _configuration;
    private readonly IPlaneService _planeService;

    public PlaneController(IAccountService accountService, IConfiguration configuration, IFlightService flightService,
        IPlaneService planeService)
    {
        _accountService = accountService;
        _configuration = configuration;
        _planeService = planeService;
    }

    [Authorize(Roles = "Admin")]
    [Route("GetPlanes")]
    [HttpGet]
    public async Task<IEnumerable<Plane>> GetPlanes()
    {
        var response = await _planeService.GetPlanes();
        if (response.StatusCode == Domain.Enum.StatusCode.OK)
        {
            return response.Data;
        }

        return null;
    }
}