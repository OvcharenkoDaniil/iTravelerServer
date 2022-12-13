using iTravelerServer.Domain.Entities;
using iTravelerServer.Domain.ViewModels.AccountVM;
using iTravelerServer.Domain.ViewModels.FlightVM;
using iTravelerServer.Domain.ViewModels.OrderVM;
using iTravelerServer.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iTravelerServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController:Controller
{
    private readonly IOrderService _orderService;
    private readonly IFlightService _flightService;
    private readonly ITicketService _ticketService;

    //[Authorize(Roles = "User")]
    public OrderController(IOrderService orderService , IFlightService flightService, ITicketService ticketService)
    {
        _orderService = orderService;
        _flightService = flightService;
        _ticketService = ticketService;
    }

    [Authorize(Roles = "User")]
    [Route("AddOrder")] 
    [HttpPost]
    public async Task<bool> AddOrder(OrderVM order)
    {
        //TicketListVM ticket = new TicketListVM();
        var response = await _orderService.AddOrder(order);
         if (response.StatusCode == Domain.Enum.StatusCode.OK)
         {
             return true;
         }
        return false;
    }
    
    [Authorize(Roles = "User")]
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
    
    [Authorize(Roles = "User")]
    [Route("GetOrders")] 
    [HttpPost]
    public List<TicketListVM> GetOrders(AccountVM accountVm)
    {
        //TicketSearchRequest filter = new TicketSearchRequest();

        var response = _flightService.GetFlightList();
        if (response.StatusCode == Domain.Enum.StatusCode.OK)
        {
            
            var listOfSortedTickets = _orderService.GetOrdersList(response.Data, accountVm.email,_ticketService);

            if (listOfSortedTickets.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return listOfSortedTickets.Data;
            }
        }

        return null;
    }
    
}