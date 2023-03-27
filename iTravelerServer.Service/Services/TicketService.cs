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
            //var freeFwPlaces = GetFreeFwPlaces(ticketVm.FwFlight_id, ticketVm.FlightClass);
            var listOfFreeFwPlaces = GetFreePlaces(ticketVm.FwFlight_id, ticketVm.FlightClass,
                GetFwTicketDetail(ticketVm.FwFlight_id));
            var listOfFreeBwPlaces = GetFreePlaces(ticketVm.BwFlight_id, ticketVm.FlightClass,
                GetBwTicketDetail(ticketVm.BwFlight_id));
            //var freeBwPlaces = GetFreeBwPlaces(ticketVm.BwFlight_id, ticketVm.FlightClass);

            if (IsPlaceAvailable(listOfFreeFwPlaces, ticketVm.FwFirst_ticket_num) &&
                IsPlaceAvailable(listOfFreeFwPlaces, ticketVm.FwSecond_ticket_num) &&
                IsPlaceAvailable(listOfFreeFwPlaces, ticketVm.FwThird_ticket_num) &&
                IsPlaceAvailable(listOfFreeBwPlaces, ticketVm.BwFirst_ticket_num) &&
                IsPlaceAvailable(listOfFreeBwPlaces, ticketVm.BwSecond_ticket_num) &&
                IsPlaceAvailable(listOfFreeBwPlaces, ticketVm.BwThird_ticket_num))
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

    private bool IsPlaceAvailable(List<int> listOfFreePlaces, int place)
    {
        if (place != 0)
        {
            foreach (var item in listOfFreePlaces)
            {
                if (item == place)
                {
                    return true;
                }
            }

            return false;
        }

        return true;
    }
    

    public List<int> GetFreePlaces(int FlightId, string FlightClass, List<TicketDetail> ticketDetail)
    {
        try
        {
            var listOfUsedValues = new List<int>();
            var listOfFreevalues = new List<int>();
            var plane = (from flight in _db.Flight
                join planes in _db.Plane on flight.Plane_id equals planes.Plane_id
                select new
                {
                    flight.Flight_id,
                    planes.FirstClassCapacity,
                    planes.StandardClassCapacity
                }).FirstOrDefault(x => x.Flight_id == FlightId);

            foreach (var item in ticketDetail)
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
                listOfFreevalues = getListOfFreeValues(listOfFreevalues, listOfUsedValues, plane.FirstClassCapacity);
            }

            if (FlightClass == "StandardClass")
            {
                listOfFreevalues = getListOfFreeValues(listOfFreevalues, listOfUsedValues, plane.StandardClassCapacity);
            }

            return listOfFreevalues;
        }
        catch (Exception ex)
        {
            return new List<int>();
        }
    }

    private List<int> getListOfFreeValues(List<int> listOfFreevalues, List<int> listOfUsedValues, int capacity)
    {
        for (int i = 1; i <= capacity; i++)
        {
            listOfFreevalues.Add(i);
        }

        foreach (var item in listOfUsedValues)
        {
            for (int i = 1; i <= capacity; i++)
            {
                if (item == i)
                {
                    listOfFreevalues.Remove(item);
                }
            }
        }

        return listOfFreevalues;
    }

    public List<TicketDetail> GetBwTicketDetail(int FlightId)
    {
        var bwticketDetail = (from tickets in _db.Ticket
            join ticketDetails in _db.TicketDetail on tickets.BwTicketDetail_id equals ticketDetails.Ticket_Detail_id
            select new
            {
                tickets.BwFlight_id,
                ticketDetails.First_ticket_num,
                ticketDetails.Second_ticket_num,
                ticketDetails.Third_ticket_num
            }).Where(x => x.BwFlight_id == FlightId).ToList();
        var ticketDetail = new TicketDetail();
        List<TicketDetail> ticketDetailList = new List<TicketDetail>();
        foreach (var item in bwticketDetail)
        {
            ticketDetail.First_ticket_num = item.First_ticket_num;
            ticketDetail.Second_ticket_num = item.Second_ticket_num;
            ticketDetail.Third_ticket_num = item.Third_ticket_num;
            ticketDetailList.Add(ticketDetail);
        }

        return ticketDetailList;
    }

    public List<TicketDetail> GetFwTicketDetail(int FlightId)
    {
        var fwticketDetail = (from tickets in _db.Ticket
            join ticketDetails in _db.TicketDetail on tickets.FwTicketDetail_id equals ticketDetails.Ticket_Detail_id
            select new
            {
                tickets.FwFlight_id,
                ticketDetails.First_ticket_num,
                ticketDetails.Second_ticket_num,
                ticketDetails.Third_ticket_num
            }).Where(x => x.FwFlight_id == FlightId).ToList();
        var ticketDetail = new TicketDetail();
        List<TicketDetail> ticketDetailList = new List<TicketDetail>();
        foreach (var item in fwticketDetail)
        {
            ticketDetail.First_ticket_num = item.First_ticket_num;
            ticketDetail.Second_ticket_num = item.Second_ticket_num;
            ticketDetail.Third_ticket_num = item.Third_ticket_num;
            ticketDetailList.Add(ticketDetail);
        }

        return ticketDetailList;
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
                if (isAvailable(item, filter.DepartureDate, filter.ArrivalCity,
                        filter.DepartureCity, filter.NumberOfPassangers, filter.FlightClass))
                {
                    fwTicketList.Add(item);
                }

                if (isAvailable(item, filter.ReturnDate, filter.DepartureCity,
                        filter.ArrivalCity, filter.NumberOfPassangers, filter.FlightClass))
                {
                    bwTicketList.Add(item);
                }
            }

            if (fwTicketList.Count != 0 && bwTicketList.Count != 0)
            {
                int i = 0;
                
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

    private bool isAvailable(FlightListVM item, DateTime DepartureDate, string ArrivalCity,
        string DepartureCity, int NumberOfPassangers, string FlightClass)
    {
        if (
            item.ArrivalCity == ArrivalCity &&
            item.DepartureCity == DepartureCity
        )
        {
            if (item.DepartureDate.Date == DepartureDate.Date)
            {
                if (FlightClass == "FirstClass" &&
                    item.FirstClassCapacity >= NumberOfPassangers)
                {
                    item.Price = NumberOfPassangers * item.FirstClassPrice;
                    return true;
                }

                if (FlightClass == "StandardClass" &&
                    item.StandardClassCapacity >= NumberOfPassangers)
                {
                    item.Price = NumberOfPassangers * item.StandardClassPrice;
                    return true;
                }
            }
        }

        return false;
    }


    public TicketListVM createTicket(
        FlightListVM fwTicket,
        FlightListVM bwTicket,
        int NumberOfPassangers,
        int i
    )

    {
        
        try
        {
            var ticket = new TicketListVM();
            ticket.TicketElem_id = i;
            ticket.FwFlight_id = fwTicket.Flight_id;
            ticket.BwFlight_id = bwTicket.Flight_id;
            ticket.NumberOfPassengers = NumberOfPassangers;

            ticket.TotalPrice = fwTicket.Price + bwTicket.Price;
            ticket.FwPrice = fwTicket.Price;
            ticket.BwPrice = bwTicket.Price;

            ticket.FwDepartureCity = fwTicket.DepartureCity;
            ticket.FwDepartureAirportName = fwTicket.DepartureAirportName;
            ticket.FwDepartureAirportCountry = fwTicket.DepartureAirportCountry;
            ticket.FwArrivalCity = fwTicket.ArrivalCity;
            ticket.FwArrivalAirportName = fwTicket.ArrivalAirportName;
            ticket.FwArrivalAirportCountry = fwTicket.ArrivalAirportCountry;

            ticket.FwAircompany_name = fwTicket.Aircompany_name;
            ticket.FwStandardClassCapacity = fwTicket.StandardClassCapacity;
            ticket.FwFirstClassCapacity = fwTicket.FirstClassCapacity;

            ticket.FwDepartureDate = fwTicket.DepartureDate;
            ticket.FwDepartureTime = fwTicket.DepartureTime;
            ticket.FwArrivalDate = fwTicket.ArrivalDate;
            ticket.FwArrivalTime = fwTicket.ArrivalTime;
            ticket.FwFlightDuration = fwTicket.FlightDuration;
            ticket.FwNumberOfTransfers = fwTicket.NumberOfTransfers;

            ticket.BwDepartureCity = bwTicket.DepartureCity;
            ticket.BwDepartureAirportName = bwTicket.DepartureAirportName;
            ticket.BwDepartureAirportCountry = bwTicket.DepartureAirportCountry;
            ticket.BwArrivalCity = bwTicket.ArrivalCity;
            ticket.BwArrivalAirportName = bwTicket.ArrivalAirportName;
            ticket.BwArrivalAirportCountry = bwTicket.ArrivalAirportCountry;

            ticket.BwAircompany_name = bwTicket.Aircompany_name;
            ticket.BwFirstClassCapacity = bwTicket.FirstClassCapacity;
            ticket.BwStandardClassCapacity = bwTicket.StandardClassCapacity;

            ticket.BwDepartureDate = bwTicket.DepartureDate;
            ticket.BwDepartureTime = bwTicket.DepartureTime;
            ticket.BwArrivalDate = bwTicket.ArrivalDate;
            ticket.BwArrivalTime = bwTicket.ArrivalTime;
            ticket.BwFlightDuration = bwTicket.FlightDuration;
            ticket.BwNumberOfTransfers = bwTicket.NumberOfTransfers;
            

            return ticket;
        }
        catch (Exception ex)
        {
            return new TicketListVM();
        }
    }
}