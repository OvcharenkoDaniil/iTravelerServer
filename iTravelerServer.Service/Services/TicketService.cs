using iTravelerServer.DAL;
using iTravelerServer.DAL.Interfaces;
using iTravelerServer.Domain.Entities;
using iTravelerServer.Domain.Enum;
using iTravelerServer.Domain.Response;
using iTravelerServer.Domain.ViewModels.FlightVM;
using iTravelerServer.Domain.ViewModels.TicketVM;
using iTravelerServer.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace iTravelerServer.Service.Services;

public class TicketService : ITicketService
{
    private readonly IBaseRepository<Ticket> _ticketRepository;
    private readonly IBaseRepository<TicketDetail> _ticketDetailRepository;
    private readonly ApplicationDbContext _db;


    public TicketService(IBaseRepository<Ticket> ticketRepository, IBaseRepository<TicketDetail> ticketDetailRepository,
        ApplicationDbContext db)
    {
        _ticketRepository = ticketRepository;
        _ticketDetailRepository = ticketDetailRepository;
        _db = db;
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

    public BaseResponse<Ticket> AddTicket(TicketVM ticketVm)
    {
        var baseResponse = new BaseResponse<Ticket>();
        try
        {
            bool val = false;
            bool val2 = false;
            var freeFwPlaces = GetFreeFwPlaces(ticketVm.FwFlight_id, ticketVm.FlightClass);
            var freeBwPlaces = GetFreeBwPlaces(ticketVm.BwFlight_id, ticketVm.FlightClass);
            foreach (var item in freeFwPlaces)
            {
                if (
                    item == ticketVm.FwFirst_ticket_num
                )
                {
                    val = true;
                }
            }

            if (ticketVm.FwSecond_ticket_num != 0)
            {
                val = false;
                foreach (var item in freeFwPlaces)
                {
                    if (
                        item == ticketVm.FwSecond_ticket_num
                    )
                    {
                        val = true;
                    }
                }
            }

            if (ticketVm.FwThird_ticket_num != 0)
            {
                val = false;
                foreach (var item in freeFwPlaces)
                {
                    if (
                        item == ticketVm.FwThird_ticket_num
                    )
                    {
                        val = true;
                    }
                }
            }

            foreach (var item in freeBwPlaces)
            {
                if (
                    item == ticketVm.BwFirst_ticket_num
                )
                {
                    val2 = true;
                }
            }

            if (ticketVm.BwSecond_ticket_num != 0)
            {
                val2 = false;
                foreach (var item in freeBwPlaces)
                {
                    if (
                        item == ticketVm.BwSecond_ticket_num
                    )
                    {
                        val2 = true;
                    }
                }
            }

            if (ticketVm.BwThird_ticket_num != 0)
            {
                val2 = false;
                foreach (var item in freeBwPlaces)
                {
                    if (
                        item == ticketVm.BwThird_ticket_num
                    )
                    {
                        val2 = true;
                    }
                }
            }

            if (val && val2)
            {
                var ticket = new Ticket()
                {
                    Price = ticketVm.Price,
                    FwFlight_id = ticketVm.FwFlight_id,
                    BwFlight_id = ticketVm.BwFlight_id
                };

                _ticketRepository.Create(ticket);
                //var returnedTicket = _ticketRepository.Get(ticket);
                if (ticket.Ticket_id != 0)
                {
                    var fwTicketDetail = new TicketDetail()
                    {
                        First_ticket_num = ticketVm.FwFirst_ticket_num,
                        Second_ticket_num = ticketVm.FwSecond_ticket_num,
                        Third_ticket_num = ticketVm.FwThird_ticket_num,
                        Direction = "Forward",
                        Ticket_id = ticket.Ticket_id
                    };
                    var bwTicketDetail = new TicketDetail()
                    {
                        First_ticket_num = ticketVm.BwFirst_ticket_num,
                        Second_ticket_num = ticketVm.BwSecond_ticket_num,
                        Third_ticket_num = ticketVm.BwThird_ticket_num,
                        Direction = "Backward",
                        Ticket_id = ticket.Ticket_id
                    };

                    _ticketDetailRepository.Create(fwTicketDetail);
                    _ticketDetailRepository.Create(bwTicketDetail);

                    // var returnedFwTicketDetail = _ticketDetailRepository.Get(fwTicketDetail);
                    // var returnedBwTicketDetail = _ticketDetailRepository.Get(bwTicketDetail);

                    ticket.FwTicketDetail_id = fwTicketDetail.Ticket_Detail_id;
                    ticket.BwTicketDetail_id = bwTicketDetail.Ticket_Detail_id;
                    var res = _ticketRepository.UpdateSync(ticket);
                    //var a = _ticketRepository.Get(ticket);
                    return new BaseResponse<Ticket>()
                    {
                        Description = "Ticket created",
                        Data = ticket,
                        StatusCode = StatusCode.OK
                    };
                }
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

    public List<int> GetFreeFwPlaces(int fwFlightId, string FlightClass)
    {
        try
        {
            var listOfUsedValues = new List<int>();
            var listOfFreevalues = new List<int>();
            var fwPlane = (from flight in _db.Flight
                join plane in _db.Plane on flight.Plane_id equals plane.Plane_id
                select new
                {
                    flight.Flight_id,
                    plane.FirstClassCapacity,
                    plane.StandardClassCapacity
                }).FirstOrDefault(x => x.Flight_id == fwFlightId);

            var fwticketDetail = (from tickets in _db.Ticket
                join ticketDetail in _db.TicketDetail on tickets.FwTicketDetail_id equals ticketDetail.Ticket_Detail_id
                select new
                {
                    tickets.FwFlight_id,
                    ticketDetail.First_ticket_num,
                    ticketDetail.Second_ticket_num,
                    ticketDetail.Third_ticket_num
                }).Where(x => x.FwFlight_id == fwFlightId).ToList();
            foreach (var item in fwticketDetail)
            {
                if (item.First_ticket_num != 0)
                {
                    listOfUsedValues.Add(item.First_ticket_num);
                }

                if (item.Second_ticket_num != 0)
                {
                    listOfUsedValues.Add(item.Second_ticket_num);
                }

                if (item.Third_ticket_num != 0)
                {
                    listOfUsedValues.Add(item.Third_ticket_num);
                }
            }

            if (FlightClass == "FirstClass")
            {
                for (int i = 1; i < fwPlane.FirstClassCapacity; i++)
                {
                    listOfFreevalues.Add(i);
                }

                foreach (var item in listOfUsedValues)
                {
                    for (int i = 1; i < fwPlane.FirstClassCapacity; i++)
                    {
                        if (item == i)
                        {
                            listOfFreevalues.Remove(item);
                        }
                    }
                }
            }

            if (FlightClass == "StandardClass")
            {
                for (int i = 1; i < fwPlane.StandardClassCapacity; i++)
                {
                    listOfFreevalues.Add(i);
                }

                foreach (var item in listOfUsedValues)
                {
                    for (int i = 1; i < fwPlane.StandardClassCapacity; i++)
                    {
                        if (item == i)
                        {
                            listOfFreevalues.Remove(item);
                        }
                    }
                }
            }

            return listOfFreevalues;
        }
        catch (Exception ex)
        {
            return new List<int>();
        }
    }

    public List<int> GetFreeBwPlaces(int bwFlightId, string FlightClass)
    {
        try
        {
            var listOfUsedValues = new List<int>();
            var listOfFreevalues = new List<int>();
            var bwPlane = (from flight in _db.Flight
                join plane in _db.Plane on flight.Plane_id equals plane.Plane_id
                select new
                {
                    flight.Flight_id,
                    plane.FirstClassCapacity,
                    plane.StandardClassCapacity
                }).FirstOrDefault(x => x.Flight_id == bwFlightId);
            var bwticketDetail = (from tickets in _db.Ticket
                join ticketDetail in _db.TicketDetail on tickets.BwTicketDetail_id equals ticketDetail.Ticket_Detail_id
                select new
                {
                    tickets.BwFlight_id,
                    ticketDetail.First_ticket_num,
                    ticketDetail.Second_ticket_num,
                    ticketDetail.Third_ticket_num
                }).Where(x => x.BwFlight_id == bwFlightId).ToList();
            foreach (var item in bwticketDetail)
            {
                if (item.First_ticket_num != 0)
                {
                    listOfUsedValues.Add(item.First_ticket_num);
                }

                if (item.Second_ticket_num != 0)
                {
                    listOfUsedValues.Add(item.Second_ticket_num);
                }

                if (item.Third_ticket_num != 0)
                {
                    listOfUsedValues.Add(item.Third_ticket_num);
                }
            }

            if (FlightClass == "FirstClass")
            {
                for (int i = 1; i < bwPlane.FirstClassCapacity; i++)
                {
                    listOfFreevalues.Add(i);
                }

                foreach (var item in listOfUsedValues)
                {
                    for (int i = 1; i < bwPlane.FirstClassCapacity; i++)
                    {
                        if (item == i)
                        {
                            listOfFreevalues.Remove(item);
                        }
                    }
                }
            }

            if (FlightClass == "StandardClass")
            {
                for (int i = 1; i < bwPlane.StandardClassCapacity; i++)
                {
                    listOfFreevalues.Add(i);
                }

                foreach (var item in listOfUsedValues)
                {
                    for (int i = 1; i < bwPlane.StandardClassCapacity; i++)
                    {
                        if (item == i)
                        {
                            listOfFreevalues.Remove(item);
                        }
                    }
                }
            }

            return listOfFreevalues;
        }
        catch (Exception ex)
        {
            return new List<int>();
        }
    }

    public BaseResponse<List<TicketListVM>> GetTicketList(List<FlightListVM> flightList, TicketSearchRequest filter)
    {


        try
        {
        var fwTicketList = new List<FlightListVM>();
        var bwTicketList = new List<FlightListVM>();
        var ticketList = new List<TicketListVM>();
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
                        item.FwPrice *= filter.NumberOfPassangers * 2;
                        fwTicketList.Add(item);
                    }

                    if (filter.FlightClass == "StandardClass" &&
                        item.FwStandardClassCapacity >= filter.NumberOfPassangers)
                    {
                        item.FwPrice *= filter.NumberOfPassangers;
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
                        item.FwPrice *= filter.NumberOfPassangers * 2;
                        bwTicketList.Add(item);
                    }

                    if (filter.FlightClass == "StandardClass" &&
                        item.FwStandardClassCapacity >= filter.NumberOfPassangers)
                    {
                        item.FwPrice *= filter.NumberOfPassangers;
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
                        ticketList.Add(createTicket(fwTicket, bwTicket, filter.NumberOfPassangers, i));
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
        try{
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
        catch (Exception ex)
        {
            return new TicketListVM();
        }
    }
}