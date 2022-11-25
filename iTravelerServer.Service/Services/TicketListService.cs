using iTravelerServer.DAL;
using iTravelerServer.Domain.Entities;
using iTravelerServer.Domain.Enum;
using iTravelerServer.Domain.Response;
using iTravelerServer.Domain.ViewModels.FlightVM;
using iTravelerServer.Service.Interfaces;

namespace iTravelerServer.Service.Services
{
    public class TicketListService : ITicketListService
    {
        private readonly ApplicationDbContext _db;

        public TicketListService(
            ApplicationDbContext db
        )
        {
            _db = db;
        }

        public BaseResponse<List<TicketListVM>> GetFlightList()
        {
            // var result2 = _db.Airport.ToList();
            // var result1 = _db.Account.ToList();
            // var result3 = _db.Plane.ToList();
            // var result4 = _db.Ticket.ToList();
            // var result5 = _db.Transfer.ToList();
            // var result7 = _db.Order.ToList();
            // var result6 = _db.Flight.ToList();

            //var baseResponse = new BaseResponse<IEnumerable<TicketListVM>>();
            // try
            // {
            //     var fwDepAirport = (from tickets in _db.Ticket
            //         join flights in _db.Flight on tickets.FwFlight_id equals flights.Flight_id
            //         join airports in _db.Airport on flights.DepartureAirport_id equals airports.Airport_id
            //         select airports).ToList();
            //
            //     var fwArrAirport = (from tickets in _db.Ticket
            //         join flights in _db.Flight on tickets.FwFlight_id equals flights.Flight_id
            //         join airports in _db.Airport on flights.ArrivalAirport_id equals airports.Airport_id
            //         select airports).ToList();
            //
            //     var fwValues = (from tickets in _db.Ticket
            //         join flights in _db.Flight on tickets.FwFlight_id equals flights.Flight_id
            //         join planes in _db.Plane on flights.Plane_id equals planes.Plane_id
            //         join transfers in _db.Transfer on flights.Transfer_id equals transfers.Transfer_id
            //         select new
            //         {
            //             tickets.Price,
            //             planes.Aircompany_name,
            //             planes.StandardClassCapacity,
            //             planes.FirstClassCapacity,
            //             flights.DepartureDate,
            //             flights.DepartureTime,
            //             flights.ArrivalDate,
            //             flights.ArrivalTime,
            //             transfers.NumberOfTransfers,
            //             flights.FlightDuration
            //         }).ToList();
            //
            //     // var bwDepAirport = (from tickets in _db.Ticket
            //     //     join flights in _db.Flight on tickets.BwFlight_id equals flights.Flight_id
            //     //     join airports in _db.Airport on flights.DepartureAirport_id equals airports.Airport_id
            //     //     select airports).ToList();
            //     //
            //     // var bwArrAirport = (from tickets in _db.Ticket
            //     //     join flights in _db.Flight on tickets.BwFlight_id equals flights.Flight_id
            //     //     join airports in _db.Airport on flights.ArrivalAirport_id equals airports.Airport_id
            //     //     select airports).ToList();
            //     //
            //     // var bwValues = (from tickets in _db.Ticket
            //     //     join flights in _db.Flight on tickets.BwFlight_id equals flights.Flight_id
            //     //     join planes in _db.Plane on flights.Plane_id equals planes.Plane_id
            //     //     join transfers in _db.Transfer on flights.Transfer_id equals transfers.Transfer_id
            //     //     select new
            //     //     {
            //     //         tickets.Price,
            //     //         planes.Aircompany_name,
            //     //         planes.StandardClassCapacity,
            //     //         planes.FirstClassCapacity,
            //     //         flights.DepartureDate,
            //     //         flights.DepartureTime,
            //     //         flights.ArrivalDate,
            //     //         flights.ArrivalTime,
            //     //         transfers.NumberOfTransfers,
            //     //         flights.FlightDuration
            //     //     }).ToList();
            //     
            //     var ticketList = new List<TicketListVM>();
            //     var ticket = new TicketListVM();
            //     for (int i = 0; i < fwValues.Count; i++)
            //     {
            //         //ticket.TotalPrice = fwValues[i].Price + bwValues[i].Price;
            //         
            //         ticket.FwDepartureCity = fwDepAirport[i].City;
            //         ticket.FwDepartureAirportName = fwDepAirport[i].Name;
            //         ticket.FwDepartureAirportCountry = fwDepAirport[i].Country;
            //         ticket.FwArrivalCity = fwArrAirport[i].City;
            //         ticket.FwArrivalAirportName = fwArrAirport[i].Name;
            //         ticket.FwArrivalAirportCountry = fwArrAirport[i].Country;
            //         
            //         ticket.FwAircompany_name = fwValues[i].Aircompany_name;
            //         ticket.FwStandardClassCapacity = fwValues[i].StandardClassCapacity;
            //         ticket.FwFirstClassCapacity = fwValues[i].FirstClassCapacity;
            //         
            //         ticket.FwDepartureDate = fwValues[i].DepartureDate;
            //         ticket.FwDepartureTime = fwValues[i].DepartureTime;
            //         ticket.FwArrivalDate = fwValues[i].ArrivalDate;
            //         ticket.FwArrivalTime = fwValues[i].ArrivalTime;
            //         ticket.FwFlightDuration = fwValues[i].FlightDuration;
            //         ticket.FwNumberOfTransfers = fwValues[i].NumberOfTransfers;
            //         
            //         // ticket.BwDepartureCity = bwDepAirport[i].City;
            //         // ticket.BwDepartureAirportName = bwDepAirport[i].Name;
            //         // ticket.BwDepartureAirportCountry = bwDepAirport[i].Country;
            //         // ticket.BwArrivalCity = bwArrAirport[i].City;
            //         // ticket.BwArrivalAirportName = bwArrAirport[i].Name;
            //         // ticket.BwArrivalAirportCountry = bwArrAirport[i].Country;
            //         //
            //         // ticket.BwAircompany_name = bwValues[i].Aircompany_name;
            //         // ticket.BwFirstClassCapacity = bwValues[i].FirstClassCapacity;
            //         // ticket.BwStandardClassCapacity = bwValues[i].StandardClassCapacity;
            //         //
            //         // ticket.BwDepartureDate = bwValues[i].DepartureDate;
            //         // ticket.BwDepartureTime = bwValues[i].DepartureTime;
            //         // ticket.BwArrivalDate = bwValues[i].ArrivalDate;
            //         // ticket.BwArrivalTime = bwValues[i].ArrivalTime;
            //         // ticket.BwFlightDuration = bwValues[i].FlightDuration;
            //         // ticket.BwNumberOfTransfers = bwValues[i].NumberOfTransfers;
            //         
            //         ticketList.Add(ticket);
            //     }
            //     return new BaseResponse<List<TicketListVM>>()
            //     {
            //         Data = ticketList,
            //         StatusCode = StatusCode.OK
            //     };
            // }
            return new BaseResponse<List<TicketListVM>>();
            // catch (Exception ex)
            // {
            //     return new BaseResponse<List<TicketListVM>>()
            //     {
            //         Description = $"[GetTicketList] : {ex.Message}",
            //         StatusCode = StatusCode.InternalServerError
            //     };
            // }
        }

        // public BaseResponse<List<TicketListVM>> GetTicketList(List<TicketListVM> flightList,TicketSearchRequest filter)
        // {
        //     var ticketList = new List<TicketListVM>();
        //     foreach (var item in flightList)
        //     {
        //         if (
        //             item.FwArrivalCity== filter.ArrivalCity &&
        //             item.FwDepartureCity==filter.DepartureCity&&
        //             item.FwDepartureDate==filter.DepartureDate&&
        //             item.BwDepartureDate==filter.ReturnDate
        //         )
        //         {
        //             if (filter.FlightClass=="FirstClass" && item.FwFirstClassCapacity>= filter.NumberOfPassangers)
        //             {
        //                 ticketList.Add(item);
        //             }
        //             if (filter.FlightClass=="StandardClass" && item.FwStandardClassCapacity>= filter.NumberOfPassangers)
        //             {
        //                 ticketList.Add(item);
        //             }
        //         }
        //     }
        //     
        //
        //
        // }

        
    }
}