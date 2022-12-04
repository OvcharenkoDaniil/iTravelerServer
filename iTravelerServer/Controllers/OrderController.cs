using iTravelerServer.Domain.Entities;
using iTravelerServer.Domain.ViewModels.FlightVM;
using iTravelerServer.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iTravelerServer.Controllers;

public class OrderController:Controller
{
    private readonly IOrderService _orderService;
    
    //[Authorize(Roles = "User")]
    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [Route("AddOrder")] 
    [HttpGet]
    public async Task<IActionResult> AddOrder(TicketListVM ticket, Account account)
    {
        var response = await _orderService.AddOrders(ticket,account);
        if (response.StatusCode == Domain.Enum.StatusCode.OK)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
    // [Route("DeleteOrder")] 
    // public async Task<IActionResult> DeleteOrder(TicketListVM ticket, Account account)
    // {
    //     var response = await _orderService.AddOrders(ticket,account);
    //     if (response.StatusCode == Domain.Enum.StatusCode.OK)
    //     {
    //         return Ok(response);
    //     }
    //     return BadRequest(response);
    // }
}