using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using iTravelerServer.DAL;
using iTravelerServer.DAL.Interfaces;
using iTravelerServer.Domain.Entities;
using iTravelerServer.Domain.Enum;
using iTravelerServer.Domain.Response;
using iTravelerServer.Domain.ViewModels.FlightVM;
using iTravelerServer.Domain.ViewModels.OrderVM;
using iTravelerServer.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace iTravelerServer.Service.Services
{
    public class FlightService : IFlightService
    {
        private readonly IBaseRepository<Flight> _flightRepository;
        private readonly IBaseRepository<OrderDetails> _orderDetailsRepository;
        private readonly IBaseRepository<FlightDetails> _flightDetailsRepository;
        private readonly IBaseRepository<Account> _accountRepository;
        private readonly IBaseRepository<Order> _orderRepository;
        private readonly ApplicationDbContext _db;

        public FlightService(IBaseRepository<Flight> flightRepository, IBaseRepository<Account> accountRepository,
            IBaseRepository<Order> orderRepository, IBaseRepository<OrderDetails> orderDetailsRepository,
            IBaseRepository<FlightDetails> flightDetailsRepository, ApplicationDbContext db)
        {
            _flightRepository = flightRepository;
            _orderDetailsRepository = orderDetailsRepository;
            _flightDetailsRepository = flightDetailsRepository;
            _accountRepository = accountRepository;
            _orderRepository = orderRepository;
            _db = db;
        }

        DateTime GetRandomDate(DateTime date)
        {
            return date + GetRandomTime(0, 24);
        }

        TimeSpan GetRandomTime(int hoursFrom, int hoursTo)
        {
            var totalHours = hoursTo - hoursFrom;
            Random random = new Random();
            var parts = random.Next(0, totalHours * 12);
            var minutes = parts * 5 + hoursFrom * 12 * 5;
            var ts = new TimeSpan(0, minutes, 0);

            // string time = ts.ToString("hh':'mm");
            return ts;
        }

        int GetAirportIndex(List<Airport> allAirports, int airportIndex, int currId)
        {
            int value = currId;
            if (allAirports[airportIndex].Airport_id != currId)
            {
                value = allAirports[airportIndex].Airport_id;
            }
            else if (airportIndex++ <= allAirports.Count)
            {
                value = allAirports[airportIndex++].Airport_id;
            }

            return value;
        }

        public List<Flight> FlightGenerator(int numberOfDaysFromToday, int numFlightFromOneCity)
        {
            var flightDate = DateTime.Today;
            var endDate = DateTime.Today.AddDays(numberOfDaysFromToday);
            List<Flight> flightList = new List<Flight>();


            // var depDate = GetRandomDate(flightDate);
            //
            // var arrDate = depDate + GetRandomTime(5,9);
            //
            // flightList.Add("dep "+depDate+" arr "+arrDate);


            var allAirports = GetAirports();
            var allPlanes = GetPlanes();

            var flightDetailsList =
                FlightDetailGenerator(allAirports.Count *allAirports.Count-1 * numberOfDaysFromToday *numFlightFromOneCity);
            var flightDetailIndex = 0;
            Flight flight = new Flight();
            while (flightDate < endDate)
            {
                foreach (var airport in allAirports)
                {
                    foreach (var arrAirport in allAirports)
                    {
                        if (airport.Airport_id != arrAirport.Airport_id)
                        {
                            for (int i = 0; i < numFlightFromOneCity; i++)
                            {
                                var depDate = GetRandomDate(flightDate);
                                var duration = GetRandomTime(2, 5);
                                var arrDate = depDate + duration;
                                var plane_id = allPlanes[GetRandomInt(0, allPlanes.Count)].Plane_id;

                                flight = new Flight()
                                {
                                    FlightDetail_id = flightDetailsList[flightDetailIndex],
                                    Plane_id = plane_id,
                                    DepartureDate = depDate,
                                    ArrivalDate = arrDate,
                                    FlightDuration = duration.ToString("hh':'mm"),
                                    DepartureTime = depDate.TimeOfDay.ToString("hh':'mm"),
                                    ArrivalTime = arrDate.TimeOfDay.ToString("hh':'mm"),
                                    TimeZone = "UTC",
                                    Transfer_id = 1,
                                    DepartureAirport_id = airport.Airport_id,
                                    ArrivalAirport_id = arrAirport.Airport_id
                                };
                                _flightRepository.Create(flight);

                                if (flight.Flight_id != 0)
                                {
                                    flightList.Add(flight);
                                    flightDetailIndex++;
                                }
                            }
                        }
                    }
                }

                flightDate = flightDate.AddDays(1);
            }


            return flightList;
        }

        public List<int> FlightDetailGenerator(int number)
        {
            List<int> flightDetailsId = new List<int>();
            for (int i = 0; i < number; i++)
            {
                var flightDetail = new FlightDetails()
                {
                    BaggageСompartmentsLeft = 1,
                    BaggageSurchargePrice = 7,
                    BusinessBaggagePlacesForPerson = 2,
                    BusinessClassSeatLeft = 1,
                    StandardSeatLeft = 1,
                    BusinessClassSeatPrice = GetRandomInt(30, 60) * 10,
                    StandardSeatPrice = GetRandomInt(10, 30) * 10,
                    BusinessFacility_id = 2,
                    StandardFacility_id = 1,
                    ExtraBaggagePrice = 40,
                    MaxExtraBaggagePlacesForPerson = 3,
                    StandardBaggagePlacesForPerson = 1
                };
                _flightDetailsRepository.Create(flightDetail);

                if (flightDetail.FlightDetail_id != 0)
                {
                    flightDetailsId.Add(flightDetail.FlightDetail_id);
                }
            }

            return flightDetailsId;
        }

        int GetRandomInt(int min, int max)
        {
            Random rnd = new Random();
            return rnd.Next(min, max);
        }

        List<Plane> GetPlanes()
        {
            var planesList = (from planes in _db.Plane select planes).ToList();

            return planesList;
        }

        List<Airport> GetAirports()
        {
            var airportsList = (from airports in _db.Airport select airports).ToList();
            return airportsList;
        }


        // public async Task<BaseResponse<Flight>> AddFlight(Flight flightData)
        // {
        //     var baseResponse = new BaseResponse<Flight>();
        //     try
        //     {
        //         await _flightRepository.Create(flightData);
        //
        //         return new BaseResponse<Flight>()
        //         {
        //             Description = "flight created",
        //             StatusCode = StatusCode.OK
        //         };
        //     }
        //     catch (Exception ex)
        //     {
        //         return new BaseResponse<Flight>()
        //         {
        //             Description = $"[AddFlight] : {ex.Message}",
        //             StatusCode = StatusCode.InternalServerError
        //         };
        //     }
        // }

        public BaseResponse<List<FlightListVM>> GetFlightList()
        {
            try
            {
                //tickets in _db.Ticket join on tickets.FwFlight_id equals flights.Flight_id
                var allFlights = _flightRepository.GetAll().ToList();
                var depAirport = (from flights in _db.Flight
                    join airports in _db.Airport on flights.DepartureAirport_id equals airports.Airport_id
                    select airports).ToList();

                var arrAirport = (from flights in _db.Flight
                    join airports in _db.Airport on flights.ArrivalAirport_id equals airports.Airport_id
                    select airports).ToList();

                var flightDetails = (from flights in _db.Flight
                    join details in _db.FlightDetails on flights.FlightDetail_id equals details.FlightDetail_id
                    select details).ToList();
                var standardFacility = (from flights in _db.Flight
                    join details in _db.FlightDetails on flights.FlightDetail_id equals details.FlightDetail_id
                    join facilities in _db.Facility on details.StandardFacility_id equals facilities.Facility_id
                    select facilities).ToList();
                var businessFacility = (from flights in _db.Flight
                    join details in _db.FlightDetails on flights.FlightDetail_id equals details.FlightDetail_id
                    join facilities in _db.Facility on details.BusinessFacility_id equals facilities.Facility_id
                    select facilities).ToList();

                var fwValues = (from flights in _db.Flight
                    join planes in _db.Plane on flights.Plane_id equals planes.Plane_id
                    join transfers in _db.Transfer on flights.Transfer_id equals transfers.Transfer_id
                    join baggage in _db.Baggage on planes.Baggage_id equals baggage.Baggage_id
                    select new
                    {
                        planes.Aircompany_name,
                        planes.Plane_id,
                        planes.StandardClassCapacity,
                        planes.Name,
                        planes.BusinessSeatsInRow,
                        planes.StandardSeatsInRow,
                        planes.BusinessClassCapacity,
                        // planes.FirstClassCapacity,
                        transfers.NumberOfTransfers,
                        transfers.FirstTransferDuration,
                        transfers.FirstTransferCity,
                        baggage
                    }).ToList();

                var flightList = new List<FlightListVM>();
                for (int i = 0; i < fwValues.Count; i++)
                {
                    var flight = new FlightListVM();
                    // if (isAvailable(flightDetails[i], allFlights[i], depAirport[i],arrAirport[i], filter.DepartureDate, filter.ArrivalCity,
                    //         filter.DepartureCity, filter, flight))
                    // {

                    flight.MaxBaggageWeight = fwValues[i].baggage.MaxBaggageWeight;
                    flight.StandardBaggageWeight = fwValues[i].baggage.StandardBaggageWeight;
                    flight.BusinessBaggageWeight = fwValues[i].baggage.BusinessBaggageWeight;
                    flight.StandardhandLuggageWeight = fwValues[i].baggage.StandardhandLuggageWeight;
                    flight.BusinesshandLuggageWeight = fwValues[i].baggage.BusinesshandLuggageWeight;

                    flight.Flight_id = allFlights[i].Flight_id;
                    flight.StandardSeatPrice = flightDetails[i].StandardSeatPrice;
                    flight.BusinessClassSeatPrice = flightDetails[i].BusinessClassSeatPrice;

                    flight.StandardWiFi = standardFacility[i].WiFi;
                    flight.StandardFood = standardFacility[i].Food;
                    flight.StandardDrink = standardFacility[i].Drink;
                    flight.StandardEntertainment = standardFacility[i].Entertainment;
                    flight.StandardUSB = standardFacility[i].USB;

                    flight.BusinessWiFi = businessFacility[i].WiFi;
                    flight.BusinessFood = businessFacility[i].Food;
                    flight.BusinessDrink = businessFacility[i].Drink;
                    flight.BusinessEntertainment = businessFacility[i].Entertainment;
                    flight.BusinessUSB = businessFacility[i].USB;

                    // flight.FirstClassSeatPrice = flightDetails[i].FirstClassSeatPrice;

                    flight.StandardClassCapacity = fwValues[i].StandardClassCapacity;
                    flight.BusinessClassCapacity = fwValues[i].BusinessClassCapacity;
                    flight.PlaneName = fwValues[i].Name;
                    flight.PlaneId = fwValues[i].Plane_id;
                    flight.BusinessSeatsInRow = fwValues[i].BusinessSeatsInRow;
                    flight.StandardSeatsInRow = fwValues[i].StandardSeatsInRow;
                    // flight.FirstClassCapacity = fwValues[i].FirstClassCapacity;

                    flight.DepartureCity = depAirport[i].City;
                    flight.DepartureAirportName = depAirport[i].Name;
                    flight.DepartureAirportCountry = depAirport[i].Country;
                    flight.ArrivalCity = arrAirport[i].City;
                    flight.ArrivalAirportName = arrAirport[i].Name;
                    flight.ArrivalAirportCountry = arrAirport[i].Country;
                    flight.Aircompany_name = fwValues[i].Aircompany_name;
                    flight.StandardSeatLeft = flightDetails[i].StandardSeatLeft;
                    flight.BusinessClassSeatLeft = flightDetails[i].BusinessClassSeatLeft;
                    flight.ExtraBaggagePrice = flightDetails[i].ExtraBaggagePrice;
                    flight.MaxExtraBaggagePlacesForPerson = flightDetails[i].MaxExtraBaggagePlacesForPerson;
                    flight.BaggageSurchargePrice = flightDetails[i].BaggageSurchargePrice;
                    flight.StandardBaggagePlacesForPerson = flightDetails[i].StandardBaggagePlacesForPerson;
                    flight.BusinessBaggagePlacesForPerson = flightDetails[i].BusinessBaggagePlacesForPerson;
                    // flight.FirstClassSeatLeft = flightDetails[i].FirstClassSeatLeft;
                    flight.DepartureDate = allFlights[i].DepartureDate;
                    flight.TimeZone = allFlights[i].TimeZone;
                    flight.DepartureTime = allFlights[i].DepartureTime;
                    flight.ArrivalDate = allFlights[i].ArrivalDate;
                    flight.ArrivalTime = allFlights[i].ArrivalTime;
                    flight.FlightDuration = allFlights[i].FlightDuration;
                    
                    flight.NumberOfTransfers = fwValues[i].NumberOfTransfers;
                    flight.FirstTransferDuration = fwValues[i].FirstTransferDuration;
                    flight.FirstTransferCity = fwValues[i].FirstTransferCity;

                    flightList.Add(flight);
                    //}
                }

                return new BaseResponse<List<FlightListVM>>()
                {
                    Data = flightList,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<FlightListVM>>()
                {
                    Description = $"[GetFlightList] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public BaseResponse<List<TicketListVM>> GetTicketList(List<FlightListVM> flightList, SearchRequest filter)
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
                            filter.DepartureCity) && IsPriceSet(item, filter.FlightClass, filter.NumberOfPassangers))
                    {
                        fwTicketList.Add(item);
                    }

                    if (isAvailable(item, filter.ReturnDate, filter.DepartureCity,
                            filter.ArrivalCity) && IsPriceSet(item, filter.FlightClass, filter.NumberOfPassangers))
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
                            if (fwTicket.PlaneId != bwTicket.PlaneId)
                            {
                                var ticket = new TicketListVM();
                                ticket.TicketElem_id = i;
                                ticket.TotalPrice = fwTicket.Price + bwTicket.Price;
                                ticket.FlightClass = filter.FlightClass;
                                ticket.isOneSide = false;
                                ticket = fillFwTicket(fwTicket, ticket, filter.NumberOfPassangers);
                                ticket = fillBwTicket(bwTicket, ticket);
                                ticketList.Add(ticket);
                                i++;
                            }
                            //ticketList.Add(createTicket(fwTicket, bwTicket, filter.NumberOfPassangers, i));
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

        public BaseResponse<List<TicketListVM>> GetOneSideTicketList(List<FlightListVM> flightList,
            SearchRequest filter)
        {
            try
            {
                var sortedTicketList = new List<FlightListVM>();
                var ticketList = new List<TicketListVM>();
                filter.DepartureDate = filter.DepartureDate.AddDays(1);
                //filter.ReturnDate = filter.ReturnDate.AddDays(1);
                foreach (var item in flightList)
                {
                    if (isAvailable(item, filter.DepartureDate, filter.ArrivalCity,
                            filter.DepartureCity) && IsPriceSet(item, filter.FlightClass, filter.NumberOfPassangers))
                    {
                        sortedTicketList.Add(item);
                    }
                }

                if (sortedTicketList.Count != 0)
                {
                    int i = 0;
                    foreach (var flightElem in sortedTicketList)
                    {
                        var ticket = new TicketListVM();
                        ticket.TicketElem_id = i;
                        ticket.TotalPrice = flightElem.Price;
                        ticket.FlightClass = filter.FlightClass;
                        ticket.isOneSide = true;
                        ticket = fillFwTicket(flightElem, ticket, filter.NumberOfPassangers);
                        ticket.BwFlight = ticket.FwFlight;
                        ticketList.Add(ticket);
                        i++;
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

        public bool IsPriceSet(FlightListVM item, string FlightClass, int NumberOfPassangers)
        {
            if (FlightClass == "BusinessClass" &&
                item.BusinessClassSeatLeft >= NumberOfPassangers)
            {
                item.Price = NumberOfPassangers * item.BusinessClassSeatPrice;
                return true;
            }

            if (FlightClass == "StandardClass" &&
                item.StandardSeatLeft >= NumberOfPassangers)
            {
                item.Price = NumberOfPassangers * item.StandardSeatPrice;
                return true;
            }

            return false;
        }

        private bool isAvailable(FlightListVM item, DateTime DepartureDate, string ArrivalCity,
            string DepartureCity)
        {
            if (
                item.ArrivalCity.ToLower() == ArrivalCity.ToLower() &&
                item.DepartureCity.ToLower() == DepartureCity.ToLower()
            )
            {
                if (item.DepartureDate.Date == DepartureDate.Date)
                {
                    return true;

                    // if (FlightClass == "BusinessClass" &&
                    //     item.BusinessClassSeatLeft >= NumberOfPassangers)
                    // {
                    //     item.Price = NumberOfPassangers * item.BusinessClassSeatPrice;
                    //     return true;
                    // }
                    //
                    // if (FlightClass == "StandardClass" &&
                    //     item.StandardSeatLeft >= NumberOfPassangers)
                    // {
                    //     item.Price = NumberOfPassangers * item.StandardSeatPrice;
                    //     return true;
                    // }
                }
            }

            return false;
        }

        public TicketListVM fillFwTicket(FlightListVM flightElem, TicketListVM ticket, int NumberOfPassangers
            //int i
        )
        {
            try
            {
                ticket.FwFlight = flightElem;
                //var ticket = new TicketListVM();
                //ticket.TicketElem_id = i;
                ticket.FwFlight_id = flightElem.Flight_id;
                ticket.FwPrice = flightElem.Price;

                ticket.FwMaxBaggageWeight = flightElem.MaxBaggageWeight;
                ticket.FwStandardBaggageWeight = flightElem.StandardBaggageWeight;
                ticket.FwBusinessBaggageWeight = flightElem.BusinessBaggageWeight;
                ticket.FwStandardhandLuggageWeight = flightElem.StandardhandLuggageWeight;
                ticket.FwBusinesshandLuggageWeight = flightElem.BusinesshandLuggageWeight;
                ticket.FwExtraBaggagePrice = flightElem.ExtraBaggagePrice;
                ticket.FwMaxExtraBaggagePlacesForPerson = flightElem.MaxExtraBaggagePlacesForPerson;
                ticket.FwBaggageSurchargePrice = flightElem.BaggageSurchargePrice;
                ticket.FwStandardBaggagePlacesForPerson = flightElem.StandardBaggagePlacesForPerson;
                ticket.FwBusinessBaggagePlacesForPerson = flightElem.BusinessBaggagePlacesForPerson;

                // ticket.FlightClass = flightElem.FlightClass;
                ticket.NumberOfPassengers = NumberOfPassangers;
                ticket.FwDepartureCity = flightElem.DepartureCity;
                ticket.FwDepartureAirportName = flightElem.DepartureAirportName;
                ticket.FwDepartureAirportCountry = flightElem.DepartureAirportCountry;
                ticket.FwArrivalCity = flightElem.ArrivalCity;
                ticket.FwArrivalAirportName = flightElem.ArrivalAirportName;
                ticket.FwArrivalAirportCountry = flightElem.ArrivalAirportCountry;

                ticket.FwAircompany_name = flightElem.Aircompany_name;

                ticket.FwStandardClassCapacity = flightElem.StandardClassCapacity;
                ticket.FwBusinessClassCapacity = flightElem.BusinessClassCapacity;
                //ticket.FwFirstClassCapacity = flightElem.FirstClassCapacity;

                ticket.FwDepartureDate = flightElem.DepartureDate;
                ticket.FwDepartureTime = flightElem.DepartureTime;
                ticket.FwArrivalDate = flightElem.ArrivalDate;
                ticket.FwArrivalTime = flightElem.ArrivalTime;
                ticket.FwFlightDuration = flightElem.FlightDuration;
                ticket.FwNumberOfTransfers = flightElem.NumberOfTransfers;

                return ticket;
            }
            catch (Exception ex)
            {
                return new TicketListVM();
            }
        }

        public TicketListVM fillBwTicket(FlightListVM flightElem, TicketListVM ticket)
        {
            try
            {
                ticket.BwFlight = flightElem;

                ticket.BwFlight_id = flightElem.Flight_id;
                ticket.BwPrice = flightElem.Price;

                ticket.BwMaxBaggageWeight = flightElem.MaxBaggageWeight;
                ticket.BwStandardBaggageWeight = flightElem.StandardBaggageWeight;
                ticket.BwBusinessBaggageWeight = flightElem.BusinessBaggageWeight;
                ticket.BwStandardhandLuggageWeight = flightElem.StandardhandLuggageWeight;
                ticket.BwBusinesshandLuggageWeight = flightElem.BusinesshandLuggageWeight;
                ticket.BwExtraBaggagePrice = flightElem.ExtraBaggagePrice;
                ticket.BwMaxExtraBaggagePlacesForPerson = flightElem.MaxExtraBaggagePlacesForPerson;
                ticket.BwBaggageSurchargePrice = flightElem.BaggageSurchargePrice;
                ticket.BwStandardBaggagePlacesForPerson = flightElem.StandardBaggagePlacesForPerson;
                ticket.BwBusinessBaggagePlacesForPerson = flightElem.BusinessBaggagePlacesForPerson;

                ticket.BwDepartureCity = flightElem.DepartureCity;
                ticket.BwDepartureAirportName = flightElem.DepartureAirportName;
                ticket.BwDepartureAirportCountry = flightElem.DepartureAirportCountry;
                ticket.BwArrivalCity = flightElem.ArrivalCity;
                ticket.BwArrivalAirportName = flightElem.ArrivalAirportName;
                ticket.BwArrivalAirportCountry = flightElem.ArrivalAirportCountry;
                ticket.BwAircompany_name = flightElem.Aircompany_name;
                // ticket.BwFirstClassCapacity = flightElem.FirstClassCapacity;
                ticket.BwBusinessClassCapacity = flightElem.BusinessClassCapacity;
                ticket.BwStandardClassCapacity = flightElem.StandardClassCapacity;
                ticket.BwDepartureDate = flightElem.DepartureDate;
                ticket.BwDepartureTime = flightElem.DepartureTime;
                ticket.BwArrivalDate = flightElem.ArrivalDate;
                ticket.BwArrivalTime = flightElem.ArrivalTime;
                ticket.BwFlightDuration = flightElem.FlightDuration;
                ticket.BwNumberOfTransfers = flightElem.NumberOfTransfers;
                return ticket;
            }
            catch (Exception ex)
            {
                return new TicketListVM();
            }
        }

        // public TicketListVM createTicket(
        //     FlightListVM fwTicket,
        //     FlightListVM bwTicket,
        //     int NumberOfPassangers,
        //     int i
        // )
        // {
        //     try
        //     {
        //         var ticket = new TicketListVM();
        //         ticket.TicketElem_id = i;
        //         ticket.FwFlight_id = fwTicket.Flight_id;
        //         ticket.BwFlight_id = bwTicket.Flight_id;
        //         ticket.NumberOfPassengers = NumberOfPassangers;
        //         ticket.FlightClass = fwTicket.FlightClass;
        //
        //         ticket.TotalPrice = fwTicket.Price + bwTicket.Price;
        //         ticket.FwPrice = fwTicket.Price;
        //         ticket.BwPrice = bwTicket.Price;
        //
        //         ticket.FwDepartureCity = fwTicket.DepartureCity;
        //         ticket.FwDepartureAirportName = fwTicket.DepartureAirportName;
        //         ticket.FwDepartureAirportCountry = fwTicket.DepartureAirportCountry;
        //         ticket.FwArrivalCity = fwTicket.ArrivalCity;
        //         ticket.FwArrivalAirportName = fwTicket.ArrivalAirportName;
        //         ticket.FwArrivalAirportCountry = fwTicket.ArrivalAirportCountry;
        //
        //         ticket.FwAircompany_name = fwTicket.Aircompany_name;
        //
        //         ticket.FwStandardClassCapacity = fwTicket.StandardClassCapacity;
        //         ticket.FwBusinessClassCapacity = fwTicket.BusinessClassCapacity;
        //         //ticket.FwFirstClassCapacity = fwTicket.FirstClassCapacity;
        //
        //         ticket.FwDepartureDate = fwTicket.DepartureDate;
        //         ticket.FwDepartureTime = fwTicket.DepartureTime;
        //         ticket.FwArrivalDate = fwTicket.ArrivalDate;
        //         ticket.FwArrivalTime = fwTicket.ArrivalTime;
        //         ticket.FwFlightDuration = fwTicket.FlightDuration;
        //         ticket.FwNumberOfTransfers = fwTicket.NumberOfTransfers;
        //
        //         ticket.BwDepartureCity = bwTicket.DepartureCity;
        //         ticket.BwDepartureAirportName = bwTicket.DepartureAirportName;
        //         ticket.BwDepartureAirportCountry = bwTicket.DepartureAirportCountry;
        //         ticket.BwArrivalCity = bwTicket.ArrivalCity;
        //         ticket.BwArrivalAirportName = bwTicket.ArrivalAirportName;
        //         ticket.BwArrivalAirportCountry = bwTicket.ArrivalAirportCountry;
        //
        //         ticket.BwAircompany_name = bwTicket.Aircompany_name;
        //         // ticket.BwFirstClassCapacity = bwTicket.FirstClassCapacity;
        //         ticket.BwBusinessClassCapacity = bwTicket.BusinessClassCapacity;
        //         ticket.BwStandardClassCapacity = bwTicket.StandardClassCapacity;
        //
        //         ticket.BwDepartureDate = bwTicket.DepartureDate;
        //         ticket.BwDepartureTime = bwTicket.DepartureTime;
        //         ticket.BwArrivalDate = bwTicket.ArrivalDate;
        //         ticket.BwArrivalTime = bwTicket.ArrivalTime;
        //         ticket.BwFlightDuration = bwTicket.FlightDuration;
        //         ticket.BwNumberOfTransfers = bwTicket.NumberOfTransfers;
        //
        //
        //         return ticket;
        //     }
        //     catch (Exception ex)
        //     {
        //         return new TicketListVM();
        //     }
        // }


        public async Task<BaseResponse<IEnumerable<Flight>>> GetFlights()
        {
            var baseResponse = new BaseResponse<IEnumerable<Flight>>();
            try
            {
                var flights = await _flightRepository.GetAll().ToListAsync();

                if (flights != null)
                {
                    return new BaseResponse<IEnumerable<Flight>>()
                    {
                        Data = flights,
                        StatusCode = StatusCode.OK
                    };
                }

                return new BaseResponse<IEnumerable<Flight>>()
                {
                    Description = "There aren't flights",
                    StatusCode = StatusCode.NotFound
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<Flight>>()
                {
                    Description = $"[GetFlights] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public BaseResponse<Flight> GetFlight(int flightId)
        {
            var baseResponse = new BaseResponse<Flight>();
            try
            {
                var flight = _flightRepository.Get(flightId);

                baseResponse.Data = flight;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<Flight>()
                {
                    Description = $"[GetFlight] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        // public BaseResponse<bool> UpdateFlight(int flightId, Flight flightData)
        // {
        //     var baseResponse = new BaseResponse<bool>();
        //     try
        //     {
        //         var dbFlight = _flightRepository.Get(flightId);
        //         if (dbFlight == null)
        //         {
        //             baseResponse.Description = "Flight not found";
        //             baseResponse.StatusCode = StatusCode.NotFound;
        //             return baseResponse;
        //         }
        //
        //
        //         dbFlight.DepartureDate = flightData.DepartureDate;
        //         dbFlight.ArrivalDate = flightData.ArrivalDate;
        //         dbFlight.DepartureTime = flightData.DepartureTime;
        //         dbFlight.ArrivalTime = flightData.ArrivalTime;
        //         dbFlight.FlightDuration = flightData.FlightDuration;
        //         dbFlight.StandardClassPrice = flightData.StandardClassPrice;
        //         dbFlight.FirstClassPrice = flightData.FirstClassPrice;
        //
        //         var a = dbFlight;
        //         var res = _flightRepository.UpdateSync(dbFlight);
        //
        //         return new BaseResponse<bool>()
        //         {
        //             Description = "Updated",
        //             StatusCode = StatusCode.OK
        //         };
        //     }
        //     catch (Exception ex)
        //     {
        //         return new BaseResponse<bool>()
        //         {
        //             Description = $"[UpdateFlight] : {ex.Message}",
        //             StatusCode = StatusCode.InternalServerError
        //         };
        //     }
        // }


        //!!!!!!!!!!!!!!!!!!!!!
        //!!!!!!!!!!!!!!!!!!!!!
        //!!!!!!!!!!!!!!!!!!!!!


        // public void CheckBaggageExtraPayment(OrderDetails orderDetail,int NumberOfTickets, int flight_id, int extraBaggageWeight)
        // {
        //     var flightDetails = (from flight in _db.Flight
        //         join details in _db.FlightDetails on flight.FlightDetail_id equals details.FlightDetail_id
        //         select new
        //         {
        //             flight.Flight_id,
        //             details
        //         }).FirstOrDefault(x => x.Flight_id == flight_id);
        //
        //     int permittedNumberOfBaggagePlaces=0;
        //     if (orderDetail.FlightClass == "StandardClass")
        //         permittedNumberOfBaggagePlaces = flightDetails.details.StandardBaggagePlacesForPerson;
        //     if (orderDetail.FlightClass == "BusinessClass")
        //         permittedNumberOfBaggagePlaces = flightDetails.details.BusinessBaggagePlacesForPerson;
        //
        //     int extraPayment = 0;
        //     if (orderDetail.NumberOfBaggagePlaces>permittedNumberOfBaggagePlaces*NumberOfTickets)
        //     {
        //         extraPayment =
        //             (orderDetail.NumberOfBaggagePlaces - (permittedNumberOfBaggagePlaces * NumberOfTickets)) *
        //             flightDetails.details.ExtraBaggagePrice;
        //     }
        //
        //     if (extraBaggageWeight>0)
        //     {
        //         extraPayment += extraBaggageWeight * flightDetails.details.BaggageSurchargePrice;
        //     }
        //
        //     orderDetail.Price += extraPayment;
        //     //return orderDetail;
        // }
        //
        // public BaseResponse<OrderDetails> AddOrderDetails(OrderDetails orderDetail, List<int> listOfFreePlaces)
        // {
        //     // var baseResponse = new BaseResponse<Ticket>();
        //     try
        //     {
        //         bool val = false;
        //         bool val2 = false;
        //         //var freeFwPlaces = GetFreeFwPlaces(ticketVm.FwFlight_id, ticketVm.FlightClass);
        //         List<int> orderdetailList = new List<int>();
        //         SplitNumbersString(orderDetail.SeatNumbers, orderdetailList);
        //         //var freeBwPlaces = GetFreeBwPlaces(ticketVm.BwFlight_id, ticketVm.FlightClass);
        //         // for (int i=0;i<orderDetail.NumberOfTickets;i++)
        //         // {
        //         foreach (var elem in orderdetailList)
        //         {
        //             if (!IsPlaceAvailable(listOfFreePlaces, elem))
        //             {
        //                 return new BaseResponse<OrderDetails>()
        //                 {
        //                     Description = "OrderDetail: place is not free",
        //
        //                     StatusCode = StatusCode.NotFound
        //                 };
        //             }
        //         }
        //
        //         _orderDetailsRepository.Create(orderDetail);
        //
        //         if (orderDetail.Order_Detail_id != 0)
        //         {
        //             return new BaseResponse<OrderDetails>()
        //             {
        //                 Description = "Ticket created",
        //                 Data = orderDetail,
        //                 StatusCode = StatusCode.OK
        //             };
        //         }
        //
        //         return new BaseResponse<OrderDetails>()
        //         {
        //             Description = "OrderDetail was not created",
        //
        //             StatusCode = StatusCode.InternalServerError
        //         };
        //     }
        //     catch (Exception ex)
        //     {
        //         return new BaseResponse<OrderDetails>()
        //         {
        //             Description = $"[CreateOrderDetail] : {ex.Message}",
        //             StatusCode = StatusCode.InternalServerError
        //         };
        //     }
        // }
        //
        // public async Task<BaseResponse<Order>> AddOrder(OrderDetails fwOrderDetails, OrderDetails bwOrderDetails,
        //     OrderDetailsVM orderDetailsVM)
        // {
        //     try
        //     {
        //         var acc = new Account()
        //         {
        //             Email = orderDetailsVM.Email
        //         };
        //         var account = _accountRepository.Get(acc);
        //         if (account == null || account.Password == null)
        //             return null;
        //
        //         var fwFlightData = (from flight in _db.Flight
        //             select flight).Where(f => f.Flight_id == orderDetailsVM.FwFlight_id).FirstOrDefault();
        //
        //         if (fwFlightData.DepartureDate.Date > DateTime.Today.AddDays(1).Date)
        //         {
        //             
        //             var expirationDate = new DateTime();
        //
        //             if (DateTime.Today.AddDays(8).Date <= fwFlightData.DepartureDate.Date)
        //                 expirationDate = DateTime.Today.AddDays(7).Date;
        //             else expirationDate = fwFlightData.DepartureDate.AddDays(-1).Date;
        //
        //             var order = new Order()
        //             {
        //                 FwFlight_id = orderDetailsVM.FwFlight_id,
        //                 BwFlight_id = orderDetailsVM.BwFlight_id,
        //                 FwOrderDetail_id = fwOrderDetails.Order_Detail_id,
        //                 BwOrderDetail_id = bwOrderDetails.Order_Detail_id,
        //                 TotalPrice = fwOrderDetails.Price+bwOrderDetails.Price,
        //                 Account_id = account.Account_id,
        //                 NumberOfTickets = orderDetailsVM.NumberOfTickets,
        //                 CreationDate = DateTime.Today,
        //                 ExpirationDate = expirationDate
        //             };
        //             if (orderDetailsVM.IsOneSide)
        //                 order.NumberOfFlights = 1;
        //             else order.NumberOfFlights = 2;
        //
        //             await _orderRepository.Create(order);
        //
        //             ChangeFlightDetailData(fwOrderDetails.FlightClass,order.FwFlight_id,order.NumberOfTickets,fwOrderDetails.NumberOfBaggagePlaces);
        //             
        //             if (order.FwFlight_id!=order.BwFlight_id)
        //             {
        //                 
        //                 ChangeFlightDetailData(bwOrderDetails.FlightClass,order.BwFlight_id,order.NumberOfTickets,bwOrderDetails.NumberOfBaggagePlaces);
        //             }
        //
        //             if (order.Order_id != 0)
        //             {
        //                 return new BaseResponse<Order>()
        //                 {
        //                     Description = "Order created",
        //                     Data = order,
        //                     StatusCode = StatusCode.OK
        //                 };
        //             }
        //         }
        //
        //         return new BaseResponse<Order>()
        //         {
        //             Description = "Order doesn't created",
        //             StatusCode = StatusCode.InvalidDate
        //         };
        //     }
        //     catch (Exception ex)
        //     {
        //         return new BaseResponse<Order>()
        //         {
        //             Description = $"[AddOrder] : {ex.Message}",
        //             StatusCode = StatusCode.InternalServerError
        //         };
        //     }
        // }

        // private void ChangeFlightDetailData(string FlightClass,int flight_id, int numberOfTickets,int numberOfBaggage)
        // {
        //     var flightDetails = (from flight in _db.Flight
        //         join details in _db.FlightDetails on flight.FlightDetail_id equals details.FlightDetail_id
        //         where flight.Flight_id==flight_id
        //         select details).FirstOrDefault();
        //     
        //     flightDetails.BaggageСompartmentsLeft -= numberOfBaggage;
        //     if (FlightClass == "BusinessClass" && flightDetails.BusinessClassSeatLeft>numberOfTickets)
        //     {
        //         flightDetails.BusinessClassSeatLeft -= numberOfTickets;
        //         _flightDetailsRepository.UpdateSync(flightDetails);
        //     }
        //     if (FlightClass == "StandardClass" && flightDetails.StandardSeatLeft>numberOfTickets)
        //     {
        //         flightDetails.StandardSeatLeft -= numberOfTickets;
        //         _flightDetailsRepository.UpdateSync(flightDetails);
        //     }
        // }
        // private bool IsPlaceAvailable(List<int> listOfFreePlaces, int place)
        // {
        //     if (place != 0)
        //     {
        //         foreach (var item in listOfFreePlaces)
        //         {
        //             if (item == place)
        //             {
        //                 return true;
        //             }
        //         }
        //
        //         return false;
        //     }
        //
        //     return true;
        // }

        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        public List<int> GetFreePlaces(int FlightId, string FlightClass, List<int> listOfUsedValues)
        {
            try
            {
                // var listOfUsedValues = new List<int>();
                var listOfFreevalues = new List<int>();
                var plane = (from flight in _db.Flight
                    join planes in _db.Plane on flight.Plane_id equals planes.Plane_id
                    select new
                    {
                        flight.Flight_id,
                        planes.BusinessClassCapacity,
                        planes.StandardClassCapacity
                    }).FirstOrDefault(x => x.Flight_id == FlightId);

                if (FlightClass == "BusinessClass")
                {
                    listOfFreevalues =
                        getListOfFreeValues(listOfFreevalues, listOfUsedValues, plane.BusinessClassCapacity);
                }

                if (FlightClass == "StandardClass")
                {
                    listOfFreevalues =
                        getListOfFreeValues(listOfFreevalues, listOfUsedValues, plane.StandardClassCapacity);
                }

                return listOfFreevalues;
            }
            catch (Exception ex)
            {
                return new List<int>();
            }
        }

        public string FreePlacesToString(List<int> listOfFreevalues)
        {
            string fwPlaces = "";
            foreach (var item in listOfFreevalues)
            {
                fwPlaces += item.ToString() + ",";
            }

            return fwPlaces;
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
    }
}