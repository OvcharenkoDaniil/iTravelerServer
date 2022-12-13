using iTravelerServer.DAL.Interfaces;
using iTravelerServer.Domain.Entities;
using iTravelerServer.Domain.Enum;
using iTravelerServer.Domain.Response;
using iTravelerServer.Domain.ViewModels.FlightVM;
using iTravelerServer.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace iTravelerServer.Service.Services;

public class TicketService : ITicketService
{
    private readonly IBaseRepository<Ticket> _ticketRepository;

    public TicketService(IBaseRepository<Ticket> ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<BaseResponse<IEnumerable<Ticket>>> GetTickets()
    {
        var baseResponse = new BaseResponse<IEnumerable<Ticket>>();
        try
        {
            var tickets = await _ticketRepository.GetAll().ToListAsync();

            baseResponse.Data = tickets;
            return baseResponse;
        }
        catch (Exception ex)
        {
            return new BaseResponse<IEnumerable<Ticket>>()
            {
                Description = $"[GetTickets] : {ex.Message}",
                StatusCode = StatusCode.InternalServerError
            };
        }
    }

    public BaseResponse<Ticket> AddTicket(Ticket ticket)
    {
        var baseResponse = new BaseResponse<Ticket>();
        try
        {
            
            _ticketRepository.Create(ticket);

            var result = _ticketRepository.Get(ticket);
            if (result!=null)
            {
                return new BaseResponse<Ticket>()
                {
                    Description = "Ticket created",
                    Data = result,
                    StatusCode = StatusCode.OK
                };
            }

            return new BaseResponse<Ticket>()
            {
                Description = "Ticket was not created",
                
                StatusCode = StatusCode.InternalServerError
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<Ticket>()
            {
                Description = $"[BookTheTicket] : {ex.Message}",
                StatusCode = StatusCode.InternalServerError
            };
        }
    }


    public BaseResponse<List<TicketListVM>> GetTicketList(List<FlightListVM> flightList, TicketSearchRequest filter)
    {
        var fwTicketList = new List<FlightListVM>();
        var bwTicketList = new List<FlightListVM>();
        var ticketList = new List<TicketListVM>();


        try
        {
            filter.DepartureDate = filter.DepartureDate.AddDays(1);
            filter.ReturnDate = filter.ReturnDate.AddDays(1);
            foreach (var item in flightList)
            {
                if (
                    item.FwArrivalCity == filter.ArrivalCity &&
                    item.FwDepartureCity == filter.DepartureCity &&
                    item.FwDepartureDate.Date == filter.DepartureDate.Date
                    //item.BwDepartureDate==filter.ReturnDate
                )
                {
                    if (filter.FlightClass == "FirstClass" &&
                        item.FwFirstClassCapacity >= filter.NumberOfPassangers)
                    {
                        fwTicketList.Add(item);
                    }

                    if (filter.FlightClass == "StandardClass" &&
                        item.FwStandardClassCapacity >= filter.NumberOfPassangers)
                    {
                        fwTicketList.Add(item);
                    }
                }

                if (
                    item.FwArrivalCity == filter.DepartureCity &&
                    item.FwDepartureCity == filter.ArrivalCity &&
                    item.FwDepartureDate.Date == filter.ReturnDate.Date
                    //item.BwDepartureDate==filter.ReturnDate
                )
                {
                    if (filter.FlightClass == "FirstClass" &&
                        item.FwFirstClassCapacity >= filter.NumberOfPassangers)
                    {
                        bwTicketList.Add(item);
                    }

                    if (filter.FlightClass == "StandardClass" &&
                        item.FwStandardClassCapacity >= filter.NumberOfPassangers)
                    {
                        bwTicketList.Add(item);
                    }
                }
            }

            if (fwTicketList.Count != 0 && bwTicketList.Count != 0)
            {
                int i = 0;
                //int i = 1;
                foreach (var fwTicket in fwTicketList)
                {
                    foreach (var bwTicket in bwTicketList)
                    {
                        ticketList.Add(createTicket(fwTicket,bwTicket,filter.NumberOfPassangers,i));
                        i++;
                    }
                }

                return new BaseResponse<List<TicketListVM>>()
                {
                    Data = ticketList,
                    StatusCode = StatusCode.OK
                };
            }

            return new BaseResponse<List<TicketListVM>>()
            {
                Description = "there is not such tickets",
                StatusCode = StatusCode.NotFound
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<List<TicketListVM>>()
            {
                Description = $"[GetTicketList] : {ex.Message}",
                StatusCode = StatusCode.InternalServerError
            };
        }
    }

    public TicketListVM createTicket(
    FlightListVM fwTicket,
    FlightListVM bwTicket,
    int NumberOfPassangers,
    int i
    )
    {
        // int i = 1;
        // foreach (var fwTicket in fwTicketList)
        // {
        //     foreach (var bwTicket in bwTicketList)
        //     {
                var ticket = new TicketListVM();
                ticket.TicketElem_id = i;
                ticket.FwFlight_id = fwTicket.Flight_id;
                ticket.BwFlight_id = bwTicket.Flight_id;
                ticket.NumberOfPassengers = NumberOfPassangers;

                ticket.TotalPrice = fwTicket.FwPrice + bwTicket.FwPrice;
                ticket.FwPrice = fwTicket.FwPrice;
                ticket.BwPrice = bwTicket.FwPrice;

                ticket.FwDepartureCity = fwTicket.FwDepartureCity;
                ticket.FwDepartureAirportName = fwTicket.FwDepartureAirportName;
                ticket.FwDepartureAirportCountry = fwTicket.FwDepartureAirportCountry;
                ticket.FwArrivalCity = fwTicket.FwArrivalCity;
                ticket.FwArrivalAirportName = fwTicket.FwArrivalAirportName;
                ticket.FwArrivalAirportCountry = fwTicket.FwArrivalAirportCountry;

                ticket.FwAircompany_name = fwTicket.FwAircompany_name;
                ticket.FwStandardClassCapacity = fwTicket.FwStandardClassCapacity;
                ticket.FwFirstClassCapacity = fwTicket.FwFirstClassCapacity;

                ticket.FwDepartureDate = fwTicket.FwDepartureDate;
                ticket.FwDepartureTime = fwTicket.FwDepartureTime;
                ticket.FwArrivalDate = fwTicket.FwArrivalDate;
                ticket.FwArrivalTime = fwTicket.FwArrivalTime;
                ticket.FwFlightDuration = fwTicket.FwFlightDuration;
                ticket.FwNumberOfTransfers = fwTicket.FwNumberOfTransfers;

                ticket.BwDepartureCity = bwTicket.FwDepartureCity;
                ticket.BwDepartureAirportName = bwTicket.FwDepartureAirportName;
                ticket.BwDepartureAirportCountry = bwTicket.FwDepartureAirportCountry;
                ticket.BwArrivalCity = bwTicket.FwArrivalCity;
                ticket.BwArrivalAirportName = bwTicket.FwArrivalAirportName;
                ticket.BwArrivalAirportCountry = bwTicket.FwArrivalAirportCountry;

                ticket.BwAircompany_name = bwTicket.FwAircompany_name;
                ticket.BwFirstClassCapacity = bwTicket.FwFirstClassCapacity;
                ticket.BwStandardClassCapacity = bwTicket.FwStandardClassCapacity;

                ticket.BwDepartureDate = bwTicket.FwDepartureDate;
                ticket.BwDepartureTime = bwTicket.FwDepartureTime;
                ticket.BwArrivalDate = bwTicket.FwArrivalDate;
                ticket.BwArrivalTime = bwTicket.FwArrivalTime;
                ticket.BwFlightDuration = bwTicket.FwFlightDuration;
                ticket.BwNumberOfTransfers = bwTicket.FwNumberOfTransfers;

                //ticketList.Add(ticket);
            //     i++;
            // }
        //}

        return ticket;
    }
}